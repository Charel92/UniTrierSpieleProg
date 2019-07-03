using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

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
    public Dictionary<string, string> cubesPositions; //position
    public Dictionary<string, int> cubesMatIndexes; //MatIndex

    public StateOfCubesFromClientMessage()
    {
        type = "stateofcubes";
        cubesPositions = new Dictionary<string, string>();
        cubesMatIndexes = new Dictionary<string, int>();
    }

    public void addCube(GameObject cubePos, int cubeMatIndex)
    {
        string pos = Globals.PositionToString(cubePos.transform.position);
        cubesPositions.Add(cubesPositions.Count.ToString(), pos);
        cubesMatIndexes.Add(cubesMatIndexes.Count.ToString(), cubeMatIndex);
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
    public List<ReceivedCubeFromServer> cubes; //position,mat

    public StateOfCubesFromServerMessage(List<ReceivedCubeFromServer> cubes)
    {
        type = "stateofcubes";
        this.cubes = cubes;
    }
}

public class AddCubeFromClientMessage : MessageObject
{
    public string matIndex;

    public AddCubeFromClientMessage(string position, string sender, string matIndex)
    {
        type = "addCube";
        this.position = position;
        this.sender = sender;
        this.matIndex = matIndex;
    }
}

public class AddCubeFromServerMessage : MessageObject
{
    public ReceivedCubeFromServer cube;

    public AddCubeFromServerMessage(string position, string matIndex, string owner)
    {
        type = "addCube";
        cube = new ReceivedCubeFromServer(position, Int32.Parse(matIndex),owner);
    }
}

public class RemoveCubeFromClientMessage : MessageObject
{

    public RemoveCubeFromClientMessage(string position, string sender)
    {
        type = "removeCube";
        this.position = position;
        this.sender = sender;
    }
}

public class RemoveCubeFromServerMessage : MessageObject
{
    public ReceivedCubeFromServer cube;

    public RemoveCubeFromServerMessage(string position)
    {
        type = "removeCube";
        cube = new ReceivedCubeFromServer(position);
    }
}


public class ReceivedCubeFromServer
{
    public string position;
    public string owner;
    public int matIndex;

    [JsonConstructor]
    public ReceivedCubeFromServer(string position, int matIndex, string owner)
    {
        this.position = position;
        this.matIndex = matIndex;
        this.owner = owner;
    }

    public ReceivedCubeFromServer(string position, int matIndex)
    {
        this.position = position;
        this.matIndex = matIndex;
    }

    public ReceivedCubeFromServer(string position)
    {
        this.position = position;
    }


    public override string ToString()
    {
        return "position: " + position + " owner: " + owner + " matIndex: " + matIndex;
    }
}




