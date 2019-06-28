using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class MessageObject
{
    public string type;
    public string position ="";
    public string receiver ="";
    public string sender ="";
    public string content ="";
    public bool world = false;
    private Text textObject;

    public MessageObject()
    {

    }

    //send message



    public MessageObject(MessageType messageType)
    {

        switch (messageType)
        {

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
        return "type: " + type;
    }
}

public enum MessageType { Hello, Chat, State };

public class HelloMessage : MessageObject
{
    public string name;


    public HelloMessage(string name)
    {
        type = "hello";
        this.name = name;
    }

    public override string ToString()
    {
        return base.ToString() + " name: " + name;
    }

}

public class WelcomeMessage : MessageObject
{
    public WelcomeMessage(string position)
    {
        type = "welcome";
        this.position = position;
    }

    public WelcomeMessage(MessageObject msg)
    {
        type = "welcome";
        position = msg.position;

    }

    public override string ToString()
    {
        return base.ToString()+ " pos: " + position;
    }

}

public class ChatMessage : MessageObject
{

    public ChatMessage(string receiver, string sender, string content)
    {
        type = "chat";
        this.receiver = receiver;
        this.content = content;
        this.sender = sender;
    }

    public ChatMessage(MessageObject msg)
    {
        type = "chat";
        sender = msg.sender;
        content = msg.content;
        world = msg.world;
    }

    public override string ToString()
    {
        return base.ToString() + " receiver: " + receiver + " sender: " + sender + " content: " + content + " world: " + world;
    }



}


public class GetStateMessage : MessageObject
{
    public GetStateMessage()
    {
        type = "getstate";

    }
}

public class StateMessage : MessageObject
{

    public List<string> playerList = new List<string>();
    public Dictionary<string, string> positions;

    public StateMessage(Dictionary<string, string> positions)
    {
        type = "state";
        this.positions = positions;
        updatePlayersInPlayerList();
    }

    private void updatePlayersInPlayerList()
    {
        
        if (playerList != null && playerList.Count > 0) playerList.Clear();
        foreach (KeyValuePair<string, string> pos in positions)
        {
            playerList.Add(pos.Key);
        }
    }
}

public class StateOfCubesFromClientMessage : MessageObject
{
    public Dictionary<string, string> cubes; //position,mat

    public StateOfCubesFromClientMessage()
    {
        type = "stateofcubes";
        cubes = new Dictionary<string, string>();
    }

    public void addCube(GameObject cube)
    {
        string pos = cube.transform.position.x + "," + cube.transform.position.y + "," + cube.transform.position.z;
        cubes.Add(cubes.Count.ToString(), pos);
    }
}

public class GetStateOfCubesMessage : MessageObject
{
    public Dictionary<string, string> cubes;

    public GetStateOfCubesMessage()
    {
        type = "getstateofcubes";
    }
}

public class StateOfCubesFromServerMessage : MessageObject
{
    public Dictionary<string, string> cubes; //position,mat

    public StateOfCubesFromServerMessage(Dictionary<string, string> cubes)
    {
        type = "stateofcubes";
        this.cubes = cubes;
    }

    public void addCube(GameObject cube)
    {
        string pos = cube.transform.position.x + "," + cube.transform.position.y + "," + cube.transform.position.z;
        cubes.Add(cubes.Count.ToString(), pos);
    }
}


