using UnityEngine;


public class PositionOfGridCube : MonoBehaviour
{

    public Transform gridCube;
    public Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gridCube.position = new Vector3((int)(transform.position.x + cam.transform.forward.x * 2), (int)(transform.position.y + cam.transform.forward.y * 3), (int)(transform.position.z + cam.transform.forward.z * 2));
    }
}
