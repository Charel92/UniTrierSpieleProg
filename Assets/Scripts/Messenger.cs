using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Messenger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject controllers;
    List<MessageObject> messages=new List<MessageObject>();
    public GameObject chatpanel, textObject;
    public InputField textField;
    public Dropdown dropdown;
    public MessageObject messageObject;


    // Update is called once per frame
    void Update()
    {
        if (!textField.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
            sendNewMessage("BOT", "Dies ist eine Testnachricht:"+System.DateTime.Today.ToString("F"));
            }
        }


        if (textField.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                sendNewMessage(dropdown.options[dropdown.value].text, textField.text);
                textField.text = "";
            }
        }
        else
        {
            if (!textField.isFocused && Input.GetKeyDown(KeyCode.Return))
            {
                textField.ActivateInputField();
            }
        }
        
    }

    void sendNewMessage(string to, string content)
    {
        messageObject = new MessageObject(to, content);
        controllers.GetComponent<ServerController>().SendMessage(messageObject);
        printMessage(messageObject);

    }
    public void printMessage(MessageObject msg)
    {
        if (messages.Count > 25)
        {
            Destroy(messages[0].getTextObject().gameObject);
            messages.Remove(messages[0]);
        }
        GameObject newText = Instantiate(textObject, chatpanel.transform);
        msg.setTextObject(newText.GetComponent<Text>());

        if (msg.sender != null && !msg.sender.Equals(""))
        {
            msg.getTextObject().text = msg.sender + "-> self:"+ msg.content;
        } else
        {
            if (msg.receiver != null && !msg.receiver.Equals(""))
            {
                msg.getTextObject().text = "self -> " + msg.receiver + ": " + msg.content;
            } else
            {
                Debug.Log("Printed Message has no receiver and no sender");
            }

        }

        messages.Add(msg);
    }

    
}



