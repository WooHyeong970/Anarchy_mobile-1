using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class test2 : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(TestCo());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            StartCoroutine(TestCo());
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
}
