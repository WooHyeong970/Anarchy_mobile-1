using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveUnit : MonoBehaviour
{
    public Tile pairTile;
    public int cost;
    bool isMaster;
    public bool isChecked = false;
    public Image checkPoint;
    public bool isMove = false;
    public Image[] p1unit;
    public Image[] p2unit;

    //private void Start()
    //{
    //    isMaster = CentralProcessor.Instance.isMaster;
    //}

    public void Move()
    {
        CentralProcessor.Instance.targetTile = pairTile;
        if(!isMove)
        {
            return;
        }

        if(!isChecked)
        {
            isChecked = true;
            ChangeColor(Color.blue);
            if(CentralProcessor.Instance.UI.currentMoveButton != null)
            {
                CentralProcessor.Instance.UI.currentMoveButton.GetComponent<MoveUnit>().ChangeColor(Color.black);
                CentralProcessor.Instance.UI.currentMoveButton.GetComponent<MoveUnit>().isChecked = false;
                CentralProcessor.Instance.UI.currentMoveButton = this.gameObject.GetComponent<Button>();
            }
            else
            {
                CentralProcessor.Instance.UI.currentMoveButton = this.gameObject.GetComponent<Button>();
            }
        }
        else
        {
            CentralProcessor.Instance.UI.currentMoveButton = null;
            OffCheck();
        }
    }

    public void OffCheck()
    {
        isChecked = false;
        ChangeColor(Color.black);
    }

    public void ChangeColor(Color c)
    {
        Image tmp = this.gameObject.GetComponent<Image>();
        tmp.color = c;
    }

    public void MoveCamera()
    {
        pairTile.MoveTile();
        CentralProcessor.Instance.UI.SetIdleState();
    }
}
