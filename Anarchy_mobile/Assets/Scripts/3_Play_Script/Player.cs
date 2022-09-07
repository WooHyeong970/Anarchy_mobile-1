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
    private Transform camStartPoint;

    [SerializeField]
    private Transform[] buildingArea = new Transform[3];

    [SerializeField]
    private Transform[] unitArea = new Transform[3];

    [SerializeField]
    private int quaternion;

    [SerializeField]
    private int layer;

    [SerializeField]
    private Color color;

    public Tile GetCoreTile()
    {
        return coreTile;
    }

    public Transform GetCamPoint()
    {
        return camStartPoint;
    }

    public Transform GetBuilingArea(int type)
    {
        return buildingArea[type];
    }

    public Transform GetUnitArea(int num)
    {
        return unitArea[num];
    }

    public int GetQuaternioin()
    {
        return quaternion;
    }

    public int GetLayer()
    {
        return layer;
    }

    public Color GetPlayerColor()
    {
        return color;
    }
}
