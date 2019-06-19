using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataController : MonoBehaviour
{

    public GameObject player;
    List<GameObject> cubes;
    public SaveData saveData;



    public void setUpToSave() {
        player.GetComponent<setDeleteCube>().deleteNullRefs();
        cubes = player.GetComponent<setDeleteCube>().getCubes();
        //listAllCubes();
        //Debug.Log("-----------------------");
        convertForSerialization();
        SaveGame();

    }

    public void setUpToLoad()
    {
        LoadData();
        player.GetComponent<setDeleteCube>().createAllCubesFromSaveData(saveData);
        //listSaveData();
    }
    

    [ContextMenu("Save Data")]
    public void SaveGame()
    {
        //listSaveData();
        //Debug.Log("-----------------------");
        var data = JsonUtility.ToJson(saveData);
        //Debug.Log("saved data: " + data);
        PlayerPrefs.SetString("GameData", data);
    }

    [ContextMenu("Load Data")]
    public void LoadData()
    {
        var data = PlayerPrefs.GetString("GameData");
        //Debug.Log("loaded data: " + data);
        saveData = JsonUtility.FromJson<SaveData>(data);
    }

    private void listAllCubes()
    {
        Debug.Log("Cube length: " + cubes.Count);
        for (int i = 0; i < cubes.Count; i++)
        {
            if (cubes[i] != null) Debug.Log("cube with index " + i + " : " + cubes[i].name);
            else Debug.Log("cube with index " + i + " : null");
        }
    }

    private void listSaveData()
    {
        Debug.Log("Cube length: " + saveData.cubesData.Count);
        for (int i = 0; i < saveData.cubesData.Count; i++)
        {
             Debug.Log("cube with index " + i + " : " + saveData.cubesData[i]);
        }
    }

    private void convertForSerialization()
    {
        saveData.cubesData = new List<CubeData>();
        foreach (GameObject gm in cubes)
        {
            saveData.cubesData.Add(new CubeData(gm));
        }    
    }

}
