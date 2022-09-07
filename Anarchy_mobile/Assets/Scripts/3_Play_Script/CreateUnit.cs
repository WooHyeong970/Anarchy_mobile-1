using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Status;
using AnarchyUtility;

public class CreateUnit : MonoBehaviourPunCallbacks
{
    public Button[]     unitButtons = new Button[3];
    public Text[]       costTexts = new Text[3];
    public Image[]      illusts = new Image[3];

    [SerializeField]
    int[]               unitCosts = new int[3];
    string              forceName;

    StatusManager       statusManager = new StatusManager();
    Player              player = CentralProcessor.Instance.GetPlayer();
    Utility             UT = new Utility();
    UIManager           UI;
    CentralProcessor    CP;
    VariableManager     VM;

    private void Start()
    {
        forceName = GameManager.instance.playerData.getForceName();
        unitButtons[0].onClick.AddListener(() => CreateUnitFunc(1));
        unitButtons[1].onClick.AddListener(() => CreateUnitFunc(2));
        unitButtons[2].onClick.AddListener(() => CreateUnitFunc(3));

        for (int i = 0; i < 3; i++)
            illusts[i].sprite = Resources.Load<Sprite>("UnitIllusts/" + forceName + "_Illust_TYPE" + (i + 1).ToString());
    }

    private void CreateUnitFunc(int type)
    {
        UT.SetManager(ref UI, ref CP, ref VM);

        if(CP.GetUnitCnt() <= 0)
        {
            UI.fadeOutErrorMessage("소환 횟수 초과");
            return;
        }

        if(UT.CheckCost(unitCosts[type - 1], CP.GetMoney()))
        {
            UI.fadeOutErrorMessage("돈이 부족합니다");
            return;
        }

        for(int i = 0; i < 3; i++)
        {
            if(!player.GetCoreTile().GetCheckPos(i, player.GetLayer()))
            {
                CP.effectSoundManager.PlayButtonClickSound();
                CP.SetMoney(CP.GetMoney() - unitCosts[type - 1]);

                GameObject unit = InstantiateUnit(type, i);
                CP.SumScore(1, 0);
                CP.SumUnit(1, 0);
                CP.SetUnitCnt();
                player.GetCoreTile().SetCheckPos(i, true, player.GetLayer());

                if (VM.isUnitCostEffect && VM.UnitEffects.Count == 0)
                {
                    VM.isUnitCostEffect = false;
                    VM.UnitCostEffect(-VM.currentUnitBuff);
                    VM.isUnitCostEffect = false;
                }
                else if (VM.isUnitCostEffect && VM.UnitEffects.Count > 0)
                {
                    VM.isUnitCostEffect = false;
                    VM.UnitCostEffect(-VM.currentUnitBuff);
                    VM.isUnitCostEffect = false;
                    var n = VM.UnitEffects.Dequeue();
                    VM.UnitCostEffect(n);
                }
                return;
            }
        }
        UI.fadeOutErrorMessage("공간이 부족합니다");
    }

    GameObject InstantiateUnit(int type, int area)
    {
        GameObject unit = PhotonNetwork.Instantiate(forceName + "_TYPE" + type.ToString(), player.GetUnitArea(area).position, Quaternion.Euler(0, player.GetQuaternioin(), 0)) as GameObject;
        statusManager.setCreatedUnitStatus(unit);
        return unit;
    }
}
