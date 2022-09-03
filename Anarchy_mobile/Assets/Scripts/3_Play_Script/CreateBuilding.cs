using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateBuilding : MonoBehaviourPunCallbacks
{
    [SerializeField]
    int[]               levels = new int[] { 0, 0, 0 };
    string              forceName;

    public Image[]      illust = new Image[3];
    public Image[]      level_max = new Image[3];
    public Text[]       level_text = new Text[3];

    public Button[]     buildingButtons = new Button[3];

    private void Start()
    {
        buildingButtons[0].onClick.AddListener(() => CreateBuildingFunc(buildingButtons[0], 1, levels[0]));
        buildingButtons[1].onClick.AddListener(() => CreateBuildingFunc(buildingButtons[1], 2, levels[1]));
        buildingButtons[2].onClick.AddListener(() => CreateBuildingFunc(buildingButtons[2], 3, levels[2]));
        forceName = GameManager.instance.playerData.getForceName();
    }

    private void CreateBuildingFunc(Button button, int type, int level)
    {
        if(CheckCost(level))
        {
            CentralProcessor.Instance.uIManager.fadeOutErrorMessage("돈이 부족합니다");
            return;
        }

        if (CentralProcessor.Instance.createBuildingNumber == 0)
        {
            CentralProcessor.Instance.uIManager.fadeOutErrorMessage("건설 횟수 초과");
            return;
        }

        CentralProcessor.Instance.effectSoundManager.PlayButtonClickSound();

        CalculateCost(VariableManager.Instance.building_resultCost[level]);

        levels[type - 1] = ++level;

        GameObject _building = InstantiateBuilding(type, level);

        CentralProcessor.Instance.currentBuildings[type - 1] = _building.GetComponent<MyBuilding>();

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

    private bool CheckCost(int level)
    {
        bool b = true;
        if (VariableManager.Instance.building_resultCost[level] > int.Parse(CentralProcessor.Instance.currentMoney.text))
        {
            return b;
        }
        else
        {
            return !b;
        }
    }

    private void CalculateCost(int cost)
    {
        CentralProcessor.Instance.currentMoney.text = (int.Parse(CentralProcessor.Instance.currentMoney.text) - cost).ToString();
    }

    GameObject InstantiateBuilding(int type, int level)
    {
        CentralProcessor.Instance.createBuildingNumber = 0;
        CentralProcessor.Instance.SumScore(5, 0);
        return PhotonNetwork.Instantiate(type.ToString() + "-" + level.ToString() + "_" + forceName, CentralProcessor.Instance.player.getBuilingArea(type - 1).position, Quaternion.Euler(0, CentralProcessor.Instance.player.getQuaternioin(), 0)) as GameObject;
    }
}
