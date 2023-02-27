using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class GlobalVolume : MonoBehaviour
{
    public Volume volume;
    public GameObject text;
    
    private void Awake()
    {
        StartCoroutine(StartFailed());
    }

    private IEnumerator StartFailed()
    {
        yield return new WaitForSeconds(1f);
        FailedMenu();
    }

    private void FailedMenu()
    {
        volume.weight = 1;
        text.SetActive(true);
    }
}
