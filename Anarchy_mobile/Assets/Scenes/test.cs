using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int num;
}
