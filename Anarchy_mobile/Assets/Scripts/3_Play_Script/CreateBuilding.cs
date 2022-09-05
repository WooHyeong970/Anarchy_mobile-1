using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnarchyUtility;

public class CreateBuilding : MonoBehaviourPunCallbacks
{
    [SerializeField]
    int[]               levels = new int[] { 0, 0, 0 };
    string              forceName;

    public Image[]      illust = new Image[3];
    public Image[]      levelMaxImages = new Image[3];
    public Text[]       levelTexts = new Text[3];

    public Button[]     buildingButtons = new Button[3];
    
    Utility             UT = new Utility();
    UIManager           UI;
    CentralProcessor    CP;
    VariableManager     VM;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
            buildingButtons[i].onClick.AddListener(() => CreateBuildingFunc(buildingButtons[i], i + 1, levels[i]));
        forceName = GameManager.instance.playerData.getForceName();
    }

    private void CreateBuildingFunc(Button button, int type, int level)
    {
        UT.SetManager(ref UI, ref CP, ref VM);

        int cost = VM.building_resultCost[level];

        if (UT.CheckCost(cost, CP.GetMoney()))
        {
            UI.fadeOutErrorMessage("돈이 부족합니다");
            return;
        }

        if (CP.buildCnt == 0)
        {
            UI.fadeOutErrorMessage("건설 횟수 초과");
            return;
        }

        CP.effectSoundManager.PlayButtonClickSound();
        CP.SetMoney(UT.CalculateCost(CP.GetMoney(), cost));

        levels[type - 1] = ++level;

        GameObject _building = InstantiateBuilding(type, level);

        CP.currentBuildings[type - 1] = _building.GetComponent<MyBuilding>();
        illust[type - 1].sprite = Resources.Load<Sprite>("BuildingIllusts/TYPE_" + type.ToString() + "_" + (level + 1).ToString());
        levelTexts[type - 1].text = "X " + levels[type - 1].ToString();

        VM.BuildingBuffSelect(((type - 1) * 3) + 1);
        VM.BuildingCostSetting();

        if (level == 3)
        {
            levelMaxImages[type - 1].gameObject.SetActive(true);
            button.gameObject.SetActive(false);
            return;
        }

        if (VM.isBuildCostEffect && VM.buildEffects.Count == 0)
        {
            VM.isBuildCostEffect = false;
            VM.BuildingCostEffect(-VM.currentBuff);
            VM.isBuildCostEffect = false;
        }
        else if (VM.isBuildCostEffect && VM.buildEffects.Count > 0)
        {
            VM.isBuildCostEffect = false;
            VM.BuildingCostEffect(-VM.currentBuff);
            VM.isBuildCostEffect = false;
            var n = VM.buildEffects.Dequeue();
            VM.BuildingCostEffect(n);
        }
    }

    GameObject InstantiateBuilding(int type, int level)
    {
        CP.buildCnt = 0;
        CP.SumScore(5, 0);
        return PhotonNetwork.Instantiate(type.ToString() + "-" + level.ToString() + "_" + forceName, CP.player.getBuilingArea(type - 1).position, Quaternion.Euler(0, CP.player.getQuaternioin(), 0)) as GameObject;
    }
}
