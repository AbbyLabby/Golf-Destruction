using Cinemachine;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public CinemachineVirtualCamera camera;

    private bool _isStart;
    private bool _isBack;

    public float startFOV;
    public float targetFOV;
    public float zoomMultiplayer;

    private void Awake()
    {
        camera.m_Lens.FieldOfView = startFOV;
    }

    private void Update()
    {
        if (camera.m_Lens.FieldOfView != targetFOV && _isStart && !_isBack)
        {
            ZoomCamera(targetFOV, zoomMultiplayer);
        }

        if (camera.m_Lens.FieldOfView != 90 && _isStart && _isBack)
        {
            ZoomCamera(75, 3);
        }
    }

    public void DoZoom()
    {
        _isStart = true;
    }

    public void DoBack()
    {
        _isBack = true;
    }

    private void ZoomCamera(float target, float speed)
    {
        float angle = Mathf.Abs((90 / 2) - 90);
        camera.m_Lens.FieldOfView =
            Mathf.MoveTowards(camera.m_Lens.FieldOfView, target, angle / speed * Time.deltaTime);
    }
}