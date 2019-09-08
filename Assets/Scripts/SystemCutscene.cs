using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

/*
 * Handels the Playback of Cutscenes. Will be activated and deactivated by the SystemGameMaster
 * TODO:
 *  -Get Camera GameObject and Display the Video+(optional)Sound of the Cutscene
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

    public void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            SwitchToCutSceneCamera();
            PlayCutscene(1);
        }

        if (Input.GetKeyDown("s"))
        {
            SwitchToMainCamera();
        }

    }

    private void SwitchToMainCamera()
    {
        cutsceneCam.enabled = false;
        mainCam.enabled = true;
    }

    private void SwitchToCutSceneCamera()
    {
        mainCam.enabled = false;
        cutsceneCam.enabled = true;
    }

    private void PlayCutscene(int index)
    {
        if (index <= cutScenes.Count && index > 0)
        {
            playingIndex = index - 1;
            textBox.text = cutSceneTexts[playingIndex];
            rawImage.texture = cutScenes[playingIndex].texture;
            cutScenes[playingIndex].Play();
            cutScenes[playingIndex].loopPointReached += EndReached;
        }
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.Stop();
        Test(playingIndex);
        SwitchToMainCamera();
    }

    private void Test(int num)
    {
        Debug.Log("Set Progression Bool To True after Playing CutScene " + playingIndex);
    }
}
