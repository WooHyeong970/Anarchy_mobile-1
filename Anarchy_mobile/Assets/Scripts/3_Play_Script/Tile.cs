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
        if(this.gameObject.layer == CP.player.GetLayer())
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
        this.minimap_Tile.color = CP.player.GetPlayerColor();
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
}
