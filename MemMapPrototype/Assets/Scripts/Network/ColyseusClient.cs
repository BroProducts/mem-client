using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Colyseus;

using GameDevWare.Serialization;
using GameDevWare.Serialization.MessagePack;

public class ColyseusClient : MonoBehaviour {

	public Client client;
	public Room room;
	public string serverName = "localhost";
	public string port = "2657";
	public string roomName = "hub";
	public GameObject myPlayer;
	public Spawner spawner;

	WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

	// map of players
	Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

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


		room.Listen("players/:id", OnPlayerChange);

		room.Listen ("players/:id/moveTo/:axis", this.OnPlayerMove);

		room.Listen("teams/:id", OnTeamChange);

		room.Listen (this.OnChangeFallback);

		room.OnData += OnData;

		StartCoroutine("SendMyPlayerPosition");

		while (true)
		{
			client.Recv();

			if (client.error != null)
			{
				Debug.LogError ("Error: " + client.error);
				break;
			}

			yield return 0;
		}



		OnApplicationQuit();
	}

	IEnumerator SendMyPlayerPosition() {
		while (true)
		{
			// Place your method calls
			if (room.sessionId != null) {
				Debug.Log ("SET_PLAYER_POSITION");
				var playerPosition = myPlayer.transform.position;
				room.Send (new {
					action = "SET_PLAYER_POSITION",
					payload = new {
						x = playerPosition.x,
						y = playerPosition.y,
						z = playerPosition.z
					}
				});
			}
			yield return waitForSeconds;
		}
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
		//SendMoveTo (new Vector3(10,0,10));
	}

	void OnData (object sender, MessageEventArgs e)
	{
		var data = (IndexedDictionary<string, object>) e.data;
		if((data["action"] as string == "MOVE_PLAYER_TO") && (data["playerId"] as string != room.sessionId)) {
			
			var player = spawner.PlayerFindById (data ["playerId"] as string);
			var agent = player.GetComponent<UnityEngine.AI.NavMeshAgent>();

			var x = data ["x"];
			var y = data ["y"];
			var z = data ["z"];

			var destinationX = float.Parse (x as string);
			var destinationY = float.Parse (y as string);
			var destinationZ = float.Parse (z as string);

			var destination = new Vector3 (destinationX, destinationY, destinationZ);

			agent.SetDestination(destination);

		}
	}

	void OnUpdateHandler (object sender, RoomUpdateEventArgs e)
	{
		// Setup room first state
		if (e.isFirstState) {
			IndexedDictionary<string, object> players = (IndexedDictionary<string, object>) e.state ["players"];

			// trigger to add existing players 
			/*
			foreach(KeyValuePair<string, object> player in players)
			{
				this.OnPlayerChange (new DataChange {
					path = new Dictionary<string, string> {
						{"id", player.Key}
					},
					operation = "add",
					value = player.Value
				});
			}
			*/
		}
	}

	void OnPlayerChange (DataChange change)
	{
		Debug.Log ("OnPlayerChange");
		Debug.Log (change.operation);
		Debug.Log (change.path["id"]);
		Debug.Log (change.value);

		if (change.operation == "add") {
			if (change.path ["id"] == room.sessionId) {
				spawner.PlayerAdd (change.path ["id"], myPlayer);
				Debug.Log ("My Player Added");
			} else {

				var data = (IndexedDictionary<string, object>) change.value;
				
				var currentPosition = (IndexedDictionary<string, object>) data["currentPosition"];

				var x = Convert.ToSingle (currentPosition ["x"]);
				var y = Convert.ToSingle (currentPosition ["y"]);
				var z = Convert.ToSingle (currentPosition ["z"]);

				spawner.PlayerSpawn (change.path ["id"], new Vector3(x, y, z));
				Debug.Log ("Other Player Added");
			}

		} else if (change.operation == "remove") {
			spawner.PlayerRemove (change.path ["id"]);
			Debug.Log ("Player removed");

		}
	}

	void OnPlayerMove (DataChange change)
	{
		/*
		Debug.Log ("ON_PLAYER_MOVED");
		var x = change.path;
		var y = x ["axis"];
		Debug.Log ("playerId: " + change.path["id"] + ", Axis: " + change.path["axis"]);
		Debug.Log (change);
		Debug.Log (change.value);
		*/

		/*
		GameObject cube;
		players.TryGetValue (change.path ["id"], out cube);

		cube.transform.Translate (new Vector3 (Convert.ToSingle(change.value), 0, 0));
		*/
	}

	void OnPlayerRemoved (DataChange change)
	{
		Debug.Log ("OnPlayerRemoved");
		Debug.Log (change.path);
		Debug.Log (change.value);
	}

	void OnTeamChange (DataChange change)
	{
		/*
		Debug.Log ("OnTeamChange");
		Debug.Log (change.operation);
		Debug.Log (change.path["id"]);
		Debug.Log (change.value);

		if (change.operation == "add") {

			Debug.Log ("Team added");

		} else if (change.operation == "remove") {

			Debug.Log ("Team removed");

		}
		*/
	}

	void OnMessageAdded (DataChange change)
	{
		Debug.Log ("OnMessageAdded");
		Debug.Log (change.path["number"]);
		Debug.Log (change.value);
	}

	void OnChangeFallback (PatchObject change)
	{
		// Debug.Log ("OnChangeFallback");
		// Debug.Log (change.operation);
		// Debug.Log (change.path);
		// Debug.Log (change.value);
	}

	void OnApplicationQuit()
	{
		// Ensure the connection with server is closed immediatelly
		client.Close();
	}

	public void SendMoveTo(Vector3 destination) {
		room.Send (new { 
			action = "MOVE_PLAYER_TO",
			payload = new {
				x = destination.x,
				y = destination.y,
				z = destination.z
			}
		});
	}
}
