using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnarchyUtility;

public class MyBuilding : MonoBehaviour
{
    public int level;
    [TextArea]
    public string desc;
    public string buildingName;
    public Sprite illust;
    
    bool isClicked;
    Utility UT = new Utility();
    UIManager UI;
    CentralProcessor CP;

    public void OnClick()
    {
        UT.SetManager(ref UI, ref CP);
        if (CP.currentUnit != null)
            CP.currentUnit.CloseInfo();

        if(isClicked)
        {
            isClicked = false;
            CloseInfo();
            return;
        }

        if(CP.currentBuilding != null)
            CP.currentBuilding.CloseInfo();

        CP.currentBuilding = this.gameObject.GetComponent<MyBuilding>();
        UI.ShowBuildingInfo(this.gameObject.GetComponent<MyBuilding>());
    }

    public void CloseInfo()
    {
        isClicked = false;
        UI.CloseBuildingInfo();
    }
}
