using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

 [Serializable]
public class Player //: MonoBehaviour
{
    [SerializeField]
    private Transform cam_start_point;

    [SerializeField]
    private Transform[] building_area = new Transform[3];

    [SerializeField]
    private int quaternion;

    public Transform getCamPoint()
    {
        return cam_start_point;
    }

    public Transform getBuilingArea(int type)
    {
        return building_area[type];
    }

    public int getQuaternioin()
    {
        return quaternion;
    }
}
