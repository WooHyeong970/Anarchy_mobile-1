using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DecisionIcon : MonoBehaviour, IPointerClickHandler
{
    public bool isP1Decision = false;
    public bool isP2Decision = false;
    public Tile thisTile;
    public int layer;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(CentralProcessor.Instance.currentTile != thisTile)
        {
            return;
        }

        if(CentralProcessor.Instance.UI.state != UIManager.State.Idle)
        {
            return;
        }

        //if((isP1Decision && !CentralProcessor.Instance.UI) || (isP2Decision && CentralProcessor.Instance.isMaster))
        //{
        //    return;
        //}

        VariableManager.Instance.GetComponent<Decision>().ArouseDecision();
        this.gameObject.SetActive(false);
    }
}
