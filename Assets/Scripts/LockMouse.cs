using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class LockMouse : MonoBehaviour
{

    public InputField textField;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update()
    {

        if (!textField.isFocused)
        {

            if (Input.GetKeyDown(KeyCode.M))
            {
                if (Cursor.visible)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
            }

        }

    }



}
