using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class test2 : MonoBehaviour
{
    test t = test.Instance;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            t.num += 1;
        }
    }
}
