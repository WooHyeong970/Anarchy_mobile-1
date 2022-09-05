using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleChar : MonoBehaviour
{
    public Image[]  images = new Image[3];
    int             idx = 0;
    Color           imageColor;

    float           start = 0f;
    float           end = 1f;
    float           time;
    float           fadeTime = 1f;

    [SerializeField]
    bool            fade = false;

    private void Start()
    {
        StartCoroutine("FadeIn");
    }

    IEnumerator FadeIn()
    {
        images[idx].gameObject.SetActive(true);
        imageColor = images[idx].color;
        time = 0f;
        imageColor.a = Mathf.Lerp(start, end, time);
        while (true)
        {
            yield return null;
            if (fade)
            {
                StartCoroutine("FadeOut");
                StopCoroutine("FadeIn");
            }
            if (imageColor.a < 1f)
            {
                time += Time.deltaTime / fadeTime;
                imageColor.a = Mathf.Lerp(start, end, time);
                images[idx].color = imageColor;
            }
            if (imageColor.a == 1f)
            {
                fade = true;
                yield return new WaitForSeconds(1.5f);
            }
        }
    }

    IEnumerator FadeOut()
    {
        time = 0f;
        imageColor.a = Mathf.Lerp(end, start, time);
        while (true)
        {
            yield return null;
            if (!fade)
            {
                images[idx].gameObject.SetActive(false);
                idx++;
                if (idx > 2)
                    idx = 0;
                StartCoroutine("FadeIn");
                StopCoroutine("FadeOut");
            }
            if (imageColor.a > 0f)
            {
                time += Time.deltaTime / fadeTime;
                imageColor.a = Mathf.Lerp(end, start, time);
                images[idx].color = imageColor;
            }
            if (imageColor.a == 0f)
            {
                fade = false;
                images[idx].color = imageColor;
                yield return new WaitForSeconds(0.4f);
            }
        }
    }
}
