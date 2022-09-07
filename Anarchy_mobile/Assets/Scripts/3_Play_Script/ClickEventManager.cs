using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickEventManager : MonoBehaviour
{
    [SerializeField]
    bool check;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            string tagName = IsPointerOverUIObject();
            if (tagName == "UI" || tagName == "Window")
            {
                CentralProcessor.Instance.UI.OffInOf();
                return;
            }

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(tagName == "Unit")
                {
                    hit.transform.GetComponent<MyUnit>().OnClick();
                }
                else if(tagName == "Building")
                {
                    hit.transform.GetComponent<MyBuilding>().OnClick();
                }
                else if(tagName == "Tile")
                {
                    //hit.transform.GetComponent<Tile>().OnClick();
                }
            }
        }
    }

    private string IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        Debug.Log("ClickEventManager/results[0] : " + results[0].gameObject.tag);
        return results[0].gameObject.tag;
    }
}