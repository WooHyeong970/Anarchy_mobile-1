using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnarchyUtility;

public class MyUnit : MonoBehaviourPun
{
    public int          type;
    public int          cost;
    public int          hp;
    public int          activeCost;
    public int          defensive;
    public int          offensive;
    public Sprite       illust;
    public string       unitName;
    public int          accDamage = 0;
    
    public int          areaPosNumber;
    public Tile         currentTile;
    public int          occScore;

    [SerializeField]
    ParticleSystem      particle;
    [SerializeField]
    ParticleSystem      attackParticle;

    [SerializeField]
    bool                isClicked;

    Utility             UT = new Utility();
    UIManager           UI;
    CentralProcessor    CP;
    VariableManager     VM;

    public void OnClick()
    {
        Debug.Log("Debug 1");
        UT.SetManager(ref UI, ref CP);
        Debug.Log("Debug 2");
        //if (UI.unitInfoPanel.gameObject.activeSelf)
        //{
        //    CloseInfo();
        //    return;
        //}

        Debug.Log("Debug 3");
        if (isClicked)
        {
            CloseInfo();
            return;
        }
        Debug.Log("Debug 4");

        isClicked = true;
        Debug.Log("Debug 5");
        switch (UI.state)
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
        Debug.Log("Debug 6");
        if (this.gameObject.layer == CP.GetPlayer().GetLayer())
        {
            Debug.Log("Debug 7");
            StartParticle();
            Debug.Log("Debug 11");
            UT.SetActive(UI.unitButtonPanel, true);
            Debug.Log("Debug 12");
            if (CP.currentUnit != null)
                CP.currentUnit.CloseInfo();
            Debug.Log("Debug 13");
            CP.currentUnit = this.gameObject.GetComponent<MyUnit>();
        }
        ShowInfo();
    }

    private void ShowInfo()
    {
        Debug.Log("Debug 1");
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
        if (this.gameObject.layer == CP.GetPlayer().GetLayer())
            return;

        CP.Attact(CP.currentUnit.GetComponent<PhotonView>().ViewID, this.gameObject.GetComponent<PhotonView>().ViewID);
    }

    private void StopParticle()
    {
        particle.Clear();
        UT.SetActive(particle, false);
    }

    private void StartParticle()
    {
        Debug.Log("Debug 8");
        UT.SetActive(particle, true);
        particle.Play();
        Debug.Log("Debug 10");
    }

    public void GetAttack()
    {
        hp -= 1;
        accDamage = 0;
        attackParticle.Play();
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