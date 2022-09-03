using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleFontShadow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text text;

    public void OnPointerEnter(PointerEventData eventData)
    {
         text.GetComponent<Shadow>().enabled = true;
         text.GetComponent<Outline>().enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.GetComponent<Shadow>().enabled = false;
        text.GetComponent<Outline>().enabled = false;
    }

    public void TutorialButton()
    {
        GameManager.instance.LoadScene("4_Tutorial");
    }

    public void GameStartButton()
    {
        GameManager.instance.LoadScene("1_Select");
    }

    public void ExitButton()
    {

    }
}
