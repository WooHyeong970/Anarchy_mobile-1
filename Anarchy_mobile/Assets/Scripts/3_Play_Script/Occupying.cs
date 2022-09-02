using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Occupying : MonoBehaviour
{
    int num = 0;
    IEnumerator t;

    private void Start()
    {
        t = testCo();
        StartCoroutine(t);
    }

    private void Update()
    {
        if (num > 10)
        {

            StopCoroutine(t);
            
        }
    }

    IEnumerator testCo()
    {
        Debug.Log("start");
        while(true)
        {
            yield return null;
            Debug.Log(num);
            num++;
            if (num > 10)
                Debug.Log("IS BIG");
        }
    }
}
