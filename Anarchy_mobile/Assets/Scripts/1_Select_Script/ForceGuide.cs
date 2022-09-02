using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ForceGuide : MonoBehaviour
{
    public enum forceNum2 { SOCIETY, NEW_WAVE, MAFIA };
    public Image    ChooseforcePanel;
    public Image    ChoosemapPanel;
    public Image    force_image;
    public Text     description;

    int             enterWidth = 410;
    int             enterHeight = 630;
    int             exitWidth = 380;
    int             exitHeight = 600;

    [TextArea]
    public string   sDisc;
    [TextArea]
    public string   nDisc;
    [TextArea]
    public string   mDisc;

    public bool     selected = false;
    public Button   toTitleBtn;
    public Button   nextButton;
    public Button   backButton;
    public Button   mapSelectButton;
    public Button[] forces = new Button[3];
    Button          curButton;

    public delegate void DelSelectForce(Button btn, int num, string disc, string name);

    private void Start()
    {
        DelSelectForce sfHandler = new DelSelectForce(SelectForce);
        forces[0].onClick.AddListener(() => sfHandler(forces[0], (int)forceNum2.SOCIETY , sDisc, "SOCIETY" ));
        forces[1].onClick.AddListener(() => sfHandler(forces[1], (int)forceNum2.NEW_WAVE, nDisc, "NEW_WAVE" ));
        forces[2].onClick.AddListener(() => sfHandler(forces[2], (int)forceNum2.MAFIA, mDisc, "MAFIA" ));

        toTitleBtn.onClick.AddListener(() => GameManager.instance.LoadScene("0_Title"));
        nextButton.onClick.AddListener(() => SelectPanel(ChooseforcePanel, ChoosemapPanel));
        backButton.onClick.AddListener(() => SelectPanel(ChoosemapPanel, ChooseforcePanel));
        mapSelectButton.onClick.AddListener(() => GameManager.instance.LoadScene("2_Lobby"));
    }

    private void SelectForce(Button btn, int num, string disc, string name)
    {
        if(!selected)
        {
            curButton = btn;
            selected = true;
            nextButton.GetComponent<Button>().interactable = true;
            btn.GetComponent<RectTransform>().sizeDelta = new Vector2(enterWidth, enterHeight);
            force_image.gameObject.SetActive(true);
            description.text = disc;
            //GameManager.instance.playerData.forceNumber = num;
            //GameManager.instance.playerData.setForceNumber(num);
            //GameManager.instance.playerData.setForceName(name);
            GameManager.instance.playerData.setForceInfo(num, name);
            GameManager.instance.SaveDataToJson();
        }
        else
        {
            if(curButton == btn)
                CancelForce(btn);
            else
            {
                CancelForce(curButton);
                SelectForce(btn, num, disc, name);
            }
        }
    }

    private void CancelForce(Button btn)
    {
        selected = false;
        btn.GetComponent<RectTransform>().sizeDelta = new Vector2(exitWidth, exitHeight);
        force_image.gameObject.SetActive(false);
    }

    private void SelectPanel(Image curPanel, Image nextPanel)
    {
        GameManager.instance.audioManager.ButtonClickSound();
        curPanel.gameObject.SetActive(false);
        nextPanel.gameObject.SetActive(true);
    }
}
