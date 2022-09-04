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
    public Image[]      level_max = new Image[3];
    public Text[]       level_text = new Text[3];

    public Button[]     buildingButtons = new Button[3];

    UIManager UI = CentralProcessor.Instance.uIManager;
    Utility UT = new Utility();
    CentralProcessor CP;
    VariableManager VM;

    private void Start()
    {
        buildingButtons[0].onClick.AddListener(() => CreateBuildingFunc(buildingButtons[0], 1, levels[0]));
        buildingButtons[1].onClick.AddListener(() => CreateBuildingFunc(buildingButtons[1], 2, levels[1]));
        buildingButtons[2].onClick.AddListener(() => CreateBuildingFunc(buildingButtons[2], 3, levels[2]));
        forceName = GameManager.instance.playerData.getForceName();
    }

    private void CreateBuildingFunc(Button button, int type, int level)
    {
        VM = new VariableManager();
        CP = CentralProcessor.Instance;
        int cost = VM.building_resultCost[level];

        if (UT.CheckCost(cost, CP.getMoney()))
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
        CP.currentMoney.text = UT.CalculateCost(CP.getMoney(), cost);

        levels[type - 1] = ++level;

        GameObject _building = InstantiateBuilding(type, level);

        CP.currentBuildings[type - 1] = _building.GetComponent<MyBuilding>();

        illust[type - 1].sprite = Resources.Load<Sprite>("BuildingIllusts/TYPE_" + type.ToString() + "_" + (level + 1).ToString());
        level_text[type - 1].text = "X " + levels[type - 1].ToString();

        VariableManager.Instance.BuildingBuffSelect(((type - 1) * 3) + 1);
        VariableManager.Instance.BuildingCostSetting();

        if (level == 3)
        {
            level_max[type - 1].gameObject.SetActive(true);
            button.gameObject.SetActive(false);
            return;
        }

        if (VariableManager.Instance.isBuildCostEffect && VariableManager.Instance.buildEffects.Count == 0)
        {
            VariableManager.Instance.isBuildCostEffect = false;
            VariableManager.Instance.BuildingCostEffect(-VariableManager.Instance.currentBuff);
            VariableManager.Instance.isBuildCostEffect = false;
        }
        else if (VariableManager.Instance.isBuildCostEffect && VariableManager.Instance.buildEffects.Count > 0)
        {
            VariableManager.Instance.isBuildCostEffect = false;
            VariableManager.Instance.BuildingCostEffect(-VariableManager.Instance.currentBuff);
            VariableManager.Instance.isBuildCostEffect = false;
            var n = VariableManager.Instance.buildEffects.Dequeue();
            VariableManager.Instance.BuildingCostEffect(n);
        }
    }

    private void CalculateCost(int cost)
    {
        CentralProcessor.Instance.currentMoney.text = (int.Parse(CentralProcessor.Instance.currentMoney.text) - cost).ToString();
    }

    GameObject InstantiateBuilding(int type, int level)
    {
        CentralProcessor.Instance.buildCnt = 0;
        CentralProcessor.Instance.SumScore(5, 0);
        return PhotonNetwork.Instantiate(type.ToString() + "-" + level.ToString() + "_" + forceName, CentralProcessor.Instance.player.getBuilingArea(type - 1).position, Quaternion.Euler(0, CentralProcessor.Instance.player.getQuaternioin(), 0)) as GameObject;
    }
}
