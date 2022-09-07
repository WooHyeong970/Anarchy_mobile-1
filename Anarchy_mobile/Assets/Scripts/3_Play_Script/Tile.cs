using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnarchyUtility;

public class Tile : MonoBehaviourPun
{
    public bool isP1Tile = false;
    public bool isP2Tile = false;
    public bool isP1CoreTile = false;
    public bool isP2CoreTile = false;
    public float occupationCost = 0;
    public int row;
    public int col;
    public bool[]       isP1_unitArea = new bool[3];
    public bool[]       isP2_unitArea = new bool[3];
    public Transform[]  P1_unitArea = new Transform[3];
    public Transform[]  P2_unitArea = new Transform[3];
    public MyUnit[]     P1_units = new MyUnit[3];
    public MyUnit[]     P2_units = new MyUnit[3];
    bool isMaster;
    public Transform cameraPoint;
    public bool isDecision = true;
    public Image minimap_Tile;
    public GameObject decisionIcon;
    public int money;
    public Button MoveMapButton;
    public GameObject[] occBlue;
    public GameObject[] occRed;
    public int gold;
    int occupatedScore;

    Utility             UT = new Utility();
    Player              player = CentralProcessor.Instance.GetPlayer();
    UIManager           UI;
    CentralProcessor    CP;

    public void OnClick()
    {
        UT.SetManager(ref UI, ref CP);
        if (UI.state == UIManager.State.Attack) return;

        UI.OffInOf();
        MoveTile();
        if (UI.state == UIManager.State.Idle)
            UI.InfoWindowReset();
    }

    public void GetTileMoney()
    {
        if(this.gameObject.layer == player.GetLayer())
        {
            int money = CP.GetMoney() + gold;
            CP.SetMoney(money);
        }
    }

    public void MoveTile()
    {
        CP.currentTile.DisappearOcc();
        CP.cameraManager.transform.position = cameraPoint.position;
        CP.currentTile.minimap_Tile.color = CP.minimapNormalColor;
        CP.currentTile = this.gameObject.GetComponent<Tile>();
        CP.currentTile.ShowOcc();
        this.minimap_Tile.color = player.GetPlayerColor();
    }

    public void DisappearOcc()
    {
        foreach (GameObject o in occBlue)
            UT.SetActive(o, false);

        foreach (GameObject o in occRed)
            UT.SetActive(o, false);
    }

    public void ShowOcc()
    {
        if (occupationCost > 0)
        {
            for (int i = 0; i < occupationCost; i++)
                UT.SetActive(occBlue[i], true);
        }
        else if (occupationCost < 0)
        {
            for (int i = 0; i < -(occupationCost); i++)
                UT.SetActive(occRed[i], true);
        }
    }

    public void ShowOcc(int score)
    {
        if (occupationCost > 0)
        {
            for (int i = 0; i < score; i++)
                UT.SetActive(occBlue[i], true);
        }
        else if (occupationCost < 0)
        {
            for (int i = 0; i < -(score); i++)
                UT.SetActive(occRed[i], true);
        }
    }

    public int GetOccupatedScore()
    {
        return occupatedScore;
    }

    public void SetOcupatedScore(MyUnit unit)
    {
        if (unit.gameObject.layer == player.GetLayer())
            occupatedScore += unit.occScore;
    }

    public void SetOcupatedScore(int score)
    {
        occupatedScore = score;
    }

    public bool GetCheckPos(int n, int layer)
    {
        if(layer == 7)
            return isP1_unitArea[n];
        else
            return isP2_unitArea[n];
    }

    public void SetCheckPos(int n, bool check, int layer)
    {
        if(layer == 7)
            isP1_unitArea[n] = check;
        else
            isP2_unitArea[n] = check;
    }

    public void SetUnits(MyUnit unit, int n, int layer)
    {
        if (layer == 7)
            P1_units[n] = unit;
        else
            P2_units[n] = unit;
    }

    private void SetUnits(int n, int layer)
    {
        if (layer == 7)
            P1_units[n] = null;
        else
            P2_units[n] = null;
    }

    public void DeleteUnit(MyUnit unit)
    {
        SetCheckPos(unit.areaPosNumber, false, unit.gameObject.layer);
        SetUnits(unit.areaPosNumber, unit.gameObject.layer);
        if(unit.gameObject.layer == 7)
            MoveMapButton.GetComponent<MoveUnit>().p1unit[unit.areaPosNumber].gameObject.SetActive(false);
        else
            MoveMapButton.GetComponent<MoveUnit>().p2unit[unit.areaPosNumber].gameObject.SetActive(false);
    }

    public void CheckTileOwner(bool p1, bool p2)
    {
        isP1Tile = p1;
        isP2Tile = p2;
        UT.SetActive(transform.Find("flag_Blue"), p1);
        UT.SetActive(transform.Find("flag_Red"), p2);
    }
}
