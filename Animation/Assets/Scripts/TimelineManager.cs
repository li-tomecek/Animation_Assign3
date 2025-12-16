using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayableDirector))]
public class TimelineManager : MonoBehaviour
{
    PlayableDirector playableDirector;
    
    void OnEnable()
    {
        playableDirector = GetComponent<PlayableDirector>();
        playableDirector.stopped += OnTimelineFinished;
    }

    void OnDisable()
    {
        playableDirector.stopped -= OnTimelineFinished;
    }

    void OnTimelineFinished(PlayableDirector director)
    {
        SceneManager.LoadScene("Playable_Scene");
    }
}
