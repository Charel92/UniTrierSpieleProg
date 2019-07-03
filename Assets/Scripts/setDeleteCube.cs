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
    public GameObject controllers;


    public List<GameObject> getCubes() {
        return cubes;
    }

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
        return -1;
    }

    


    private void setAndCreateCubeToPosition(Vector3 pos,int matIndex, string owner)
    {
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        if (owner.Equals(Globals.name))cube.layer = 8;
        cube.transform.localScale = new Vector3(1, 1, 1);
        cube.transform.position = pos;
        cubeRenderer = cube.gameObject.GetComponent<Renderer>();
        cubeRenderer.sharedMaterial = materials[matIndex];
        cubes.Add(cube);
    }

    void Update()
    {
        if (!textField.isFocused)
        {

            //for tests
            /*if (Input.GetKeyDown("3"))
            {
                getCubesRendererMatIndex();
            }*/

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
                    cube.name = "playersCube Nr." + cubes.Count;

                    cubes.Add(cube);
                    controllers.GetComponent<ServerController>().SendMessage(
                        new AddCubeFromClientMessage(Globals.PositionToString(cube.transform.position),Globals.name,activeMaterial.ToString()));
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
                            controllers.GetComponent<ServerController>().SendMessage(
                        new RemoveCubeFromClientMessage(Globals.PositionToString(col.gameObject.transform.position), Globals.name));
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
            if (cd.materialIndex != -1) //cube has no texture
            {
                cubeRenderer = go.GetComponent<Renderer>();
                cubeRenderer.sharedMaterial = materials[cd.materialIndex];
            }
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


    public void createAndSetSpheresToPosition(Dictionary<string, string> positions)
    {
        GameObject sphere;
        foreach (KeyValuePair<string, string> pos in positions)
        {
            if (!pos.Key.Equals(Globals.name))
            {
                string[] playercoordinates = Globals.SplitPositionString(pos.Value);
                sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = new Vector3(Int32.Parse(playercoordinates[0]), Int32.Parse(playercoordinates[1]), Int32.Parse(playercoordinates[2]));
                hitColliders = Physics.OverlapSphere(sphere.transform.position, 0.8f);
                if (hitColliders.Length != 0)
                {
                    Destroy(sphere);
                }
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

    //send the position,name and matIndex of the cubes in the game to the server
    public void sendStateOfCubesToServer()
    {
        deleteNullRefs();
        StateOfCubesFromClientMessage sendCubeMessage = new StateOfCubesFromClientMessage();
        sendCubeMessage.sender = Globals.name;
        foreach (GameObject cube in cubes){
            //created by player
            if (cube.layer == 8) sendCubeMessage.addCube(cube, getMaterialIndex(cube));
        }
        controllers.GetComponent<ServerController>().SendMessage(sendCubeMessage);
    }


    //show the cubes with the positions received from the server
    //textures were not saved
    public void loadCubesFromPositions(List<ReceivedCubeFromServer> cubes)
    {
        deleteAllCubes();

        foreach (ReceivedCubeFromServer cube in cubes)
        {
            string[] cubecoordinates = Globals.SplitPositionString(cube.position);
            Vector3 vectorPos = new Vector3(Int32.Parse(cubecoordinates[0]), Int32.Parse(cubecoordinates[1]), Int32.Parse(cubecoordinates[2]));
            
            setAndCreateCubeToPosition(vectorPos,cube.matIndex,cube.owner);
        }
    }

    public void createCube(ReceivedCubeFromServer cubeFromServer)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "cube from " + cubeFromServer.owner;
        Renderer cubeRenderer = cube.gameObject.GetComponent<Renderer>();
        cube.transform.position = Globals.SplitPositionStringToVector(cubeFromServer.position);
        cubeRenderer.sharedMaterial = materials[cubeFromServer.matIndex];
        cube.transform.localScale = new Vector3(1, 1, 1);
        //cubes.Add(cube);
    }

    public void deleteCube(ReceivedCubeFromServer cubeFromServer)
    {
        hitVector =Globals.SplitPositionStringToVector(cubeFromServer.position);
        hitColliders = Physics.OverlapBox(hitVector, new Vector3(0.4f, 0.4f, 0.4f));
        if (hitColliders.Length > 0)
        {
            foreach (Collider col in hitColliders) Destroy(col.gameObject);
        }
    }
}
