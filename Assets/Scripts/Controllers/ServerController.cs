﻿
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System.Collections.Generic;

public class ServerController : MonoBehaviour
{
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    private MessageObject receivedMessage;
    private ChatMessage receivedChatMessage;
    private WelcomeMessage receivedWelcomeMessage;
    private StateMessage receivedStateMessage;
    private StateOfCubesFromServerMessage receivedStateofCubesFromServerMessage;
    private AddCubeFromServerMessage receivedAddCubeFromServerMessage;
    private RemoveCubeFromServerMessage receivedRemoveCubeFromServerMessage;
    public GameObject player;


    void Start()
    {
        ConnectToTcpServer();
    }

    void Update()
    {
        if (receivedChatMessage != null)
        {
            player.GetComponent<Messenger>().printMessage(receivedChatMessage);
            receivedChatMessage = null;
        }
        if (receivedWelcomeMessage != null)
        {
            string[] playercoordinates = receivedWelcomeMessage.position.Split(',');
            player.GetComponent<setDeleteCube>().translateAllCubesAndPlayer(Int32.Parse(playercoordinates[0]), Int32.Parse(playercoordinates[1]), Int32.Parse(playercoordinates[2]));
            receivedWelcomeMessage = null;

        }

        if (receivedStateMessage != null)
        {
            player.GetComponent<Messenger>().updatePlayerList(receivedStateMessage.playerList);
            player.GetComponent<setDeleteCube>().createAndSetSpheresToPosition(receivedStateMessage.positions);
            receivedStateMessage = null;
        }

        if (receivedStateofCubesFromServerMessage != null)
        {
            player.GetComponent<setDeleteCube>().loadCubesFromPositions(receivedStateofCubesFromServerMessage.cubes);
            receivedStateofCubesFromServerMessage = null;
        }

        if (receivedAddCubeFromServerMessage != null)
        {
            player.GetComponent<setDeleteCube>().createCube(receivedAddCubeFromServerMessage.cube);
            receivedAddCubeFromServerMessage = null;
        }

        if (receivedRemoveCubeFromServerMessage != null)
        {
            player.GetComponent<setDeleteCube>().deleteCube(receivedRemoveCubeFromServerMessage.cube);
            receivedRemoveCubeFromServerMessage = null;
        }
    }

    /// <summary> 	
    /// Setup socket connection. 	
    /// </summary> 	
    public void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
            Debug.Log("connected to Server");
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }


    private void Disconnect()
    {
        socketConnection.Close();
        Debug.Log("connection closed");
    }
    /// <summary> 	
    /// Runs in background clientReceiveThread; Listens for incomming data. 	
    /// </summary>     
    private void ListenForData()
    {
        try
        {
            socketConnection = new TcpClient(Globals.ip, Globals.port);
            SendMessage(new HelloMessage(Globals.name));
            SendMessage(new GetStateMessage());
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading 		
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message. 						
                        string serverMessage = Encoding.UTF8.GetString(incommingData);
                        receivedMessage = JsonUtility.FromJson<MessageObject>(serverMessage);
                        Debug.Log("server message received as: " + serverMessage);
                        if (receivedMessage.type.Equals("end"))
                        {
                            Disconnect();
                            clientReceiveThread.Abort();
                        }
                        if (receivedMessage.type.Equals("chat"))
                        {
                            receivedChatMessage = new ChatMessage(receivedMessage);
                            //Debug.Log("receivedChatMessage: " + receivedChatMessage);
                        }
                        if (receivedMessage.type.Equals("welcome"))
                        {
                            receivedWelcomeMessage = new WelcomeMessage(receivedMessage);
                            //Debug.Log("receivedWelcomeMessage: " + receivedMessage);
                        }
                        if (receivedMessage.type.Equals("state"))
                        {
                            //Debug.Log("receivedStateMessage");
                            Dictionary<string, string> posMessage = JsonConvert.DeserializeObject<Dictionary<string, string>>(serverMessage);
                            receivedStateMessage = new StateMessage(JsonConvert.DeserializeObject<Dictionary<string, string>>(posMessage["positions"]));
                        }

                        if (receivedMessage.type.Equals("stateofcubes"))
                        {
                            //Debug.Log("receivedStateOfCubesMessage");
                            Dictionary<string, string> posMessage = JsonConvert.DeserializeObject<Dictionary<string, string>>(serverMessage);
                            List<ReceivedCubeFromServer> receivedCubesFromServer = JsonConvert.DeserializeObject<List<ReceivedCubeFromServer>>(posMessage["cubes"]);
                            receivedStateofCubesFromServerMessage = new StateOfCubesFromServerMessage(receivedCubesFromServer);
                        }

                        if (receivedMessage.type.Equals("addCube"))
                        {
                            //Debug.Log("receivedStateOfCubesMessage");
                            Dictionary<string, string> posMessage = JsonConvert.DeserializeObject<Dictionary<string, string>>(serverMessage);
                            receivedAddCubeFromServerMessage = new AddCubeFromServerMessage(posMessage["position"],posMessage["matIndex"], posMessage["owner"]);
                        }

                        if (receivedMessage.type.Equals("removeCube"))
                        {
                            //Debug.Log("receivedStateOfCubesMessage");
                            Dictionary<string, string> posMessage = JsonConvert.DeserializeObject<Dictionary<string, string>>(serverMessage);
                            receivedRemoveCubeFromServerMessage = new RemoveCubeFromServerMessage(posMessage["position"]);
                        }
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }


    public void SendMessage(MessageObject messageObject)
    {
        if (socketConnection == null)
        {
            return;
        }
        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                string message = JsonConvert.SerializeObject(messageObject) + '\n';
                // Convert string message to byte array.                 
                byte[] clientMessageAsByteArray = Encoding.UTF8.GetBytes(message);
                // Write byte array to socketConnection stream.                 
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                Debug.Log("Client sent a message with typ: " + messageObject.type);
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    public void sendGetState()
    {
        SendMessage(new GetStateMessage());
    }

    public void sendGetStateOfCubes()
    {
        SendMessage(new GetStateOfCubesMessage());
    }

}
