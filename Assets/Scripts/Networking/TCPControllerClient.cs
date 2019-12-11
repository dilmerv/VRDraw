using System;
using System.Net.Sockets;
using System.Threading;
using DilmerGames.Core.Singletons;
using TMPro;
using UnityEngine;

public class TCPControllerClient : Singleton<TCPControllerClient>
{
    [SerializeField]
    private bool enableTCPClient = false;

    [SerializeField]
    private string serverIPAddress = "127.0.0.1";

    [SerializeField]
    private int port = 13000;

    [SerializeField]
    private TextMeshProUGUI connectionText;

    [SerializeField]
    private int clientId = 1;

    [SerializeField]
    private int waitTimeSync = 1000;
    
    [SerializeField]
    private Color color = Color.white;

    [SerializeField]
    private DrawPayLoad drawPayLoad = new DrawPayLoad();
    
    private DrawLine activeLine = new DrawLine();

    private string prevPayLoad;
    
    private System.Object safeLock = new System.Object();

    private int messageCount = 0;
    private int maxMessageCount = 10;

    void Awake()
    {
        if(!enableTCPClient)
        {
            enabled = enableTCPClient;
        }
        
        // create singleton object before threads are created
        Dispatcher dispatcher = Dispatcher.Instance;
    }

    void Start()
    {
        new Thread(() => {
            Thread.CurrentThread.IsBackground = true;
            ConnectClient(serverIPAddress, port, clientId, drawPayLoad);
        }).Start();
    }

    public void UpdateLine(Vector3 position)
    {
        if(!drawPayLoad.lines.Contains(activeLine))
        {
            drawPayLoad.lines.Add(activeLine);
        }
        activeLine.points.Add(new DrawPoint(position.x, position.y, position.z));
    }

    public void AddNewLine(Vector3 position)
    {
        drawPayLoad.lines.Add(activeLine);
        activeLine = new DrawLine();
        activeLine.points.Add(new DrawPoint(position.x, position.y, position.z));
    }

    private void Message(int clientId, string message)
    {
        string clientConnected = $"ClientId : {clientId}";
        string clientMessage = $"{message}";
        connectionText.text += $"{clientConnected}\n{clientMessage}\n\n";

        if(messageCount >= maxMessageCount)
        {
            connectionText.text = string.Empty;
            messageCount = 0;
        }
        messageCount++;
    }

    private void ConnectClient(string server, int port, int clientId, DrawPayLoad drawPayLoad)
    {
        try
        {
            TcpClient client = new TcpClient(server, port);
            NetworkStream stream = client.GetStream();

            if(client.Connected)
            {
                Dispatcher.Instance.Enqueue(() => Message(clientId, $"Connected to server {server}:{port}"));
                
                while(true)
                {   
                    lock(safeLock)
                    {
                        string payLoad = JsonUtility.ToJson(drawPayLoad);
                        
                        // check if server already got payload
                        if(prevPayLoad != payLoad)
                        {   
                            prevPayLoad = payLoad;
                            // send information to the server
                            byte[] data = System.Text.Encoding.ASCII.GetBytes(payLoad);
                            stream.Write(data, 0, data.Length);

                            Dispatcher.Instance.Enqueue(() => Message(clientId, $"{DateTime.Now} Sent payload"));

                            // getting information from the server
                            data = new byte[256];
                            int bytes = stream.Read(data, 0, data.Length);
                            string response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                            
                            Dispatcher.Instance.Enqueue(() => Message(clientId, $"{DateTime.Now} Received payload"));
                        }

                        Thread.Sleep(waitTimeSync);
                    }
                }
                stream.Close();
                client.Close();
            }
            else 
            {
                Dispatcher.Instance.Enqueue(() => Message(clientId, $"Could not connect to the server"));
            }
        }
        catch(Exception e)
        {
            Dispatcher.Instance.Enqueue(() => Message(clientId, $"Exception: {e.Message}"));
        }

        Console.Read();
    }
}
