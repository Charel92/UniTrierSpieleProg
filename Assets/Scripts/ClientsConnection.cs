using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

public class ClientsConnection : MonoBehaviour
{
    public int clients = 10;
    public GameObject controllers;
    private TcpClient socketConnection;
    private List<Thread> clientReceiveThreads;
    private Thread clientReceiveThread;

    public void ConnectToTcpServer()
    {
           try
           {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData)); 
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
            clientReceiveThreads.Add(clientReceiveThread);
            Debug.Log("connected to Server");
           }
           catch (Exception e)
           {
               Debug.Log("On client connect exception " + e);
           }

    }


    private void ListenForData()
    {
        try
        {
            socketConnection = new TcpClient(Globals.ip, Globals.port);
            SendMessage(new HelloMessage("client_"+Globals.clientCount));
            Globals.clientCount++;
            //SendMessage(new MessageObject(MessageType.State));
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
                string message = JsonUtility.ToJson(messageObject) + '\n';
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
}
