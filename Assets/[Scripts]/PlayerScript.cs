using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [Header("Turret")] [SerializeField] private Rigidbody turret;

    //[SerializeField] private float rotationMult;
    [SerializeField] private float timeMult;

    public float maxStrength;
    public float backTimer;

    private float _strength;
    private bool _canMove;
    private bool _moveBackward;

    private float _timer;

    public Slider slider;

    private void Awake()
    {
        slider.maxValue = maxStrength;
    }

    private void Update()
    {
        slider.value = _strength;

        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            _canMove = false;
            _strength = Mathf.Clamp(_strength + Time.deltaTime * 10, 0, maxStrength);

            slider.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _canMove = true;
            _timer = backTimer;
            StartCoroutine(HideDelay());
        }

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                _moveBackward = true;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            _moveBackward = false;
        }
    }

    private IEnumerator HideDelay()
    {
        yield return new WaitForSeconds(1f);
        slider.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            turret.MoveRotation(Quaternion.Lerp(Quaternion.identity,
                Quaternion.Euler(0, turret.transform.eulerAngles.y - _strength, 0), Time.fixedDeltaTime * timeMult));
        }

        if (_canMove)
        {
            if (_moveBackward)
            {
                turret.MoveRotation(Quaternion.Lerp(turret.rotation, Quaternion.identity,
                    Time.fixedDeltaTime * timeMult / 2));
                _strength = 0;
            }
            else
            {
                turret.MoveRotation(Quaternion.Lerp(turret.rotation,
                    Quaternion.Euler(0, turret.transform.eulerAngles.y + _strength / 2, 0),
                    Time.fixedDeltaTime * timeMult / 2));
            }
        }
    }
}