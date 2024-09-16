using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneController : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public string MissionName;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndReached;
    }

    void EndReached(VideoPlayer vp)
    {
        // Load the next scene when the video ends
        SceneManager.LoadScene(MissionName);
    }
}
