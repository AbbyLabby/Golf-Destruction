using System;
using UnityEngine;

public class SmallBallScript : MonoBehaviour
{
    public BallScript ballScript;
    
    private void Awake()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(17000, 0,0), ForceMode.Impulse);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Cube") || collision.gameObject.CompareTag("Bridge")) && !ballScript._isCollide)
        {
            ballScript._center = collision.contacts[0].point;

            ballScript._isCollide = true;

            ballScript.DestroyCubes();
            StartCoroutine(ballScript.StartDelay());

                ballScript.camera.LookAt = ballScript.camTarget;
                StartCoroutine(ballScript.DelayZoom());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("FirstBall") && !ballScript.isFirst)
        {
            ballScript.isFirst = true;
            ballScript.camera.m_LookAt = transform;
        }
    }
}
