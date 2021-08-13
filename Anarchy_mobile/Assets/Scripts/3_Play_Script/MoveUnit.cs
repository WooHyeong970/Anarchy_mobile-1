using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveUnit : MonoBehaviour
{
    public Tile pairTile;
    public int cost;
    bool isMaster;
    Transform[] area;
    bool[] isEmpty;
    public bool isChecked = false;

    private void Start()
    {
        isMaster = CentralProcessor.Instance.isMaster;
    }

    public void Move()
    {
        if(!isChecked)
        {
            isChecked = true;
            ChangeColor(Color.blue);
            if(CentralProcessor.Instance.current_moveButton != null)
            {
                CentralProcessor.Instance.current_moveButton.GetComponent<MoveUnit>().ChangeColor(Color.white);
                CentralProcessor.Instance.current_moveButton.GetComponent<MoveUnit>().isChecked = false;
                CentralProcessor.Instance.current_moveButton = this.gameObject.GetComponent<Button>();
            }
            else
            {
                CentralProcessor.Instance.current_moveButton = this.gameObject.GetComponent<Button>();
            }
        }
        else
        {
            CentralProcessor.Instance.current_moveButton = null;
            OffCheck();
        }
    }

    public void OffCheck()
    {
        isChecked = false;
        ChangeColor(Color.white);
    }

    public void ChangeColor(Color c)
    {
        Image tmp = this.gameObject.GetComponent<Image>();
        tmp.color = c;
    }
}
