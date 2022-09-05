using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

 [Serializable]
public class Player
{
    [SerializeField]
    private Tile coreTile;

    [SerializeField]
    private Transform cam_start_point;

    [SerializeField]
    private Transform[] building_area = new Transform[3];

    [SerializeField]
    private Transform[] unit_area = new Transform[3];

    [SerializeField]
    private int quaternion;

    [SerializeField]
    private int layer;

    [SerializeField]
    private Color color;

    bool[] is_exists = new bool[3];

    public Tile GetCoreTile()
    {
        return coreTile;
    }

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

    public bool IsUnitExist(int num)
    {
        return is_exists[num];
    }

    public void SetIsUnitExist(int num, bool check)
    {
        is_exists[num] = check;
    }

    public LayerMask GetLayer()
    {
        return layer;
    }

    public Color GetPlayerColor()
    {
        return color;
    }
}
