using System.Dynamic;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using AnarchyUtility;

public class CentralProcessor : MonoBehaviourPunCallbacks
{
    public static CentralProcessor Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<CentralProcessor>();

            return instance;
        }
    }

    private static CentralProcessor instance;
    public CameraManager cameraManager;
    public UIManager UI;
    public EffectSoundManager effectSoundManager;
    //public Text             whoseTurnText;
    //public Text             currentTurn;
    //public bool             isMaster;


    //public MyUnit           currentUnit;
    //public MyUnit           currentEnemy;
    //public MyBuilding       currentBuilding;
    //public Tile             P1_core_Tile;
    //public Tile             P2_core_Tile;


    //public Button           current_moveButton;
    public Tile[] tiles;
    //public MyBuilding[]     currentBuildings = new MyBuilding[3];
    //public int              createUnitNumber = 3;
    //public int              buildCnt = 1;
    //public Image            waitingPanel;

    //public Queue            que = new Queue();
    //public Cloud            cloud;
    //public Button           decisionButton;
    //public bool             firstDecision = false;
    //public Text             timer;
    //public float            time = 10;
    public Color minimapNormalColor;
    //private float           selectCount;
    ////public IEnumerator      t;
    //public bool             isIgnoreCheck = true;

    //[SerializeField]


    //public int              P1_score = 0;
    //public int              P2_score = 0;
    //public int              P1_totalUnit = 0;
    //public int              P2_totalUnit = 0;
    //public int              P1_totalKill = 0;
    //public int              P2_totalKill = 0;
    //public int              P1_totalMoney = 0;
    //public int              P2_totalMoney = 0;
    //public int              P1_totalOccupation = 0;
    //public int              P2_totalOccupation = 0;

    //public Text             p1_score;
    //public Text             p2_score;
    //public Text             p1_unit;
    //public Text             p2_unit;
    //public Text             p1_kill;
    //public Text             p2_kill;
    //public Text             p1_money;
    //public Text             p2_money;
    //public Text             p1_occupation;
    //public Text             p2_occupation;
    //public Text             GameResult;

    //public Text yes;




    //================================================================================
    // variable
    //================================================================================
    Utility UT = new Utility();

    enum PlayState { ready, play };
    PlayState playState;

    enum WhoseTurn { PLAYER1 = 7, PLAYER2 };
    WhoseTurn whoseTurn;

    bool isPlay = true;
    bool isWaiting = true;
    //public Text         waitingText;

    public Tile currentTile;
    [SerializeField]
    int money;
    int unitCnt = 3;
    int buildCnt = 1;

    float selectCount;
    int time = 180;
    int turnNumber = 0;
    //public Text         turnEndText;
    int score;
    int kill;

    public MyUnit currentUnit;
    public MyBuilding currentBuilding;
    public Tile targetTile;

    public IEnumerator t;

    Player player;
    public Player p1Player;
    public Player p2Player;
    //================================================================================
    // Monobehaviour functions
    //================================================================================
    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
            player = p1Player;
        else
            player = p2Player;

        playState = PlayState.ready;
    }

    private void Start()
    {
        GameManager.instance.audioManager.StartGameSceneBGM();

        currentTile = player.GetCoreTile();
        currentTile.minimap_Tile.color = player.GetPlayerColor();
        currentTile.ShowOcc(currentTile.GetOccupatedScore());

        t = Timer();
    }

    private void Update()
    {
        if (isWaiting)
        {
            if (PhotonNetwork.PlayerList.Length > 1)
            {
                isWaiting = false;
                UT.SetText(UI.waitingText, "Success Matching!");
                Invoke("Matched", 2.0f);
            }
        }
    }

    private void LateUpdate()
    {
        if (playState == PlayState.play && PhotonNetwork.PlayerList.Length < 2 && isPlay)
        {
            isPlay = false;
            playState = PlayState.ready;
            photonView.RPC("EndGameRPC", RpcTarget.All);
        }
    }
    //================================================================================
    // functions
    //================================================================================
    #region
    private void Matched()
    {
        playState = PlayState.play;
        whoseTurn = WhoseTurn.PLAYER1;
        UI.state = UIManager.State.Idle;
        UI.StartCloudAnimation();
    }

    public Player GetPlayer()
    {
        return player;
    }

    public int GetMoney()
    {
        return money;
    }

    public void SetMoney(int m)
    {
        money = m;
        UT.SetText(UI.curMoney, money);
    }

    public void StartTimer()
    {
        t = Timer();
        UT.SetActive(UI.timer, true);
        StartCoroutine(t);
    }

    public void StopTimer()
    {
        StopCoroutine(t);
        UT.SetActive(UI.timer, false);
    }

    private bool CheckUnitsActiveCost()
    {
        MyUnit[] units = GameObject.FindObjectsOfType<MyUnit>();
        foreach (MyUnit unit in units)
        {
            if (unit.gameObject.layer == player.GetLayer() && unit.activeCost > 0)
                return true;
        }
        return false;
    }

    private bool CheckDecision()
    {
        //Decision[] decisions = GameObject.FindObjectsOfType<Decision>();
        //foreach (Decision d in decisions)
        //{
        //    if (d.isDecisionActive && (player.GetLayer() == d.layer))
        //        return true;
        //}

        return false;
    }

    private void NextTurn()
    {
        if ((int)whoseTurn == player.GetLayer())
        {
            MyTurn();
        }
        else
            OtherTurn();
    }

    private void MyTurn()
    {
        UI.state = UIManager.State.Idle;
        UI.SetIdleState();
        StartTimer();
    }

    private void OtherTurn()
    {
        UI.state = UIManager.State.Next;
        UI.SetNextState();
        StopTimer();
    }

    private void GetScore(MyUnit unit)
    {
        if (unit.gameObject.layer != player.GetLayer())
        {
            score += 50;
            kill++;
        }
    }

    private void ShowDecisionIcon(Tile tile)
    {
        if (tile.gameObject.layer == player.GetLayer())
        {
            tile.decisionIcon.GetComponent<DecisionIcon>().layer = player.GetLayer();
            UT.SetActive(tile.decisionIcon, true);
        }
    }

    public int GetBuildCnt()
    {
        return buildCnt;
    }

    public void SetBuildCnt(int num)
    {
        buildCnt = num;
    }

    public int GetUnitCnt()
    {
        return unitCnt;
    }

    public void SetUnitCnt()
    {
        unitCnt -= 1;
    }

    #endregion
    //================================================================================
    // coroutine
    //================================================================================
    IEnumerator Timer()
    {
        selectCount = time;
        while (true)
        {
            if (Mathf.Floor(selectCount) <= 0)
            {
                break;
            }
            else
            {
                selectCount -= Time.deltaTime;
                UI.timer.text = Mathf.Floor(selectCount).ToString();
            }
            yield return null;
        }
        CheckTurn();
    }
    //================================================================================
    // RPC call functions
    //================================================================================
    #region
    public void Exit()
    {
        photonView.RPC("ExitRPC", RpcTarget.Others);
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }

    public void CheckTurn()
    {
        if (UI.GetIsIgnoreCheck())
        {
            if (CheckDecision())
            {
                UI.ShowCheckWindow("남아있는 디시전이 있습니다.^^");
                return;
            }
            else if (CheckUnitsActiveCost())
            {
                UI.ShowCheckWindow("행동력이 남은 유닛이 있습니다.^^");
                return;
            }
        }
        AddTurn();
    }

    public void AddTurn()
    {
        if (UI.checkWindow.gameObject.activeSelf)
            UT.SetActive(UI.checkWindow, false);

        photonView.RPC("AddTurnNumberRPC", RpcTarget.All);
        if (turnNumber % 2 == 0)
        {
            photonView.RPC("NextTurnRPC", RpcTarget.All);
            photonView.RPC("UpdateUnitOnTileRPC", RpcTarget.All);
            photonView.RPC("UpdateTileRPC", RpcTarget.All);
        }
    }

    public void CheckPosOnTile(Tile tile, int posNum, bool check)
    {
        int id = tile.GetComponent<PhotonView>().ViewID;
        photonView.RPC("CheckPosOnTileRPC", RpcTarget.All, id, posNum, check, player.GetLayer());
    }

    public void Attact(int myId, int enemyId)
    {
        photonView.RPC("AttackRPC", RpcTarget.All, myId, enemyId);
    }

    public void DestroyBuilding(GameObject building)
    {
        int id = building.GetComponent<PhotonView>().ViewID;
        photonView.RPC("DestroyBuildingRPC", RpcTarget.All, id);
    }

    public void Move(MyUnit unit, int number)
    {
        int id = unit.GetComponent<PhotonView>().ViewID;
        int curId = currentTile.GetComponent<PhotonView>().ViewID;
        int targetId = targetTile.GetComponent<PhotonView>().ViewID;
        photonView.RPC("MoveRPC", RpcTarget.All, id, curId, targetId, number);
        currentTile = targetTile;
        targetTile = null;
    }
    #endregion
    //================================================================================
    // RPC functions
    //================================================================================
    #region
    [PunRPC]
    private void AddTurnNumberRPC()
    {
        turnNumber += 1;

        if (turnNumber >= 100)
        {
            photonView.RPC("EndGameRPC", RpcTarget.All);
            return;
        }

        if (whoseTurn == WhoseTurn.PLAYER1)
            whoseTurn = WhoseTurn.PLAYER2;
        else
            whoseTurn = WhoseTurn.PLAYER1;

        NextTurn();
    }

    [PunRPC]
    private void NextTurnRPC()
    {
        effectSoundManager.PlayGoldSound();
        UT.SetText(UI.curTurn, ((turnNumber / 2) + 1).ToString() + "   TURN");
        buildCnt = 1;
    }

    [PunRPC]
    private void UpdateUnitOnTileRPC()
    {
        unitCnt = 3;

        MyUnit[] units = FindObjectsOfType<MyUnit>();
        foreach (MyUnit unit in units)
        {
            unit.ActiveCostUpdate();
            unit.currentTile.SetOcupatedScore(unit);
        }
    }

    [PunRPC]
    private void UpdateTileRPC()
    {
        foreach (Tile tile in tiles)
        {
            tile.GetTileMoney();
            if (tile.GetOccupatedScore() >= 3)
            {
                tile.gameObject.layer = 7;
                tile.SetOcupatedScore(3);
                tile.CheckTileOwner(true, false);
                if (!tile.isDecision)
                {
                    tile.isDecision = true;
                    ShowDecisionIcon(tile);
                }
            }
            else if (tile.GetOccupatedScore() <= -3)
            {
                tile.gameObject.layer = 8;
                tile.SetOcupatedScore(-3);
                tile.CheckTileOwner(false, true);
                if (!tile.isDecision)
                {
                    tile.isDecision = true;
                    ShowDecisionIcon(tile);
                }
            }
            else
            {
                tile.gameObject.layer = 9;
                tile.CheckTileOwner(false, false);
            }

            if ((tile.isP1CoreTile && tile.isP2Tile) || (tile.isP2CoreTile && tile.isP1Tile))
            {
                photonView.RPC("EndGameRPC", RpcTarget.All);
                return;
            }

            currentTile.DisappearOcc();
            currentTile.ShowOcc();
        }
    }

    [PunRPC]
    private void ExitRPC()
    {
        UI.SetEndState();
        PhotonNetwork.LeaveRoom();
    }

    [PunRPC]
    private void CheckPosOnTileRPC(int id, int posNum, bool check, int layer)
    {
        Tile[] tiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in tiles)
        {
            if (tile.GetComponent<PhotonView>().ViewID == id)
            {
                if (layer == 7)
                    tile.isP1_unitArea[posNum] = check;
                else
                    tile.isP2_unitArea[posNum] = check;
            }
        }
    }

    [PunRPC]
    private void AttackRPC(int myId, int enemyId)
    {
        MyUnit unit = new MyUnit();
        MyUnit enemy = new MyUnit();

        MyUnit[] units = FindObjectsOfType<MyUnit>();
        foreach (MyUnit u in units)
        {
            if (u.GetComponent<PhotonView>().ViewID == myId)
                unit = u;
            else if (u.GetComponent<PhotonView>().ViewID == enemyId)
                enemy = u;
        }

        if (unit.offensive >= enemy.defensive)
            enemy.GetAttack();
        else
        {
            enemy.accDamage += unit.offensive;
            if (enemy.accDamage >= enemy.defensive)
                enemy.GetAttack();
        }

        unit.activeCost = 0;

        if (enemy.hp <= 0)
        {
            GetScore(enemy);
            enemy.currentTile.DeleteUnit(enemy);
            Destroy(enemy.gameObject);
            effectSoundManager.PlayDestroySound();
        }
        UI.OffReadyAttack();
    }

    [PunRPC]
    private void DestroyBuildingRPC(int id)
    {
        MyBuilding[] buildings = FindObjectsOfType<MyBuilding>();
        foreach (MyBuilding building in buildings)
        {
            if (building.GetComponent<PhotonView>().ViewID == id)
            {
                Destroy(building.gameObject);
                return;
            }
        }
    }

    [PunRPC]
    private void MoveRPC(int id, int curId, int targetId, int num)
    {
        Tile[] tiles = FindObjectsOfType<Tile>();
        Tile curTile = new Tile();
        Tile target = new Tile();
        foreach(Tile tile in tiles)
        {
            if (tile.GetComponent<PhotonView>().ViewID == curId)
                curTile = tile;
            if (tile.GetComponent<PhotonView>().ViewID == targetId)
                target = tile;
        }
        MyUnit[] units = FindObjectsOfType<MyUnit>();
        foreach (MyUnit unit in units)
        {
            if (id == unit.GetComponent<PhotonView>().ViewID)
            {
                if (unit.gameObject.layer == 7)
                    unit.transform.position = target.P1_unitArea[num].position;
                else
                    unit.transform.position = target.P2_unitArea[num].position;
                curTile.SetCheckPos(unit.areaPosNumber, false, unit.gameObject.layer);
                curTile.SetUnits(unit.areaPosNumber, unit.gameObject.layer);
                target.SetCheckPos(num, true, unit.gameObject.layer);
                target.SetUnits(unit, num, unit.gameObject.layer);
                unit.areaPosNumber = num;
                UI.SetIdleState();
                return;
            }
        }
    }
    #endregion
    //================================================================================
    //================================================================================





    //public void CreatedUnitAreaCheck(bool master, bool check, int area)
    //{
    //    if(master)
    //    {
    //        photonView.RPC("CreatedUnitP1AreaCheckRPC", RpcTarget.All, check, area);
    //    }
    //    else
    //    {
    //        photonView.RPC("CreatedUnitP2AreaCheckRPC", RpcTarget.All, check, area);
    //    }
    //}

    // Tile을 넘겨받아 해당 타일의 어떤 유닛 자리에 유닛이 있는지 확인하는 함수
    //public void CheckUnitArea(int layer, int id, bool check, int num)
    //{
    //    photonView.RPC("CheckUnitAreaRPC", RpcTarget.All, layer, id, check, num);
    //}
    // 타일의 어느 포지션에 유닛이 빠져나가고 들어왔는지 체크하는 함수





    //public void CheckCoreTileUnits(int unitId, int num, bool isMaster)
    //{
    //    photonView.RPC("CheckCoreTileUnitsRPC", RpcTarget.All, unitId, num, isMaster);
    //}

    //public void CheckTileUnits(int tileId, int unitId, int num, bool isMaster, bool check)
    //{
    //    photonView.RPC("CheckTileUnitsRPC", RpcTarget.All, tileId, unitId, num, isMaster, check);
    //}

    public void ApplyCreateUnitVariable(int id, int type)
    {
        switch (type)
        {
            case 1:
                photonView.RPC("ApplyCreateUnitVariableRPC", RpcTarget.All, id, VariableManager.Instance.war_hp, VariableManager.Instance.war_off, VariableManager.Instance.war_def, VariableManager.Instance.war_act);
                break;
            case 2:
                photonView.RPC("ApplyCreateUnitVariableRPC", RpcTarget.All, id, VariableManager.Instance.arc_hp, VariableManager.Instance.arc_off, VariableManager.Instance.arc_def, VariableManager.Instance.arc_act);
                break;
            case 3:
                photonView.RPC("ApplyCreateUnitVariableRPC", RpcTarget.All, id, VariableManager.Instance.mag_hp, VariableManager.Instance.mag_off, VariableManager.Instance.mag_def, VariableManager.Instance.mag_act);
                break;
        }
    }





    public void BuildingUpgrade(int buildingId)
    {
        photonView.RPC("BuildingUpgradeRPC", RpcTarget.All, buildingId);
    }

    public void ApplyUnitOffenceEffect(int layer, int war_off, int arc_off, int mag_off)
    {
        photonView.RPC("ApplyUnitOffenceEffectRPC", RpcTarget.All, layer, war_off, arc_off, mag_off);
    }

    public void ApplyUnitDefenceEffect(int layer, int war_def, int arc_def, int mag_def)
    {
        photonView.RPC("ApplyUnitDefenceEffectRPC", RpcTarget.All, layer, war_def, arc_def, mag_def);
    }

    public void ApplyUnitCurrentTile(int unitId, int tileId)
    {
        photonView.RPC("ApplyUnitCurrentTileRPC", RpcTarget.All, unitId, tileId);
    }

    public void ApplyUnitActiveCost(int id, int cost)
    {
        photonView.RPC("ApplyUnitActiveCostRPC", RpcTarget.All, id, cost);
    }

    public void SumScore(int p1, int p2)
    {
        photonView.RPC("SumScoreRPC", RpcTarget.All, p1, p2);
    }

    public void SumUnit(int p1, int p2)
    {
        photonView.RPC("SumUnitRPC", RpcTarget.All, p1, p2);
    }

    public void SumKill(int p1, int p2)
    {
        photonView.RPC("SumKillRPC", RpcTarget.All, p1, p2);
    }

    public void SumMoney(int p1, int p2)
    {
        photonView.RPC("SumMoneyRPC", RpcTarget.All, p1, p2);
    }

    public void SumOccupation(int p1, int p2)
    {
        photonView.RPC("SumOccupationRPC", RpcTarget.All, p1, p2);
    }

    public void CreateBuilding()
    {
        photonView.RPC("CreateBuildingRPC", RpcTarget.All);
    }

    #region // RPC functions










    //[PunRPC]
    //private void CreatedUnitP1AreaCheckRPC(bool check, int area)
    //{
    //    P1_core_Tile.isP1_unitArea[area] = check;
    //}

    //[PunRPC]
    //private void CreatedUnitP2AreaCheckRPC(bool check, int area)
    //{
    //    P2_core_Tile.isP2_unitArea[area] = check;
    //}

    //[PunRPC]
    //private void CheckUnitAreaRPC(int layer, int id, bool check, int num)
    //{
    //    foreach(Tile t in tiles)
    //    {
    //        if(t.GetComponent<PhotonView>().ViewID == id)
    //        {
    //            if(layer == 7)
    //            {
    //                t.isP1_unitArea[num] = check;
    //            }
    //            else
    //            {
    //                t.isP2_unitArea[num] = check;
    //            }
    //            return;
    //        }
    //    }
    //}

    //[PunRPC]
    //private void CheckCoreTileUnitsRPC(int unitId, int num, bool isMaster)
    //{
    //    GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
    //    foreach(GameObject unit in units)
    //    {
    //        if(unit.GetComponent<PhotonView>().ViewID == unitId)
    //        {
    //            unit.GetComponent<MyUnit>().myNum = num;
    //            if(isMaster)
    //            {
    //                P1_core_Tile.GetComponent<Tile>().P1_units[num] = unit.GetComponent<MyUnit>();
    //                P1_core_Tile.MoveMapButton.GetComponent<MoveUnit>().p1unit[num].gameObject.SetActive(true);
    //                unit.GetComponent<MyUnit>().currentTile = P1_core_Tile;
    //            }
    //            else
    //            {
    //                P2_core_Tile.GetComponent<Tile>().P2_units[num] = unit.GetComponent<MyUnit>();
    //                P2_core_Tile.MoveMapButton.GetComponent<MoveUnit>().p2unit[num].gameObject.SetActive(true);
    //                unit.GetComponent<MyUnit>().currentTile = P2_core_Tile;
    //            }
    //            return;
    //        }
    //    }
    //}

    //[PunRPC]
    //private void CheckTileUnitsRPC(int tileId, int unitId, int num, bool isMaster, bool check)
    //{
    //    foreach(Tile t in tiles)
    //    {
    //        if(t.GetComponent<PhotonView>().ViewID == tileId)
    //        {
    //            GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
    //            foreach(GameObject unit in units)
    //            {
    //                if(unit.GetComponent<PhotonView>().ViewID == unitId)
    //                {
    //                    if(isMaster)
    //                    {
    //                        if(!check)
    //                        {
    //                            t.P1_units[num] = null;
    //                            t.MoveMapButton.GetComponent<MoveUnit>().p1unit[num].gameObject.SetActive(false);
    //                        }
    //                        else
    //                        {
    //                            unit.GetComponent<MyUnit>().myNum = num;
    //                            t.P1_units[num] = unit.GetComponent<MyUnit>();
    //                            t.MoveMapButton.GetComponent<MoveUnit>().p1unit[num].gameObject.SetActive(true);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if(!check)
    //                        {
    //                            t.P2_units[num] = null;
    //                            t.MoveMapButton.GetComponent<MoveUnit>().p2unit[num].gameObject.SetActive(false);
    //                        }
    //                        else
    //                        {
    //                            unit.GetComponent<MyUnit>().myNum = num;
    //                            t.P2_units[num] = unit.GetComponent<MyUnit>();
    //                            t.MoveMapButton.GetComponent<MoveUnit>().p2unit[num].gameObject.SetActive(true);
    //                        }
    //                    }
    //                    return;
    //                }
    //            }
    //        }
    //    }
    //}

    [PunRPC]
    private void ApplyCreateUnitVariableRPC(int id, int hp, int off, int def, int act)
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject unit in units)
        {
            if (unit.GetComponent<PhotonView>().ViewID == id)
            {
                unit.GetComponent<MyUnit>().hp = hp;
                unit.GetComponent<MyUnit>().offensive = off;
                unit.GetComponent<MyUnit>().defensive = def;
                unit.GetComponent<MyUnit>().activeCost = act;
                return;
            }
        }
    }



    //[PunRPC]
    //private void BuildingUpgradeRPC(int buildingId)
    //{
    //    MyBuilding[] buildings = GameObject.FindObjectsOfType<MyBuilding>();
    //    foreach(MyBuilding b in buildings)
    //    {
    //        if(b.GetComponent<PhotonView>().ViewID == buildingId)
    //        {
    //            Destroy(b.gameObject);
    //            return;
    //        }
    //    }
    //}

    [PunRPC]
    private void ApplyUnitOffenceEffectRPC(int layer, int war_off, int arc_off, int mag_off)
    {
        MyUnit[] units = GameObject.FindObjectsOfType<MyUnit>();
        foreach (MyUnit unit in units)
        {
            if (unit.gameObject.layer == layer)
            {
                switch (unit.type)
                {
                    case 1:
                        unit.offensive = war_off;
                        break;
                    case 2:
                        unit.offensive = arc_off;
                        break;
                    case 3:
                        unit.offensive = mag_off;
                        break;
                }
            }
        }
    }

    [PunRPC]
    private void ApplyUnitDefenceEffectRPC(int layer, int war_def, int arc_def, int mag_def)
    {
        MyUnit[] units = GameObject.FindObjectsOfType<MyUnit>();
        foreach (MyUnit unit in units)
        {
            if (unit.gameObject.layer == layer)
            {
                switch (unit.type)
                {
                    case 1:
                        unit.defensive = war_def;
                        break;
                    case 2:
                        unit.defensive = arc_def;
                        break;
                    case 3:
                        unit.defensive = mag_def;
                        break;
                }
            }
        }
    }



    [PunRPC]
    private void ApplyUnitCurrentTileRPC(int unitId, int tileId)
    {
        MyUnit[] units = GameObject.FindObjectsOfType<MyUnit>();
        foreach (MyUnit unit in units)
        {
            if (unit.GetComponent<PhotonView>().ViewID == unitId)
            {
                foreach (Tile t in tiles)
                {
                    if (t.GetComponent<PhotonView>().ViewID == tileId)
                    {
                        unit.currentTile = t;
                        return;
                    }
                }
            }
        }
    }

    [PunRPC]
    private void ApplyUnitActiveCostRPC(int id, int cost)
    {
        MyUnit[] units = GameObject.FindObjectsOfType<MyUnit>();
        foreach (MyUnit unit in units)
        {
            if (unit.GetComponent<PhotonView>().ViewID == id)
            {
                unit.activeCost += cost;
                return;
            }
        }
    }


    //    [PunRPC]
    //    private void EndGameRPC()
    //    {
    //        uIManager.SetEndState();
    //        if(P1_score > P2_score)
    //        {
    //            if(isMaster)
    //            {
    //                GameResult.text = "V I C T O R Y";
    //            }
    //            else
    //            {
    //                GameResult.text = "L O S E";
    //            }
    //        }
    //        else if(P1_score == P2_score)
    //        {
    //            GameResult.text = "T I E";
    //        }
    //        else
    //        {
    //            if(isMaster)
    //            {
    //                GameResult.text = "L O S E";
    //            }
    //            else
    //            {
    //                GameResult.text = "V I C T O R Y";
    //            }
    //        }
    //        p1_score.text = P1_score.ToString();
    //        p2_score.text = P2_score.ToString();
    //        p1_unit.text = P1_totalUnit.ToString();
    //        p2_unit.text = P2_totalUnit.ToString();
    //        p1_kill.text = P1_totalKill.ToString();
    //        p2_kill.text = P2_totalKill.ToString();
    //        p1_money.text = P1_totalMoney.ToString();
    //        p2_money.text = P2_totalMoney.ToString();
    //        p1_occupation.text = P1_totalOccupation.ToString();
    //        p2_occupation.text = P2_totalOccupation.ToString();
    //    }

    //    [PunRPC]
    //    private void SumScoreRPC(int p1, int p2)
    //    {
    //        P1_score += p1;
    //        P2_score += p2;
    //        p1_score.text = P1_score.ToString();
    //        p2_score.text = P2_score.ToString();
    //    }

    //    [PunRPC]
    //    private void SumUnitRPC(int p1, int p2)
    //    {
    //        P1_totalUnit += p1;
    //        P2_totalUnit += p2;
    //        p1_unit.text = P1_totalUnit.ToString();
    //        p2_unit.text = P2_totalUnit.ToString();
    //    }

    //    [PunRPC]
    //    private void SumKillRPC(int p1, int p2)
    //    {
    //        P1_totalKill += p1;
    //        P2_totalKill += p2;
    //        p1_kill.text = P1_totalKill.ToString();
    //        p2_kill.text = P2_totalKill.ToString();
    //    }

    //    [PunRPC]
    //    private void SumMoneyRPC(int p1, int p2)
    //    {
    //        P1_totalMoney += p1;
    //        P2_totalMoney += p2;
    //        p1_money.text = P1_totalMoney.ToString();
    //        p2_money.text = P2_totalMoney.ToString();
    //    }

    //    [PunRPC]
    //    private void SumOccupationRPC(int p1, int p2)
    //    {
    //        P1_totalOccupation += p1;
    //        P2_totalOccupation += p2;
    //        p1_occupation.text = P1_totalOccupation.ToString();
    //        p2_occupation.text = P2_totalOccupation.ToString();
    //    }

    //    [PunRPC]
    //    private void CreateBuildingRPC()
    //    {

    //    }
    //#endregion
    //}

    //[Serializable]
    //public class Player
    //{
    //    [SerializeField]
    //    private Transform cam_start_point;

    //    public Transform getCamPoint()
    //    {
    //        return cam_start_point;
    //    }
    #endregion
}