using UnityEngine.UI;
using System.Collections.Generic;

public class MessageObject
{
    // Start is called before the first frame update
    public string type;
    public string name;
    public string receiver;
    public string sender;
    public string content;
    public bool world = false;
    public string position;
    private Text textObject;
    public Dictionary<string,string> positions;

    //general


    //send message
    public MessageObject(string receiver, string content)
    {
        type = "chat";
        this.receiver = receiver;
        this.content = content;
    }

    //hello
    public MessageObject(string name)
    {
        type = "hello";
        this.name = name;
    }
    public MessageObject(MessageType messageType)
    {

        switch (messageType)
        {
            case MessageType.Chat:
                type = "hello";
                break;
            case MessageType.Hello:
                type = "hello";
                break;

            case MessageType.State:
                type = "getstate";
                break;
            default:
                
                break;
        }

    }


    public Text getTextObject() {
        return textObject;
    }

    public void setTextObject(Text textObject)
    {
        this.textObject = textObject;
    }



    public override string ToString()
    {
        return "type: " + type + " name: " + name + " receiver: " + receiver + " sender: " + sender + " content: " + content + " pos: " + position  + " world: " + world;
    }
}
public enum MessageType { Hello, Chat, State };
