using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setDeleteCube : MonoBehaviour
{

    List<GameObject> cubes = new List<GameObject>();
    private GameObject cube;
    private float yAngleCamera;
    public Camera cam;
    private Collider[] hitColliders;
    private Vector3 hitVector;
    private static Renderer renderer;
    public Material[] materials;
    private int activeMaterial;
    public Button[] buttons;
    

    // Start is called before the first frame update
    void Start()
    {
        activeMaterial = 0;
        buttons[activeMaterial].GetComponent<Image>().color = Color.green;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("q"))
        {



            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            renderer = cube.gameObject.GetComponent<Renderer>();
            renderer.sharedMaterial = materials[activeMaterial];
            

            //Debug.Log(cam.transform.forward);
            cube.transform.position = new Vector3((int)(transform.position.x + cam.transform.forward.x * 2), (int)(transform.position.y + cam.transform.forward.y * 3), (int)(transform.position.z + cam.transform.forward.z * 2));
            cube.transform.localScale = new Vector3(1, 1, 1);
            hitColliders = Physics.OverlapBox(cube.transform.position, new Vector3(0.4f, 0.4f, 0.4f));
            if (hitColliders.Length == 0)
            {
                cube.layer = 8;
                cube.name = "playersCube Nr." + cubes.Count;

                cubes.Add(cube);
                Debug.Log("set cube with position: " + cube.transform.position);
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
                        //deleteCube(col.gameObject);
                        Destroy(col.gameObject);
                    }

                }

            }

        }


        if (Input.GetKeyDown("y"))
        {
            listAllCubes();
        }
        if (Input.GetKeyDown("1"))
        {
            buttons[activeMaterial].GetComponent<Image>().color = Color.white;
            activeMaterial = (activeMaterial - 1) % materials.Length;
            buttons[activeMaterial].GetComponent<Image>().color = Color.green;
        }
        if (Input.GetKeyDown("2"))
        {
            buttons[activeMaterial].GetComponent<Image>().color = Color.white;
            activeMaterial = (activeMaterial + 1) % materials.Length;
            buttons[activeMaterial].GetComponent<Image>().color = Color.green;
        }

    }
        private void deleteNullRefs()
    {
        for (int i = 0; i < cubes.Count; i++) {
            if (cubes[i] == null)
            {
                cubes.RemoveAt(i);
            }
        }
    }

    private void listAllCubes()
    {
        Debug.Log("Cube length: " + cubes.Count);
        for (int i = 0; i < cubes.Count; i++)
        {
                if (cubes[i] != null) Debug.Log("cube with index " + i +" : " + cubes[i].name);
                else Debug.Log("cube with index " + i + " : null");
        }
    }

}
