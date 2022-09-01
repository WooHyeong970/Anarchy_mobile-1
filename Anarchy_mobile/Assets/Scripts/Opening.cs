using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Opening : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Image back;
    float time = 0;

    void Update()
    {
        if(videoPlayer.isPlaying)
        {
            back.gameObject.SetActive(false);
        }
        if(time > 44f)
        {
            Title();
        }
        time += Time.deltaTime;
    }

    public void Title()
    {
        SceneManager.LoadScene(1);
    }
}
