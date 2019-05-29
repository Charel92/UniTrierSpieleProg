
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ServerController : MonoBehaviour
{
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    private MessageObject messageObject;
    private MessageObject receivedMessage;
    public GameObject player;

    // Use this for initialization 	
    void Start()
    {
        ConnectToTcpServer();
    }

    void Update()
    {
        if (receivedMessage != null) {
            player.GetComponent<Messenger>().printMessage(receivedMessage);
            receivedMessage = null;
        }
    }

    /// <summary> 	
    /// Setup socket connection. 	
    /// </summary> 	
    private void ConnectToTcpServer()
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
            socketConnection = new TcpClient("localhost", 4321);
            SendMessage(new MessageObject("me"));
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
                        messageObject = JsonUtility.FromJson<MessageObject>(serverMessage);
                        Debug.Log("server message received as: " + serverMessage);
                        Debug.Log("messageObject received is: " + messageObject);
                        if (messageObject.type.Equals("end"))
                        {
                            Disconnect();
                            clientReceiveThread.Abort();
                        }
                        if (messageObject.type.Equals("chat"))
                        {
                            receivedMessage = messageObject;
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
    /// <summary> 	
    /// Send message to server using socket connection. 	
    /// </summary> 	
    public void SendMessage(MessageObject messageObject)
    {
        if (socketConnection == null)
        {
            Debug.Log("in nullll");
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
                Debug.Log("Client sent his message - should be received by server");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}