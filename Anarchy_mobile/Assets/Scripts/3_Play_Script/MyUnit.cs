using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnarchyUtility;

public class MyUnit : MonoBehaviourPun
{
    public int              type;
    public int              cost;
    public int              hp;
    public int activeCost;
    public Sprite           illust;
    
    public int              accDamage = 0;
    public string           unitName;
    public int              defensive;
    public int              offensive;
    public int myNum;

    public Tile             currentTile;
    public bool             isAttackready = false;

    public ParticleSystem   particleSystem;
    public ParticleSystem   attackParticle;

    bool                    isClicked;

    Utility                 UT = new Utility();
    UIManager               UI;
    CentralProcessor        CP;
    VariableManager         VM;

    public void OnClick()
    {
        UT.SetManager(ref UI, ref CP);
        if (CP.currentBuilding != null)
            CP.currentBuilding.CloseInfo();

        if (isClicked)
        {
            CloseInfo();
            return;
        }

        isClicked = true;
        switch(UI.state)
        {
            case UIManager.State.Idle:
                Ready();
                break;
            case UIManager.State.Attack:
                Attack();
                break;
            case UIManager.State.Next:
                ShowInfo();
                break;
        }
    }

    private void Ready()
    {
        if (this.gameObject.layer == CP.player.GetLayer())
        {
            StartParticle();
            UT.SetActive(UI.unitButtonPanel, true);
            if(CP.currentUnit != null)
                CP.currentUnit.CloseInfo();

            CP.currentUnit = this.gameObject.GetComponent<MyUnit>();
        }
        ShowInfo();
    }

    private void ShowInfo()
    {
        UI.ShowUnitInfo(this.gameObject.GetComponent<MyUnit>());
    }

    public void CloseInfo()
    {
        isClicked = false;
        StopParticle();
        UI.CloseUnitInfo();
    }

    private void Attack()
    {
        if (this.gameObject.layer == CP.player.GetLayer())
            return;

        CP.Attact(CP.currentUnit.GetComponent<PhotonView>().ViewID, this.gameObject.GetComponent<PhotonView>().ViewID);
    }

    private void StopParticle()
    {
        particleSystem.Clear();
        UT.SetActive(particleSystem, false);
    }

    private void StartParticle()
    {
        UT.SetActive(particleSystem, true);
        particleSystem.Play();
    }

    public void ActiveCostUpdate()
    {
        VM = VariableManager.Instance;
        switch (type)
        {
            case 1:
                activeCost = VM.war_act;
                break;
            case 2:
                activeCost = VM.arc_act;
                break;
            case 3:
                activeCost = VM.mag_act;
                break;
        }
    }
}