using UnityEngine;

public class eyeLook : MonoBehaviour
{

    public Vector2 mouseDirection;
    public Transform player;


    //see https://www.youtube.com/watch?v=bUa9qphRLGA&t=990s
    private void Start()
    {
        player = this.transform.parent.transform;
    }
    // Update is called once per frame
    void Update()
    {
        //How much has the mouse moved?
         Vector2 mouseChange = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

         mouseDirection += mouseChange;

        //AngleAxis(float angle, Vector3 axis);
        this.transform.localRotation = Quaternion.AngleAxis(-mouseDirection.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(mouseDirection.x, Vector3.up);


    }

  /*
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    // Update is called once per frame
    void Update()
    {

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

    }

    */
}
