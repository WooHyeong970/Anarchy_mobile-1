using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Status;

public class CreateUnit : MonoBehaviourPunCallbacks
{
    public Button[]     unitButtons = new Button[3];
    public Text[]       costTexts = new Text[3];
    public Image[]      illusts = new Image[3];

    [SerializeField]
    int[]               unitCosts = new int[3];
    string              forceName;

    StatusManager       statusManager = new StatusManager();

    private void Start()
    {
        forceName = GameManager.instance.playerData.getForceName();
        unitButtons[0].onClick.AddListener(() => CreateUnitFunc(1));
        unitButtons[1].onClick.AddListener(() => CreateUnitFunc(2));
        unitButtons[2].onClick.AddListener(() => CreateUnitFunc(3));
        setUnitIllust();
    }

    private void CreateUnitFunc(int type)
    {
        if(CentralProcessor.Instance.createUnitNumber <= 0)
        {
            CentralProcessor.Instance.uIManager.fadeOutErrorMessage("소환 횟수 초과");
            return;
        }

        if(CheckCost(type - 1))
        {
            CentralProcessor.Instance.uIManager.fadeOutErrorMessage("돈이 부족합니다");
            return;
        }

        for(int i = 0; i < 3; i++)
        {
            if(!CentralProcessor.Instance.player.getIsExist(i))
            {
                CentralProcessor.Instance.effectSoundManager.PlayButtonClickSound();
                GameObject unit = InstantiateUnit(type, i);
                CalculateCost(unitCosts[type - 1]);
                CentralProcessor.Instance.SumScore(1, 0);
                CentralProcessor.Instance.SumUnit(1, 0);
                CentralProcessor.Instance.createUnitNumber -= 1;
                CentralProcessor.Instance.player.setIsExist(i, true);

                if (VariableManager.Instance.isUnitCostEffect && VariableManager.Instance.UnitEffects.Count == 0)
                {
                    VariableManager.Instance.isUnitCostEffect = false;
                    VariableManager.Instance.UnitCostEffect(-VariableManager.Instance.currentUnitBuff);
                    VariableManager.Instance.isUnitCostEffect = false;
                }
                else if (VariableManager.Instance.isUnitCostEffect && VariableManager.Instance.UnitEffects.Count > 0)
                {
                    VariableManager.Instance.isUnitCostEffect = false;
                    VariableManager.Instance.UnitCostEffect(-VariableManager.Instance.currentUnitBuff);
                    VariableManager.Instance.isUnitCostEffect = false;
                    var n = VariableManager.Instance.UnitEffects.Dequeue();
                    VariableManager.Instance.UnitCostEffect(n);
                }
                return;
            }
        }

        CentralProcessor.Instance.uIManager.fadeOutErrorMessage("공간이 부족합니다");
    }

    private bool CheckCost(int type)
    {
        bool b = true;
        if(unitCosts[type] > int.Parse(CentralProcessor.Instance.currentMoney.text))
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

    GameObject InstantiateUnit(int type, int area)
    {
        GameObject unit = PhotonNetwork.Instantiate(forceName + "_TYPE" + type.ToString(), CentralProcessor.Instance.player.getUnitArea(area).position, Quaternion.Euler(0, CentralProcessor.Instance.player.getQuaternioin(), 0)) as GameObject;
        statusManager.setCreatedUnitStatus(unit);
        return unit;
    }

    private void setUnitIllust()
    {
        for(int i = 0; i < 3; i++)
        {
            illusts[i].sprite = Resources.Load<Sprite>("UnitIllusts/" + forceName + "_Illust_TYPE" + (i + 1).ToString());
        }
    }
}
