using DG.Tweening;
using UnityEngine;

public class MoveCarScript : MonoBehaviour
{
    public Transform target;
    public float duration;

    public bool isFall;

    private void Awake()
    {
        transform.DOMove(target.position, duration);
    }

    private void Update()
    {
        if (isFall)
        {
            DOTween.Pause(transform);
        }
    }
}