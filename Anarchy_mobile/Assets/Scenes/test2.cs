using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class test2 : MonoBehaviour
{
    private void Start()
    {
        //StartCoroutine(TestCo());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TestFunc();
    }

    IEnumerator TestCo()
    {
        int num = 0;
        while (true)
        {
            Debug.Log(num);
            num++;
            if (num > 3)
                break;
        }
        yield return null;
        Debug.Log("End Coroutine");
    }

    void TestFunc()
    {
        //Debug.Log("Function Call");
        //Debug.Log(test.Instance.layerMask.);
        //Debug.Log(this.gameObject.layer.ToString());
        //if (test.Instance.layerMask == this.gameObject.layer)
            //Debug.Log("is Same Layer");
    }
}
