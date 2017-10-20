using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Colyseus;

using GameDevWare.Serialization;
using GameDevWare.Serialization.MessagePack;

public class Network : MonoBehaviour
{

    Client client;
    Room hub;
    public string serverName = "localhost";
    public string serverPort = "2657";
    public string hubName = "hub";


    // Use this for initialization
    IEnumerable Start()
    {
        String uri = "ws://" + serverName + ":" + serverPort;
        client = new Client(uri);
        client.OnOpen += OnOpenHandler;
        client.OnClose += (object sender, EventArgs e) => Debug.Log("CONNECTION CLOSED");


        yield return StartCoroutine(client.Connect());

        hub = client.Join(hubName);
        hub.OnReadyToConnect += (sender, e) => StartCoroutine(hub.Connect());
        hub.OnJoin += OnHubJoined;

        OnApplicationQuit();
    }
    void OnOpenHandler (object sender, EventArgs e)
    {
        Debug.Log("Connectet to Server. Client id: " + client.id);
    }

    void OnHubJoined (object sender, EventArgs e)
    {
        Debug.Log("Joined Hub successfully.");
    }

    void OnApplicationQuit()
    {
        client.Close();
    }

}