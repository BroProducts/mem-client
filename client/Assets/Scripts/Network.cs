using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Colyseus;

using GameDevWare.Serialization;
using GameDevWare.Serialization.MessagePack;

public class Network : MonoBehaviour
{

    Client client;
    Room room;
    public string serverName = "localhost";
    public string serverPort = "2657";
    public string roomName = "hub";

    // map of players
    Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

    // Use this for initialization
    IEnumerable Start()
    {
        String uri = "ws://" + serverName + ":" + serverPort;
        client = new Client(uri);
        client.OnOpen += OnOpenHandler;
        client.OnClose += (object sender, EventArgs e) => Debug.Log("CONNECTION CLOSED");

        yield return StartCoroutine(client.Connect());

        room = client.Join(roomName);
        room.OnReadyToConnect += (sender, e) => StartCoroutine(room.Connect());
        room.OnJoin += OnRoomJoined;
        room.OnUpdate += OnUpdateHandler;

        room.OnData += (object sender, MessageEventArgs e) => Debug.Log(e.data);

        int i = 0;

        while (true)
        {
            client.Recv();

            // string reply = client.RecvString();
            if (client.error != null)
            {
                Debug.LogError("Error: " + client.error);
                break;
            }

            i++;

            if (i % 50 == 0)
            {
                room.Send("some_command");
            }

            yield return 0;
        }
    }

    void OnOpenHandler (object sender, EventArgs e)
    {
        Debug.Log("Connectet to Server. Client id: " + client.id);
    }

    void OnRoomJoined (object sender, EventArgs e)
    {
        Debug.Log("Joined Hub successfully.");
    }
    void OnUpdateHandler(object sender, RoomUpdateEventArgs e)
    {
        Debug.Log("Connected to server. Client id: " + client.id);
    }

}