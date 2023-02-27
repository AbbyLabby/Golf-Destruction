using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BallScript : MonoBehaviour
{
    private float _radius = .75f;

    public CinemachineVirtualCamera camera;
    public CinemachineVirtualCamera firstCamera;
    public CameraScript cameraScript;

    private int _isStop;
    public bool _isCollide;

    public Vector3 _center;

    public MoveCarScript carScript;

    public TimelineManager timelineManager;

    public TrainScript trainScript;

    public GameObject collideVFX;

    public GameObject smallBall;

    public bool isFirst;

    private bool _isMult;

    public Transform camTarget;

    public Slider progressSlider;

    public TMP_Text text;

    public TMP_Text maxText;

    private float _distance;

    public GameObject volume;

    private void Start()
    {
        _isStop = 0;
        _isCollide = false;

        _distance = Vector3.Distance(transform.position, camTarget.position);

        progressSlider.maxValue = _distance;
        maxText.text = Mathf.RoundToInt(_distance) + "m";

        //gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(5000, 0, 0), ForceMode.Impulse);
    }

    private void Update()
    {
        if (!_isCollide)
        {
            var currentDist = Vector3.Distance(camera.LookAt.transform.position, camTarget.position);

            progressSlider.value = _distance - currentDist;
            text.text = Mathf.RoundToInt(_distance - currentDist) + "m";
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FirstTrigger"))
        {
            if(SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 2) timelineManager.StartFirst();
            // else if(SceneManager.GetActiveScene().buildIndex == 2)
            // {
            //     firstCamera.GetComponent<CameraScript>().DoZoom();
            //     timelineManager.StartFirst();
            // }
            else
            {
                firstCamera.m_LookAt = transform;
                firstCamera.GetComponent<CameraScript>().DoZoom();
            }
        }
        else if(other.CompareTag("SecondTrigger"))
        {
            timelineManager.StartSecond();
        }
        else
        {
            if (other.CompareTag("TrainTrigger"))
            {
                if (trainScript)
                {
                    trainScript.MoveTrain();
                }
            }

            if (other.CompareTag("SlowMoTrigger"))
            {
                Time.timeScale = .1f;
                StartCoroutine(DelaySlowMo());
            }

            if (other.CompareTag("MTrigger") && !_isMult)
            {
                _isMult = true;
                gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(17000, 0,0), ForceMode.Impulse);
                MultiplyBall();
            }
            
            if(other.CompareTag("FirstBall") && !isFirst)
            {
                isFirst = true;
                camera.m_LookAt = transform;
            }
        }
    }

    private void MultiplyBall()
    {
        transform.localScale = new Vector3(2, 2, 2);

        for (int i = 0; i <= 20; i++)
        {
            var newPos = Random.onUnitSphere * Random.Range(.3f, 1);
            
            var gameObj = Instantiate(smallBall, new Vector3(transform.position.x + newPos.x, transform.position.y + newPos.y, transform.position.z + newPos.z), Quaternion.identity);

            gameObj.transform.parent = transform;
            
            gameObj.GetComponent<SmallBallScript>().ballScript = this;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Cube") || collision.gameObject.CompareTag("Bridge")) && !_isCollide)
        {
            _center = collision.contacts[0].point;

            _isCollide = true;

            DestroyCubes();
            StartCoroutine(StartDelay());

            camera.LookAt = camTarget;
            StartCoroutine(DelayZoom());
        }

        if (collision.gameObject.CompareTag("Train"))
        {
            _isCollide = true;
            Instantiate(collideVFX, transform.position, Quaternion.identity);
            volume.SetActive(true);
        }
    }

    private IEnumerator DelaySlowMo()
    {
        yield return new WaitForSecondsRealtime(5);
        Time.timeScale = 1f;
    }

    public void DestroyCubes()
    {
        Collider[] overlappedColliders = Physics.OverlapSphere(_center, 5);

        foreach (var t in overlappedColliders)
        {
            //check what this object has a rigidbody
            var rigidbody = t.attachedRigidbody;
            if (rigidbody && rigidbody.CompareTag("Cube"))
            {
                //add explosion force to the object
                rigidbody.isKinematic = false;
            }
        }
    }

    public IEnumerator DelayZoom()
    {
        yield return new WaitForSeconds(1f);
        cameraScript.DoBack();
    }

    private void Destruction()
    {
        //Debug.Log("Destroy");

        Collider[] overlappedColliders = Physics.OverlapSphere(_center, _radius);

        foreach (var t in overlappedColliders)
        {
            //check what this object has a rigidbody
            var attachedRigidbody = t.attachedRigidbody;
            if (attachedRigidbody)
            {
                if (attachedRigidbody.isKinematic)
                {
                    _isStop++;
                }

                //add explosion force to the object
                attachedRigidbody.isKinematic = false;
            }
        }

        if (_isStop >= 3)
        {
            StartCoroutine(Delay());
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(.1f);
        //Debug.Log("Destroy turn");
        _radius += 1;
        Destruction();
    }

    public IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(1);
        //Debug.Log("Destroy turn");
        carScript.isFall = true;
        _radius += 1;
        Destruction();
    }
}