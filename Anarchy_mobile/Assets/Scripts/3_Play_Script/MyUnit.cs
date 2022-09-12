using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnarchyUtility;

public class MyUnit : MonoBehaviourPun
{
    [SerializeField]
    private int         type;
    [SerializeField]
    private string      unitName;
    public int          cost;
    public int          hp;
    public int          activeCost;
    public int          offensive;
    public int          defensive;

    public Sprite       illust;
    
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
        UT.SetManager(ref UI, ref CP);

        if (isClicked)
        {
            CloseInfo();
            return;
        }

        isClicked = true;
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
        // 클릭한 유닛이 아군 유닛이면 ButtonPanel도 함께 true
        if (this.gameObject.layer == CP.GetPlayer().GetLayer())
        {
            // 이미 클릭된 유닛이 있다면 먼저 초기화 실행
            if (CP.currentUnit != null)
                CP.currentUnit.CloseInfo();
            StartParticle();
            UT.SetActive(UI.unitButtonPanel, true);
            CP.currentUnit = this.gameObject.GetComponent<MyUnit>();
        }

        // 클릭한 유닛이 적군 유닛이면 InfoPanel만 true
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
        UT.SetActive(particle, true);
        particle.Play();
    }

    public void GetAttack()
    {
        hp -= 1;
        accDamage = 0;
        attackParticle.Play();
    }

    public int GetUnitType()
    {
        return type;
    }

    public string GetUnitName()
    {
        return name;
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