using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setDeleteCube : MonoBehaviour
{

    private List<GameObject> cubes = new List<GameObject>();
    private GameObject cube;
    public Camera cam;
    private Collider[] hitColliders;
    private Vector3 hitVector;
    private static Renderer cubeRenderer;
    public Material[] materials;
    private int activeMaterial;
    public Button[] buttons;
    public InputField textField;
    public GameObject player;
    public Color selectedMatColor = new Color(0.04797081f, 0.9081972f, 0.9245283f,1f);


    public List<GameObject> getCubes() {
        return cubes;
    }


    // Start is called before the first frame update
    void Start()
    {
        activeMaterial = 0;
        buttons[activeMaterial].GetComponent<Image>().color = selectedMatColor;

    }

    public int getMaterialIndex(GameObject cube) {
        cubeRenderer = cube.gameObject.GetComponent<Renderer>();
        for (int i = 0; i < materials.Length; i++)
        {
            if (cubeRenderer.sharedMaterial.Equals(materials[i])) return i;
        }
        return 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!textField.isFocused)
        {
            if (Input.GetKeyDown("q"))
            {
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cubeRenderer = cube.gameObject.GetComponent<Renderer>();
                cubeRenderer.sharedMaterial = materials[activeMaterial];

                



                cube.transform.position = new Vector3((int)(transform.position.x + cam.transform.forward.x * 2), (int)(transform.position.y + cam.transform.forward.y * 3), (int)(transform.position.z + cam.transform.forward.z * 2));
                cube.transform.localScale = new Vector3(1, 1, 1);
                hitColliders = Physics.OverlapBox(cube.transform.position, new Vector3(0.4f, 0.4f, 0.4f));
                if (hitColliders.Length == 0)
                {
                    cube.layer = 8;
                    cube.name = "playersCube Nr." + cubes.Count + ":"+activeMaterial;

                    cubes.Add(cube);
                    //Debug.Log("set cube with position: " + cube.transform.position);
                }
                else
                {
                    Destroy(cube);
                }


                /*if (hitCollider.Length != 0)
                {
                    foreach (Collider col in hitCollider)
                    {
                        Debug.Log("hit with Object: " + col.gameObject.name);
                    }
                
                }*/
            }


            if (Input.GetKeyDown("e"))
            {
                hitVector = new Vector3((int)(transform.position.x + cam.transform.forward.x * 2), (int)(transform.position.y + cam.transform.forward.y * 3), (int)(transform.position.z + cam.transform.forward.z * 2));
                hitColliders = Physics.OverlapBox(hitVector, new Vector3(0.4f, 0.4f, 0.4f));
                if (hitColliders.Length > 0)
                {

                    foreach (Collider col in hitColliders)
                    {
                        if (col.gameObject.layer == 8)
                        {
                            Destroy(col.gameObject);
                        }

                    }

                }

            }

            if (Input.GetKeyDown("1"))
            {
                buttons[activeMaterial].GetComponent<Image>().color = Color.white;
                activeMaterial = mod((activeMaterial - 1), materials.Length);
                buttons[activeMaterial].GetComponent<Image>().color = selectedMatColor;
            }

            if (Input.GetKeyDown("2"))
            {
                buttons[activeMaterial].GetComponent<Image>().color = Color.white;
                activeMaterial = (activeMaterial + 1) % materials.Length;
                buttons[activeMaterial].GetComponent<Image>().color = selectedMatColor;
            }

        }
    }
    //https://stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain
    private int mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }

    public void deleteNullRefs()
    {
        for (int i = 0; i < cubes.Count; i++) {
            if (cubes[i] == null)
            {
                cubes.RemoveAt(i);
            }
        }
    }

    public void changeMaterialByClick(int mat)
    {
        
        buttons[activeMaterial].GetComponent<Image>().color = Color.white;
        activeMaterial = mat;
        buttons[activeMaterial].GetComponent<Image>().color = selectedMatColor;
    }


    public void deleteAllCubes()
    {
        foreach (GameObject cube in cubes)
        {
            Destroy(cube);
        }
        cubes.Clear();
    }

    public void createAllCubesFromSaveData(SaveData saveData)
    {
        deleteAllCubes();
        GameObject go;

        foreach (CubeData cd in saveData.cubesData)
        {
            go = cd.toGameObject();
            cubeRenderer = go.GetComponent<Renderer>();
            cubeRenderer.sharedMaterial = materials[cd.materialIndex];
            cubes.Add(go);
        }
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


    public void createAndSetCubesToPosition(Dictionary<string, string> positions)
    {
        GameObject sphere;
        foreach (KeyValuePair<string, string> pos in positions)
        {
            if (!pos.Key.Equals(Globals.name))
            {
                string[] playercoordinates = pos.Value.Split(',');
                sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = new Vector3(Int32.Parse(playercoordinates[0]), Int32.Parse(playercoordinates[1]), Int32.Parse(playercoordinates[2]));
            }

        }
    }
    public void translateAllCubesAndPlayer(int x, int y, int z)
    {
        //Debug.Log("Aendere Position um"+x+y+z);
        player.transform.position = new Vector3(player.transform.position.x+ x,player.transform.position.y + y, player.transform.position.z + z);
        for (int i = 0; i < cubes.Count; i++)
        {
            // cubes[i].transform.position=new Vector3(cubes[i].transform.position.x+x, cubes[i].transform.position.y + y, cubes[i].transform.position.z + z);
            
        }
    }

}
