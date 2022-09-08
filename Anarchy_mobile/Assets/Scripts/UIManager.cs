using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using AnarchyUtility;

public class UIManager : MonoBehaviourPun
{
    

    // variable
    //================================================================================
    Utility UT = new Utility();
    Decision decision = new Decision();

    public enum State { Ready, Next, Idle, Active, Attack, End };
    public State state = State.Idle;

    public Cloud cloud;

    public GameObject[] ui;

    public Image[] unitHP = new Image[3];
    public Image[] buildingLevels = new Image[3];
    public Image waitingPanel;
    public Image unitInfoPanel;
    public Image unitIllust;
    public Image unitButtonPanel;
    public Image buildingInfoPanel;
    public Image buildingIllust;
    public Image decisionWindow;
    public Image checkWindow;

    public GameObject settingWindow;
    public GameObject exitWindow;
    public GameObject unitWindow;
    public GameObject buildWindow;
    public GameObject moveCameraMapWindow;
    public GameObject movemapWindow;
    public GameObject gameoverWindow;
    public GameObject decisionList;

    public Text curMoney;
    public Text waitingText;
    public Text checkTurn;
    public Text curTurn;
    public Text errorMessage;
    public Text unitName;
    public Text unitActiveCost;
    public Text unitATK;
    public Text unitDEF;
    public Text buildingName;
    public Text buildingEffect;
    public Text BGMOnOffText;
    public Text checkWindowText;
    public Text timer;

    public Button[] mapButtons;

    [Header("UI Buttons")]
    [SerializeField]
    private Button closeWindow;
    [SerializeField]
    private Button settingButton;
    [SerializeField]
    private Button exitButton;
    [SerializeField]
    private Button turnEndButton;
    [SerializeField]
    private Button unitButton;
    [SerializeField]
    private Button buildButton;
    [SerializeField]
    private Button moveMinimapButton;
    [SerializeField]
    private Button decisionButton;
    [SerializeField]
    private Button offAttackButton;
    [SerializeField]
    private Button unitMoveButton;
    [SerializeField]
    private Button unitAttackButton;

    [Header("Decision Select")]
    [SerializeField]
    private Button decisionCase1;
    [SerializeField]
    private Button decisionCase2;
    [SerializeField]
    private Button decisionCase3;



    public Button costIgnoreButton;
    
    public Button moveButton;
    public Button currentMoveButton;
    
    public Button BGMOnOffButton;

    Color errorMessageColor = new Color();
    IEnumerator errorMessageCo;

    float time;
    float start = 1f;
    float end = 0f;
    float fadeTime = 1f;

    bool isIgnoreCheck;
    //================================================================================
    // functions
    //================================================================================
    #region
    private void Start()
    {
        ButtonBinding();
        SetReadyState();
        errorMessageCo = fadeoutErrorMessage();
        errorMessageColor = Color.red;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (state == State.Idle)
                ExitButtonClick();
            else if (state == State.Active)
                SetIdleState();
            else if (state == State.Attack)
                OffReadyAttack();
        }
    }
    private void ButtonBinding()
    {
        //
        // UI Buttons Binding
        //
        closeWindow.onClick.AddListener(() => SetIdleState());
        settingButton.onClick.AddListener(() => SettingButtonClick());
        exitButton.onClick.AddListener(() => ExitButtonClick());
        turnEndButton.onClick.AddListener(() => CentralProcessor.Instance.CheckTurn());
        unitButton.onClick.AddListener(() => UnitButtonClick());
        buildButton.onClick.AddListener(() => BuildButtonClick());
        moveMinimapButton.onClick.AddListener(() => MinimapButtonClick());
        decisionButton.onClick.AddListener(() => DecisionButtonClick());
        offAttackButton.onClick.AddListener(() => OffReadyAttack());
        unitMoveButton.onClick.AddListener(() => MoveButtonClick());
        unitAttackButton.onClick.AddListener(() => ReadyAttack());

        //
        // Decision Select
        //
        //decisionCase1.onClick.AddListener(() => decision.SelectDecisionCase(0));
        //decisionCase2.onClick.AddListener(() => decision.SelectDecisionCase(1));
        //decisionCase3.onClick.AddListener(() => decision.SelectDecisionCase(2));


        costIgnoreButton.onClick.AddListener(() => IgnoreActiveCostCheck());
        
        
        moveButton.onClick.AddListener(() => MoveButtonClick());
        BGMOnOffButton.onClick.AddListener(() => BGMOnOff());
    }

    public void fadeOutErrorMessage(string mesg)
    {
        StopCoroutine(errorMessageCo);
        errorMessageCo = fadeoutErrorMessage();
        PrintErrorMessage(mesg);
        StartCoroutine(errorMessageCo);
    }

    public void ShowUnitInfo(MyUnit unit)
    {
        UT.SetActive(unitInfoPanel, true);
        unitName.text = unit.unitName;
        unitActiveCost.text = unit.cost.ToString();
        unitIllust.sprite = unit.illust;
        unitATK.text = unit.offensive.ToString();
        unitDEF.text = unit.defensive.ToString();

        for (int i = 0; i < unit.hp; i++)
            UT.SetActive(unitHP[i], true);
    }

    public void CloseUnitInfo()
    {
        UT.SetActive(unitInfoPanel, false);
        UT.SetActive(unitButtonPanel, false);
        CentralProcessor.Instance.currentUnit = null;
        ResetUnitHPBar();
    }

    public void ShowBuildingInfo(MyBuilding building)
    {
        UT.SetActive(buildingInfoPanel, true);
        buildingName.text = building.name;
        buildingIllust.sprite = building.illust;

        for (int i = 0; i < building.level; i++)
            UT.SetActive(buildingLevels[i], true);

        buildingEffect.text = building.desc;
    }

    public void CloseBuildingInfo()
    {
        UT.SetActive(buildingInfoPanel, false);
        ResetBuildingLevel();
    }

    private void ResetUnitHPBar()
    {
        foreach (Image hp in unitHP)
            UT.SetActive(hp, false);
    }

    private void ResetBuildingLevel()
    {
        foreach (Image lv in buildingLevels)
            UT.SetActive(lv, false);
    }

    public void OffInOf()
    {
        if (CentralProcessor.Instance.currentUnit != null)
            CentralProcessor.Instance.currentUnit.CloseInfo();
        if (CentralProcessor.Instance.currentBuilding != null)
            CentralProcessor.Instance.currentBuilding.CloseInfo();
    }

    public bool GetIsIgnoreCheck()
    {
        return isIgnoreCheck;
    }

    public void StartCloudAnimation()
    {
        UT.SetActive(waitingPanel, false);
        UT.SetActive(cloud.gameObject, true);
    }

    public void IgnoreActiveCostCheck()
    {
        isIgnoreCheck = !isIgnoreCheck;
        if (!isIgnoreCheck)
            checkTurn.text = "OFF";
        else
            checkTurn.text = "ON";
    }

    private void SettingButtonClick()
    {
        SetActiveState();
        UT.SetActive(closeWindow, true);
        UT.SetActive(settingWindow, true);
    }

    private void ExitButtonClick()
    {
        SetActiveState();
        UT.SetActive(closeWindow, true);
        UT.SetActive(exitWindow, true);
    }

    private void UnitButtonClick()
    {
        SetActiveState();
        UT.SetActive(closeWindow, true);
        UT.SetActive(unitWindow, true);
    }

    private void BuildButtonClick()
    {
        SetActiveState();
        UT.SetActive(closeWindow, true);
        UT.SetActive(buildWindow, true);
    }

    private void MinimapButtonClick()
    {
        SetActiveState();
        UT.SetActive(closeWindow, true);
        UT.SetActive(moveCameraMapWindow, true);
    }

    private void ShowDecisionEffect()
    {
        UT.SetActive(closeWindow, true);
        UT.SetActive(decisionWindow, true);
    }

    private void MoveButtonClick()
    {
        SetActiveState();
        UT.SetActive(closeWindow, true);
        UT.SetActive(movemapWindow, true);
        UT.SetActive(unitInfoPanel, false);
        UT.SetActive(unitButtonPanel, false);
        SearchWay();
    }

    private void SearchWay()
    {
        foreach (Button b in mapButtons)
        {
            b.gameObject.GetComponent<MoveUnit>().isChecked = false;
            b.gameObject.GetComponent<MoveUnit>().checkPoint.gameObject.SetActive(true);
            b.gameObject.GetComponent<MoveUnit>().isMove = true;
            int dis = CalculateDistance(CentralProcessor.Instance.currentTile, b.gameObject.GetComponent<MoveUnit>().pairTile);
            b.gameObject.GetComponent<MoveUnit>().cost = dis;
            if (dis == 0 || dis > CentralProcessor.Instance.currentUnit.activeCost)
            {
                b.gameObject.GetComponent<MoveUnit>().checkPoint.gameObject.SetActive(false);
                b.gameObject.GetComponent<MoveUnit>().isMove = false;
            }
        }
    }

    private int CalculateDistance(Tile current, Tile obj)
    {
        int dis = 0;
        int x = Mathf.Abs(current.row - obj.row);
        int y = Mathf.Abs(current.col - obj.col);
        dis = x + y;
        return dis;
    }

    private void SetReadyState()
    {
        state = State.Ready;
        UISetActiveFalse();
    }

    public void SetIdleState()
    {
        CentralProcessor.Instance.effectSoundManager.PlayButtonClickSound();
        state = State.Idle;
        WindowSetActiveFalse();
        UISetActiveTrue();
    }

    private void SetActiveState()
    {
        CentralProcessor.Instance.effectSoundManager.PlayButtonClickSound();
        state = State.Active;
        UISetActiveFalse();
    }

    private void SetAttackState()
    {
        state = State.Attack;
        UISetActiveFalse();
    }

    public void SetEndState()
    {
        state = State.End;
        UISetActiveFalse();
        WindowSetActiveFalse();
        gameoverWindow.gameObject.SetActive(true);
    }

    public void SetNextState()
    {
        state = State.Next;
        UISetActiveFalse();
        WindowSetActiveFalse();
    }

    private void WindowSetActiveFalse()
    {
        GameObject[] window = GameObject.FindGameObjectsWithTag("window");
        foreach (GameObject w in window)
            w.gameObject.SetActive(false);
    }

    private void UISetActiveFalse()
    {
        foreach (GameObject u in ui)
            u.gameObject.SetActive(false);
    }

    private void UISetActiveTrue()
    {
        foreach (GameObject u in ui)
            u.gameObject.SetActive(true);
    }

    private void PrintErrorMessage(string s)
    {
        errorMessage.gameObject.SetActive(true);
        errorMessage.text = s;
    }

    private void OffMapButtonsCheck()
    {
        SetIdleState();
        foreach (Button b in mapButtons)
            b.GetComponent<MoveUnit>().OffCheck();
        currentMoveButton = null;
    }

    private void ReadyAttack()
    {
        if (CentralProcessor.Instance.currentUnit.activeCost == 0)
            return;
        SetAttackState();
        UT.SetActive(unitInfoPanel, false);
        UT.SetActive(unitButtonPanel, false);
        UT.SetActive(offAttackButton, true);
    }

    public void OffReadyAttack()
    {
        SetIdleState();
        offAttackButton.gameObject.SetActive(false);
    }

    private void DecisionButtonClick()
    {
        CentralProcessor.Instance.effectSoundManager.PlayButtonClickSound();
        if (decisionList.gameObject.activeSelf)
            UT.SetActive(decisionList, false);
        else
            UT.SetActive(decisionList, true);
    }

    private void BGMOnOff()
    {
        if (GameManager.instance.audioManager.GetComponent<AudioSource>().volume == 1)
        {
            BGMOnOffText.text = "OFF";
            GameManager.instance.audioManager.GetComponent<AudioSource>().volume = 0;
        }
        else
        {
            BGMOnOffText.text = "ON";
            GameManager.instance.audioManager.GetComponent<AudioSource>().volume = 1;
        }
    }

    public void ShowCheckWindow(string s)
    {
        if (checkWindow.gameObject.activeSelf)
        {
            CentralProcessor.Instance.effectSoundManager.PlayButtonClickSound();
            SetIdleState();
            checkWindow.gameObject.SetActive(false);
        }
        else
        {
            SetActiveState();
            checkWindow.gameObject.SetActive(true);
            checkWindowText.text = s;
        }
    }

    public void ExitGame()
    {
        GameManager.instance.audioManager.ButtonClickSound();
        Application.Quit();
    }
    #endregion
    //================================================================================
    // coroutine
    //================================================================================

    //================================================================================
    IEnumerator fadeoutErrorMessage()
    {
        errorMessage.gameObject.SetActive(true);
        Color fadecolor = errorMessageColor;
        time = 0f;
        fadecolor.a = Mathf.Lerp(start, end, time);
        while (fadecolor.a > 0f)
        {
            time += Time.deltaTime / fadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time);
            errorMessage.color = fadecolor;
            yield return null;
        }
        errorMessage.gameObject.SetActive(false);
        StopCoroutine(fadeoutErrorMessage());
    }
    //================================================================================

    public void MoveUnit()
    {
        for(int i = 0; i < 3; i++)
        {
            if(!CentralProcessor.Instance.targetTile.GetCheckPos(i, CentralProcessor.Instance.GetPlayer().GetLayer()))
            {
                CentralProcessor.Instance.Move(CentralProcessor.Instance.currentUnit, i);
                return;
            }
        }
        








        //if(CentralProcessor.Instance.current_moveButton == null)
        //{
        //    return;
        //}

        //Transform[] area;
        //bool[] isEmpty;

        //if(CentralProcessor.Instance.isMaster)
        //{
        //    area = CentralProcessor.Instance.current_moveButton.GetComponent<MoveUnit>().pairTile.P1_unitArea;
        //    isEmpty = CentralProcessor.Instance.current_moveButton.GetComponent<MoveUnit>().pairTile.isP1_unitArea;
        //}
        //else
        //{
        //    area = CentralProcessor.Instance.current_moveButton.GetComponent<MoveUnit>().pairTile.P2_unitArea;
        //    isEmpty = CentralProcessor.Instance.current_moveButton.GetComponent<MoveUnit>().pairTile.isP2_unitArea;
        //}

        //if(isEmpty[0] && isEmpty[1] && isEmpty[2])
        //{
        //    return;
        //}
        //if(CentralProcessor.Instance.isMaster)
        //{
        //    CentralProcessor.Instance.CheckUnitArea(7, CentralProcessor.Instance.currentTile.gameObject.GetComponent<PhotonView>().ViewID,false,CentralProcessor.Instance.currentUnit.myNum);
        //}
        //else
        //{
        //    CentralProcessor.Instance.CheckUnitArea(8, CentralProcessor.Instance.currentTile.gameObject.GetComponent<PhotonView>().ViewID,false,CentralProcessor.Instance.currentUnit.myNum);
        //}
        //CentralProcessor.Instance.CheckTileUnits(CentralProcessor.Instance.currentTile.gameObject.GetComponent<PhotonView>().ViewID, CentralProcessor.Instance.currentUnit.gameObject.GetComponent<PhotonView>().ViewID, CentralProcessor.Instance.currentUnit.myNum, CentralProcessor.Instance.isMaster, false);

        //for(int i = 0; i < 3; i++)
        //{
        //    if(isEmpty[i] == false)
        //    {
        //        if(CentralProcessor.Instance.isMaster)
        //        {
        //            CentralProcessor.Instance.CheckUnitArea(7, CentralProcessor.Instance.current_moveButton.GetComponent<MoveUnit>().pairTile.GetComponent<PhotonView>().ViewID,true,i);
        //        }
        //        else
        //        {
        //            CentralProcessor.Instance.CheckUnitArea(8, CentralProcessor.Instance.current_moveButton.GetComponent<MoveUnit>().pairTile.GetComponent<PhotonView>().ViewID,true,i);
        //        }
        //        //CentralProcessor.Instance.CheckUnitArea(CentralProcessor.Instance.current_moveButton.GetComponent<MoveUnit>().pairTile.GetComponent<PhotonView>().ViewID,true,i,CentralProcessor.Instance.isMaster);
        //        CentralProcessor.Instance.CheckTileUnits(CentralProcessor.Instance.current_moveButton.GetComponent<MoveUnit>().pairTile.GetComponent<PhotonView>().ViewID, CentralProcessor.Instance.currentUnit.GetComponent<PhotonView>().ViewID, i, CentralProcessor.Instance.isMaster, true);
        //        CentralProcessor.Instance.currentUnit.transform.position = area[i].position;
        //        //CentralProcessor.Instance.currentUnit.currentTile = CentralProcessor.Instance.current_moveButton.GetComponent<MoveUnit>().pairTile;
        //        CentralProcessor.Instance.ApplyUnitCurrentTile(CentralProcessor.Instance.currentUnit.GetComponent<PhotonView>().ViewID, CentralProcessor.Instance.current_moveButton.GetComponent<MoveUnit>().pairTile.GetComponent<PhotonView>().ViewID);
        //        //CentralProcessor.Instance.currentUnit.activeCost -= CentralProcessor.Instance.current_moveButton.GetComponent<MoveUnit>().cost;
        //        CentralProcessor.Instance.ApplyUnitActiveCost(CentralProcessor.Instance.currentUnit.GetComponent<PhotonView>().ViewID, -CentralProcessor.Instance.current_moveButton.GetComponent<MoveUnit>().cost);
        //        //CentralProcessor.Instance.CurrentUnitNull();
        //        CentralProcessor.Instance.current_moveButton.GetComponent<MoveUnit>().ChangeColor(Color.black);
        //        CentralProcessor.Instance.current_moveButton = null;
        //        CentralProcessor.Instance.uIManager.SetIdleState();
        //        CentralProcessor.Instance.uIManager.UISetActiveTrue();
        //        CentralProcessor.Instance.effectSoundManager.PlayMoveSound();
        //        return;
        //    }
        //}
    }
}
