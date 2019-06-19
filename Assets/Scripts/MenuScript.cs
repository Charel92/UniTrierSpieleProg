using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

    private bool isPaused = false;
    public InputField textField;
    public Image pauseMenu;
    public Button[] menuContent;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.enabled = isPaused;
        for (int i = 0; i < menuContent.Length; i++)
        {
            menuContent[i].gameObject.SetActive(isPaused);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!textField.isFocused)
        {
            if (Input.GetKeyDown("p"))
            {
                if (!isPaused)
                {
                    openMenu();

                }
                else {
                    closeMenu();
                }
                
            }
        }
    }



    void openMenu()
    {
        isPaused = true;
        pauseMenu.enabled = isPaused;
        for (int i = 0;i<menuContent.Length ;i++)
        {
            menuContent[i].gameObject.SetActive (isPaused);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
    void closeMenu()
    {
        isPaused = false;
        pauseMenu.enabled = false;
        for (int i = 0; i < menuContent.Length; i++)
        {
            menuContent[i].gameObject.SetActive(isPaused);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
