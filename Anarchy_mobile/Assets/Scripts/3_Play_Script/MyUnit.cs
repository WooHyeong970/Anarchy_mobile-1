//using System.Net.NetworkInformation;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.UI;

public class MyUnit : MonoBehaviourPun, IPointerClickHandler
{
    public int type;
    public int cost;
    public int hp;
    public Sprite illust;
    public int activeCost;
    public int accDamage = 0;
    public string unit_name;
    public int myNum;
    public int defensive;
    public int offensive;
    public Tile currentTile;
    public bool isAttackready = false;
    bool isMaster;
    public ParticleSystem particleSystem;
    public ParticleSystem attackParticle;
    bool isReady;


    private void Start()
    {
        isMaster = CentralProcessor.Instance.isMaster;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(currentTile != CentralProcessor.Instance.currentTile)
            return;
        // ======================================================================
        switch(CentralProcessor.Instance.uIManager.state)
        {
            case UIManager.State.Idle:
                if(this.gameObject.layer == CentralProcessor.Instance.player.getLayer())
                    Ready();
                else
                    ShowInfo();
                break;
            case UIManager.State.Attack:
                if (this.gameObject.layer == CentralProcessor.Instance.player.getLayer())
                {

                }
                else
                {

                }
                break;
            case UIManager.State.Next:
                if (this.gameObject.layer == CentralProcessor.Instance.player.getLayer())
                {

                }
                else
                {

                }
                break;
        }
        // ======================================================================
        if (CentralProcessor.Instance.uIManager.state == UIManager.State.Idle)
        {
            if((isMaster && this.gameObject.layer == 8) || (!isMaster && this.gameObject.layer == 7))
            {
                if(CentralProcessor.Instance.currentEnemy != this.gameObject.GetComponent<MyUnit>())
                {
                    CentralProcessor.Instance.uIManager.InfoWindowReset();
                    CentralProcessor.Instance.currentEnemy = this.gameObject.GetComponent<MyUnit>();
                    ShowInfo();
                }
                else
                {
                    CentralProcessor.Instance.uIManager.InfoWindowReset();
                }
            }   
            else
            {
                if(CentralProcessor.Instance.currentUnit != this.gameObject.GetComponent<MyUnit>())
                {
                    CentralProcessor.Instance.uIManager.InfoWindowReset();
                    Ready();
                }
                else
                {
                    CentralProcessor.Instance.uIManager.InfoWindowReset();
                }
            }
        }
        else if(CentralProcessor.Instance.uIManager.state == UIManager.State.Attack)
        {
            if((isMaster && this.gameObject.layer == 8) || (!isMaster && this.gameObject.layer == 7))
            {
                if(CentralProcessor.Instance.uIManager.state == UIManager.State.Attack)
                {
                    CentralProcessor.Instance.Attact(CentralProcessor.Instance.currentUnit.GetComponent<PhotonView>().ViewID, this.gameObject.GetComponent<PhotonView>().ViewID);
                    if(CentralProcessor.Instance.currentUnit == null)
                    {
                        return;
                    }
                }
            }
        }
        else if(CentralProcessor.Instance.uIManager.state == UIManager.State.Next)
        {
            if((isMaster && this.gameObject.layer == 8) || (!isMaster && this.gameObject.layer == 7))
            {
                if(CentralProcessor.Instance.currentEnemy != this.gameObject.GetComponent<MyUnit>())
                {
                    CentralProcessor.Instance.uIManager.InfoWindowReset();
                    CentralProcessor.Instance.currentEnemy = this.gameObject.GetComponent<MyUnit>();
                    ShowInfo();
                }
                else
                {
                    CentralProcessor.Instance.uIManager.InfoWindowReset();
                }
            }   
            else
            {
                if(CentralProcessor.Instance.currentUnit != this.gameObject.GetComponent<MyUnit>())
                {
                    CentralProcessor.Instance.uIManager.InfoWindowReset();
                    CentralProcessor.Instance.currentUnit = this.gameObject.GetComponent<MyUnit>();
                    CentralProcessor.Instance.currentUnit.particleSystem.gameObject.SetActive(true);
                    CentralProcessor.Instance.currentUnit.particleSystem.Play();
                    ShowInfo();
                }
                else
                {
                    CentralProcessor.Instance.uIManager.InfoWindowReset();
                }
            }
        }
    }

    public void Ready()
    {
        if(!isReady)
        {
            isReady = true;
            CentralProcessor.Instance.uIManager.InfoWindowReset();
            CentralProcessor.Instance.currentUnit = this.gameObject.GetComponent<MyUnit>();
            CentralProcessor.Instance.currentUnit.particleSystem.gameObject.SetActive(true);
            CentralProcessor.Instance.currentUnit.particleSystem.Play();
            ShowInfo();
            CentralProcessor.Instance.uIManager.unitButtonPanel.gameObject.SetActive(true);
        }
        else
        {
            isReady = false;
            CentralProcessor.Instance.uIManager.InfoWindowReset();
        }
    }

    public void ShowInfo()
    {
        if (CentralProcessor.Instance.currentEnemy != this.gameObject.GetComponent<MyUnit>())
        {
            CentralProcessor.Instance.uIManager.InfoWindowReset();
            CentralProcessor.Instance.currentEnemy = this.gameObject.GetComponent<MyUnit>();
            CentralProcessor.Instance.uIManager.ShowUnitInfo(hp, illust, unit_name, activeCost, offensive, defensive);
        }
        else
            CentralProcessor.Instance.uIManager.InfoWindowReset();
    }

    public void ActiveCostUpdate()
    {
        switch(type)
        {
            case 1:
            activeCost =  VariableManager.Instance.war_act;
            break;
            case 2:
            activeCost = VariableManager.Instance.arc_act;
            break;
            case 3:
            activeCost = VariableManager.Instance.mag_act;
            break;
        }
    }
}