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
    public string serverName = "mem-server.herokuapp.com";
    public string serverPort = "4200";
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

        OnApplicationQuit();
    }
    void OnOpenHandler (object sender, EventArgs e)
    {
        Debug.Log("Connectet to Server. Client id: " + client.id);
    }
    void OnApplicationQuit()
    {
        client.Close();
    }
}
