using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FadeOutOnClick : MonoBehaviour
{
    public int fademode = 3; // 0 = fade in, 1 = nothing, 2 = fade out, 3 = stay off
    private VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fademode == 0){
            videoPlayer.targetCameraAlpha += Time.deltaTime;
            if (videoPlayer.targetCameraAlpha > 1){
                videoPlayer.targetCameraAlpha = 1;
                fademode = 1;
            }
        } else if (fademode == 1){
            videoPlayer.targetCameraAlpha = 1;
        } else if (fademode == 2){
            videoPlayer.targetCameraAlpha -= Time.deltaTime;
            if (videoPlayer.targetCameraAlpha < 0){
                videoPlayer.targetCameraAlpha = 0;
                fademode = 3;
            }
        } else if (fademode == 3){
            videoPlayer.targetCameraAlpha = 0;
        }
    }
    public void SetFadeMode(int mode){
        Debug.Log("SetFadeMode: " + mode);
        if (fademode == 0){ return; }
        fademode = mode;
        
    }
}
