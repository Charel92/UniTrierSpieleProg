using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setDeleteCube : MonoBehaviour
{

    private List<GameObject> cubes = new List<GameObject>();
    private GameObject cube;
    private float yAngleCamera;
    public Camera cam;
    private Collider[] hitColliders;
    private Vector3 hitVector;
    private static Renderer cubeRenderer;
    public Material[] materials;
    private int activeMaterial;
    public Button[] buttons;
    public InputField textField;


    public List<GameObject> getCubes() {
        return cubes;
    }


    // Start is called before the first frame update
    void Start()
    {
        activeMaterial = 0;
        buttons[activeMaterial].GetComponent<Image>().color = Color.green;

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

                



                //Debug.Log(cam.transform.forward);
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
                buttons[activeMaterial].GetComponent<Image>().color = Color.green;
            }

            if (Input.GetKeyDown("2"))
            {
                buttons[activeMaterial].GetComponent<Image>().color = Color.white;
                activeMaterial = (activeMaterial + 1) % materials.Length;
                buttons[activeMaterial].GetComponent<Image>().color = Color.green;
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
        buttons[activeMaterial].GetComponent<Image>().color = Color.green;
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

}
