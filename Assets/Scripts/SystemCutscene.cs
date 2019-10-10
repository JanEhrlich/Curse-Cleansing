using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

/*
 * Handels the Playback of Cutscenes.
 */
public class SystemCutscene : MonoBehaviour
{
    private Camera mainCam;
    private Camera cutsceneCam;

    public RawImage rawImage;
    public TextMeshProUGUI textBox;

    public List<VideoPlayer> cutScenes;
    public List<string> cutSceneTexts;

    private int playingIndex;

    public void Start()
    {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        cutsceneCam = GetComponent<Camera>();

        foreach (VideoPlayer player in cutScenes)
        {
            player.Prepare();
        }
    }

    private void SwitchToMainCamera()
    {
        cutsceneCam.enabled = false;
        mainCam.enabled = true;
        Time.timeScale = 1f;
    }

    private void SwitchToCutSceneCamera()
    {
        mainCam.enabled = false;
        cutsceneCam.enabled = true;
        Time.timeScale = 0f;
    }

    //Index Starts at 1 ==> So cutscene 1 needs index 1 as Input
    public void PlayCutscene(int index)
    {
        SwitchToCutSceneCamera();

        if (index <= cutScenes.Count && index > 0)
        {
            playingIndex = index - 1;
            textBox.text = cutSceneTexts[playingIndex].Replace("/n", System.Environment.NewLine);
            rawImage.texture = cutScenes[playingIndex].texture;
            cutScenes[playingIndex].Play();
            cutScenes[playingIndex].loopPointReached += EndReached;
        }
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.Stop();
        SwitchToMainCamera();
    }
}
