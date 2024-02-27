using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;
    private float currentAnimationTime;
    private VideoPlayer videoPlayer;
    [HideInInspector] public bool IsCutsceneCompleted;
    void Awake()
    {
        IsCutsceneCompleted = true;
        if(Instance == null) Instance = this;
        else Destroy(this);

        videoPlayer = GetComponent<VideoPlayer>();
    }
    void Update()
    {
        if(IsCutsceneCompleted == false)
        {
            Cutscene();
        }
    }
    public void SetCutscene(VideoClip clip)
    {
        videoPlayer.clip = clip;
        IsCutsceneCompleted = false;
        videoPlayer.Play();
    }
    private void Cutscene()
    {
        if(currentAnimationTime <= 0)
        {
            currentAnimationTime = (float) videoPlayer.length;
        }
        else
        {
            currentAnimationTime -= Time.deltaTime;
            currentAnimationTime = Mathf.Clamp(currentAnimationTime, 0, (float) videoPlayer.length);
        }

        if(currentAnimationTime <= 0)
        {
            IsCutsceneCompleted = true;
            videoPlayer.Stop();
        }
    }
}