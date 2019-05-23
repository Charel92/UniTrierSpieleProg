using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Messenger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    List<Message> messages=new List<Message>();
    public GameObject chatpanel, textObject;
    public InputField textField;

    void Start()
    {
        
    }

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
                sendNewMessage("Spieler 1", textField.text);
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

    void sendNewMessage(string name, string text)
    {
        Message msg = new Message(name,text);
        //TO-DO
        printMessage(msg);

    }
    void printMessage(Message msg)
    {
        if (messages.Count > 25)
        {
            Destroy(messages[0].textObject.gameObject);
            messages.Remove(messages[0]);
        }
        GameObject newText = Instantiate(textObject, chatpanel.transform);
        msg.textObject = newText.GetComponent<Text>();
        msg.textObject.text = msg.name+": "+msg.text;
        messages.Add(msg);
    }
}
[System.Serializable]
public class Message
{
    public string name;
    public string text;
    public Text textObject;
    public Message(string name, string text){
        this.name = name;
        this.text = text;
    }


}
