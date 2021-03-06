﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text;
using System.Collections.Generic;

public class Player
{
    public string playerName;
    public GameObject avatar;
    public int connectionId;
}

public class Client : MonoBehaviour
{
    private const int MAX_CONNECTION = 100;

    private int port = 6666;

    private int hostID;
    private int webHostID;

    private int reliableChannel;
    private int unreliableChannel;

    private int ourClientId;
    private int connectionId;

    private float connectionTime;
    private bool isConnected = false;
    private bool isStarted = false;
    private byte error;

    private string playerName;

    public GameObject playerPrefab;
    public Dictionary<int, Player> players = new Dictionary<int, Player>();

    public void Connect()
    {
        // Does the player have a name?
        string pName = GameObject.Find("NameInput").GetComponent<InputField>().text;
        if(pName == "")
        {
            Debug.Log("You must enter a name");
            return;
        }

        playerName = pName;

        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        hostID = NetworkTransport.AddHost(topo, 0);
        connectionId = NetworkTransport.Connect(hostID, "127.0.0.1", port, 0, out error);

        connectionTime = Time.time;
        isConnected = true;
    }

    private void Update()
    {
        if (!isConnected)
            return;

        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData)
        {
            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                Debug.Log("Receiving : " + msg);
                string[] splitData = msg.Split('|');

                switch(splitData[0])
                {
                    case "ASKNAME":
                        OnAskName(splitData);
                        break;
                    case "CNN":

                        SpawnPlayer(splitData[1], int.Parse(splitData[2]));
                        break;
                    case "DC":
                        PlayerDisconnected(int.Parse(splitData[1]));
                        break;
                    case "ASKPOSITION":
                        OnAskPosition(splitData);
                        break;
                    default:
                        Debug.Log("Invalid message : " + msg);
                        break;
                }

                break;
        }
    }

    private void OnAskName(string[] data)
    {
        // Set this client's ID
        ourClientId = int.Parse(data[1]);

        // Send our name to the server
        Send("NAMEIS|" + playerName, reliableChannel);

        // Create all the other players  - 0 = ASKNAME, 1 = ID so start 2
        for(int i = 2; i < data.Length - 1; i++ )
        {
            string[] d = data[i].Split('%');
            SpawnPlayer(d[0], int.Parse(d[1]));
        }
    }
    private void OnAskPosition(string[] data)
    {
        if (!isStarted)
            return; 

        // Update everyone else
        for(int i = 1; i < data.Length - 1; i++)
        {
            string[] d = data[i].Split('%');

            // Prevent server from updating self
            if (ourClientId != int.Parse(d[0]))
            {
                Vector3 position = Vector3.zero;
                position.x = float.Parse(d[1]);
                position.y = float.Parse(d[2]);
                players[int.Parse(d[0])].avatar.transform.position = position;
            }
        }

        // Send own position
        Vector3 myPosition = players[ourClientId].avatar.transform.position;
        string m = "MYPOSITION|" + myPosition.x.ToString() + "|" + myPosition.y.ToString();
        Send(m, unreliableChannel);
    }

    private void SpawnPlayer(string playerName, int cnnId)
    {
        GameObject go = Instantiate(playerPrefab) as GameObject;

        // is this ours?
        if(cnnId == ourClientId)
        {
            // Add mobility
            go.AddComponent<CharacterMovement>();

            // Remove Canvas
            GameObject.Find("Canvas").SetActive(false);
            print("Deactivating canvas");
            isStarted = true;
        }

        Player p = new Player();
        p.avatar = go;
        p.playerName = playerName;
        p.connectionId = cnnId;
        p.avatar.GetComponentInChildren<TextMesh>().text = playerName;
        players.Add(cnnId, p);
    }
    private void PlayerDisconnected(int cnnId)
    {
        Destroy(players[cnnId].avatar);
        players.Remove(cnnId);
    }

    private void Send(string message, int channelId)
    {
        Debug.Log("Sending : " + message);
        byte[] msg = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostID, connectionId, channelId, msg, message.Length * sizeof(char), out error);
    }
}
