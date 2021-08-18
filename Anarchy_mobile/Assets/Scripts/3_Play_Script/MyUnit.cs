using System.Net.NetworkInformation;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyUnit : MonoBehaviourPun, IPointerClickHandler
{
    public int type;
    public int cost;
    public float current_hp;
    public float max_hp;
    public Sprite illust;
    public int activeCost;
    public string unit_name;
    public int myNum;
    public int defensive;
    public int offensive;
    public Tile currentTile;
    public bool isClicked = false;
    public bool isAttackready = false;
    bool isMaster;

    private void Start()
    {
        isMaster = CentralProcessor.Instance.isMaster;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if((isMaster && this.gameObject.layer == 8) || (!isMaster && this.gameObject.layer == 7))
        {
            if(CentralProcessor.Instance.uIManager.state == UIManager.State.Attack)
            {
                
            }
            else
            {
                return;
            }
        }

        if(CentralProcessor.Instance.uIManager.state != UIManager.State.Idle)
        {
            return;
        }

        if(currentTile != CentralProcessor.Instance.currentTile)
        {
            return;
        }

        if(!isClicked)
        {
            OnReady();
        }
        else
        {
            OffReady();
        }
    }

    public void OnReady()
    {
        isClicked = true;
        if(CentralProcessor.Instance.currentUnit == this.gameObject.GetComponent<MyUnit>())
        {
            CentralProcessor.Instance.uIManager.ShowUnitInfo(max_hp, current_hp, illust, unit_name, activeCost);
        }
        else if(CentralProcessor.Instance.currentUnit == null)
        {
            CentralProcessor.Instance.currentUnit = this.gameObject.GetComponent<MyUnit>();
            CentralProcessor.Instance.uIManager.ShowUnitInfo(max_hp, current_hp, illust, unit_name, activeCost);
        }
        else
        {
            CentralProcessor.Instance.currentUnit.isClicked = false;
            CentralProcessor.Instance.currentUnit = this.gameObject.GetComponent<MyUnit>();
            CentralProcessor.Instance.uIManager.ShowUnitInfo(max_hp, current_hp, illust, unit_name, activeCost);
        }
    }

    public void OffReady()
    {
        isClicked = false;
        CentralProcessor.Instance.currentUnit = null;
        CentralProcessor.Instance.uIManager.CloseUnitInfo();
    }
}
