﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SaveData
{
    public List<CubeData> cubesData;

}

[Serializable]
public struct CubeData
{
    public string id;
    public string name;
    public int materialIndex;
    public float posX;
    public float posY;
    public float posZ;

    public CubeData (GameObject cube)
    {
        id = cube.name;
        name = cube.name;
        materialIndex = Int32.Parse(cube.name[cube.name.Length - 1].ToString());
        posX = cube.transform.position.x;
        posY = cube.transform.position.y;
        posZ = cube.transform.position.z;
    }

    public GameObject toGameObject()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(posX,posY,posZ);
        cube.layer = 8;
        cube.name = name;
        return cube;
    }

    public override string ToString()
    {
        return "id: " + id +" material: " + materialIndex + " pos:(" + posX+","+posY+","+posZ+")";
    }
}