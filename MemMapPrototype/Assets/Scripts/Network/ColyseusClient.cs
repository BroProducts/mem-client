using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Colyseus;

using GameDevWare.Serialization;
using GameDevWare.Serialization.MessagePack;

public class ColyseusClient : MonoBehaviour {

	Client client;
	Room room;
	public string serverName = "localhost";
	public string port = "2657";
	public string roomName = "hub";

	// Use this for initialization
	IEnumerator Start () {

		String uri = "ws://" + serverName + ":" + port;
		client = new Client(uri);
		client.OnOpen += OnOpenHandler;
		client.OnClose += (object sender, EventArgs e) => Debug.Log ("CONNECTION CLOSED");

		yield return StartCoroutine(client.Connect());

		room = client.Join(roomName);
		room.OnReadyToConnect += (sender, e) => StartCoroutine ( room.Connect() );
		room.OnJoin += OnRoomJoined;
		room.OnUpdate += OnUpdateHandler;


		room.OnData += (object sender, MessageEventArgs e) => Debug.Log(e.data);


		OnApplicationQuit();
	}

	void OnDestroy ()
	{
		// Make sure client will disconnect from the server
		room.Leave ();
		client.Close ();
	}

	void OnOpenHandler (object sender, EventArgs e)
	{
		Debug.Log("Connected to server. Client id: " + client.id);
	}

	void OnRoomJoined (object sender, EventArgs e)
	{
		Debug.Log("Joined room successfully.");
	}
	void OnUpdateHandler (object sender, RoomUpdateEventArgs e)
	{
		Debug.Log("State Update.");
	}

	void OnApplicationQuit()
	{
		// Ensure the connection with server is closed immediatelly
		client.Close();
	}
}