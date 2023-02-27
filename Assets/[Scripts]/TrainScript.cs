using DG.Tweening;
using UnityEngine;

public class TrainScript : MonoBehaviour
{
    public Transform target;
    public float duration;

    public Rigidbody rigidbody;

    public void MoveTrain()
    {
        rigidbody.DOMove(target.position, duration);
    }
}
