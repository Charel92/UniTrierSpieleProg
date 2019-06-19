using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    public Camera cam;
    public float speed = 5f;
    public InputField textField;
    //public float jumpSpeed = 50f;
    //public BoxCollider col;
    //public LayerMask groundLayers;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!textField.isFocused)
        {
            if (Input.GetKey("w"))
            {
                //transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
                transform.position = transform.position + cam.transform.forward * speed * Time.deltaTime;
            }

            if (Input.GetKey("s"))
            {
                transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
            }

            if (Input.GetKey("a"))
            {
                transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
            }

            if (Input.GetKey("d"))
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }

            if (Input.GetKey("space"))
            {
                transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
            }
        }
    }
}
