using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public static test Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<test>();

            return instance;
        }
    }
    private static test instance;

    public Text t;

    public int num;

    public LayerMask layerMask;

    private void Start()
    {
        Debug.Log(layerMask);
    }
}
