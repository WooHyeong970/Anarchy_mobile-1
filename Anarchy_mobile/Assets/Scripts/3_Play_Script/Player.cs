using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

 [Serializable]
public class Player
{
    [SerializeField]
    private Transform cam_start_point;

    [SerializeField]
    private Transform[] building_area = new Transform[3];

    [SerializeField]
    private Transform[] unit_area = new Transform[3];

    [SerializeField]
    private int quaternion;

    [SerializeField]
    LayerMask layer;

    bool[] is_exists = new bool[3];
    

    public Transform getCamPoint()
    {
        return cam_start_point;
    }

    public Transform getBuilingArea(int type)
    {
        return building_area[type];
    }

    public Transform getUnitArea(int num)
    {
        return unit_area[num];
    }

    public int getQuaternioin()
    {
        return quaternion;
    }

    public bool getIsExist(int num)
    {
        return is_exists[num];
    }

    public void setIsExist(int num, bool check)
    {
        is_exists[num] = check;
    }

    public LayerMask getLayer()
    {
        return layer;
    }
}
