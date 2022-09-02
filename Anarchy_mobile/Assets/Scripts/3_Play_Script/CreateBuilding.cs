using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
//using System.IO;

public class CreateBuilding : MonoBehaviourPunCallbacks
{
    //public int          type;
    //public int          level = 0;

    //int[] type = new int[3];
    int[] levels = new int[] { 0, 0, 0 };
    public Image[] illust = new Image[3];
    public Image[] level_max = new Image[3];
    public Text[] level_text = new Text[3];

    //public bool         isMaster;

    //public GameObject   building;
    //public GameObject test;
    //public GameObject[] buildings = new GameObject[9];

    //public Transform[]  P1_buildingArea = new Transform[3];
    //public Transform[]  P2_buildingArea = new Transform[3];

    //public Image        maxImg;
    //public Image        illust;

    //public Text         levelText;

    //string              s;
    //int                 forceNum;
    string forceName;
    int buildingLevel = 1;
    int buildingNumber = 1;

    public Button[] buildingButtons = new Button[3];

    UIManager uiManager;

    //bool isCreateBuilding = true;

    private void Start()
    {
        //Image image = Resources.Load<Image>("BuildingIllust/TYPE_1_2");
        //illust[0].sprite = image.sprite;
        //illust[0].sprite = Resources.Load<Image>("BuildingIllust/TYPE_1_2.png").sprite;
        buildingButtons[0].onClick.AddListener(() => CreateBuildingFunc(buildingButtons[0], 1, levels[0]));
        buildingButtons[1].onClick.AddListener(() => CreateBuildingFunc(buildingButtons[1], 2, levels[1]));
        buildingButtons[2].onClick.AddListener(() => CreateBuildingFunc(buildingButtons[2], 3, levels[2]));
        forceName = GameManager.instance.playerData.getForceName();
        //isMaster = CentralProcessor.Instance.isMaster;
        //illust.sprite = buildings[0].GetComponent<MyBuilding>().illust;
        //test = Resources.Load<GameObject>("Assets/Photon/PhotonUnityNetworking/Resources/1-1_M.prefab");
        //switch(GameManager.instance.playerData.getForceNumber())
        //{
        //    case 1:
        //        building = buildings[0];
        //        break;
        //    case 2:
        //        building = buildings[1];
        //        break;
        //    case 3:
        //        building = buildings[2];
        //        break;
        //}
        //go = AssetDatabase.LoadAssetAtPath("Assets/Photon/PhotonUnityNetworking/Resources/1-1_M.prefab", typeof(GameObject)) as GameObject;
//#if UNITY_EDITOR
//        building = setBuilding(buildingNumber, buildingLevel);
//#endif
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
#if UNITY_EDITOR
        GameObject _building = InstantiateBuilding(type, level);
#endif
        
        illust[type - 1].sprite = Resources.Load<Sprite>("BuildingIllust/TYPE_" + type.ToString() + "_" + (level + 1).ToString());
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

    //public void CreateButtonClick()
    //{
    //    if (CheckCost())
    //    {
    //        //CentralProcessor.Instance.uIManager.StopCoroutine("fadeoutErrorMessage");
    //        //s = "돈이 부족합니다";
    //        //CentralProcessor.Instance.uIManager.PrintErrorMessage(s);
    //        //CentralProcessor.Instance.uIManager.StartCoroutine("fadeoutErrorMessage");

    //        CentralProcessor.Instance.uIManager.fadeOutErrorMessage("돈이 부족합니다");
    //        return;
    //    }

    //    if (CentralProcessor.Instance.createBuildingNumber == 0)
    //    {
    //        //CentralProcessor.Instance.uIManager.StopCoroutine("fadeoutErrorMessage");
    //        //s = "건설 횟수 초과";
    //        //CentralProcessor.Instance.uIManager.PrintErrorMessage(s);
    //        //CentralProcessor.Instance.uIManager.StartCoroutine("fadeoutErrorMessage");
    //        CentralProcessor.Instance.uIManager.fadeOutErrorMessage("건설 횟수 초과");
    //        return;
    //    }

    //    if (level == 0)
    //    {
    //        GameObject _building = Instantiate(building, type, buildingLevel);

    //        //if(isMaster)
    //        //{
    //        //    GameObject b;
    //        //    b = PhotonNetwork.Instantiate(building.name, P1_buildingArea[type].position, Quaternion.Euler(0,180,0)) as GameObject;
    //        //    CalculateCost(VariableManager.Instance.building_resultCost[0]);
    //        //    CentralProcessor.Instance.currentBuildings[type] = b.GetComponent<MyBuilding>();
    //        //    CentralProcessor.Instance.createBuildingNumber -= 1;
    //        //    CentralProcessor.Instance.SumScore(5,0);
    //        //}
    //        //else
    //        //{
    //        //    GameObject b;
    //        //    b = PhotonNetwork.Instantiate(building.name, P2_buildingArea[type].position, Quaternion.Euler(0,0,0)) as GameObject;
    //        //    CalculateCost(VariableManager.Instance.building_resultCost[0]);
    //        //    CentralProcessor.Instance.currentBuildings[type] = b.GetComponent<MyBuilding>();
    //        //    CentralProcessor.Instance.createBuildingNumber -= 1;
    //        //    CentralProcessor.Instance.SumScore(0,5);
    //        //}
    //        CentralProcessor.Instance.effectSoundManager.PlayButtonClickSound();
    //        illust.sprite = buildings[3].GetComponent<MyBuilding>().illust;
    //        levelText.text = "X 2";
    //        level++;
    //        VariableManager.Instance.BuildingBuffSelect((type * 3) + 1);
    //        VariableManager.Instance.BuildingCostSetting();
    //    }
    //    else
    //    {
    //        //building = buildings[level];
    //        switch (GameManager.instance.playerData.forceNumber)
    //        {
    //            case 1:
    //                if (level == 1)
    //                {
    //                    building = buildings[3];
    //                }
    //                else
    //                {
    //                    building = buildings[6];
    //                }
    //                break;
    //            case 2:
    //                if (level == 1)
    //                {
    //                    building = buildings[4];
    //                }
    //                else
    //                {
    //                    building = buildings[7];
    //                }
    //                break;
    //            case 3:
    //                if (level == 1)
    //                {
    //                    building = buildings[5];
    //                }
    //                else
    //                {
    //                    building = buildings[8];
    //                }
    //                break;
    //        }
    //        CentralProcessor.Instance.BuildingUpgrade(CentralProcessor.Instance.currentBuildings[type].GetComponent<PhotonView>().ViewID);
    //        if (isMaster)
    //        {
    //            GameObject b;
    //            b = PhotonNetwork.Instantiate(building.name, P1_buildingArea[type].position, Quaternion.Euler(0, 180, 0)) as GameObject;
    //            CalculateCost(VariableManager.Instance.building_resultCost[level]);
    //            CentralProcessor.Instance.currentBuildings[type] = b.GetComponent<MyBuilding>();
    //            CentralProcessor.Instance.createBuildingNumber -= 1;
    //            CentralProcessor.Instance.SumScore(5, 0);
    //        }
    //        else
    //        {
    //            GameObject b;
    //            b = PhotonNetwork.Instantiate(building.name, P2_buildingArea[type].position, Quaternion.Euler(0, 0, 0)) as GameObject;
    //            CalculateCost(VariableManager.Instance.building_resultCost[level]);
    //            CentralProcessor.Instance.currentBuildings[type] = b.GetComponent<MyBuilding>();
    //            CentralProcessor.Instance.createBuildingNumber -= 1;
    //            CentralProcessor.Instance.SumScore(0, 5);
    //        }
    //        CentralProcessor.Instance.effectSoundManager.PlayButtonClickSound();
    //        illust.sprite = buildings[6].GetComponent<MyBuilding>().illust;
    //        levelText.text = "X 3";
    //        level++;
    //        VariableManager.Instance.BuildingBuffSelect((type * 3) + level);
    //        VariableManager.Instance.BuildingCostSetting();
    //        if (level == 3)
    //        {
    //            maxImg.gameObject.SetActive(true);
    //            this.gameObject.SetActive(false);
    //            return;
    //        }
    //    }

    //        if (VariableManager.Instance.isBuildCostEffect && VariableManager.Instance.buildEffects.Count == 0)
    //    {
    //        VariableManager.Instance.isBuildCostEffect = false;
    //        VariableManager.Instance.BuildingCostEffect(-VariableManager.Instance.currentBuff);
    //        VariableManager.Instance.isBuildCostEffect = false;
    //    }
    //    else if (VariableManager.Instance.isBuildCostEffect && VariableManager.Instance.buildEffects.Count > 0)
    //    {
    //        VariableManager.Instance.isBuildCostEffect = false;
    //        VariableManager.Instance.BuildingCostEffect(-VariableManager.Instance.currentBuff);
    //        VariableManager.Instance.isBuildCostEffect = false;
    //        var n = VariableManager.Instance.buildEffects.Dequeue();
    //        VariableManager.Instance.BuildingCostEffect(n);
    //    }
    //}

    public bool CheckCost(int level)
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

    public void CalculateCost(int cost)
    {
        CentralProcessor.Instance.currentMoney.text = (int.Parse(CentralProcessor.Instance.currentMoney.text) - cost).ToString();
    }

#if UNITY_EDITOR
    GameObject setBuilding(int type, int level)
    {
        string path = "Assets/Photon/PhotonUnityNetworking/Resources/" + type.ToString() + "-" + level.ToString() + "_" + forceName + ".prefab";
        return AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
    }
#endif

#if UNITY_EDITOR
    GameObject InstantiateBuilding(int type, int level)
    {
        GameObject _building = setBuilding(type, level);
        CentralProcessor.Instance.currentBuildings[type] = _building.GetComponent<MyBuilding>();
        CentralProcessor.Instance.createBuildingNumber = 0;
        CentralProcessor.Instance.SumScore(5, 0);
        return PhotonNetwork.Instantiate(_building.name, CentralProcessor.Instance.player.getBuilingArea(type - 1).position, Quaternion.Euler(0, CentralProcessor.Instance.player.getQuaternioin(), 0)) as GameObject;
    }
#endif
}
