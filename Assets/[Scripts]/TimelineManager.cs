using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public PlayableDirector firstDirector;
    public PlayableDirector secondDirector;

    public void StartFirst()
    {
        firstDirector.Play();
    }

    public void StartSecond()
    {
        secondDirector.Play();
    }
}