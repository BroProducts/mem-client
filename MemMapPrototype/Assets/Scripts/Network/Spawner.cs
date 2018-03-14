using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject playerPrefab;

	Dictionary<string, GameObject> players = new Dictionary<string, GameObject> ();

	public GameObject PlayerSpawn(string id)
	{
		var player = Instantiate (playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

		player.GetComponent<NetworkEntity> ().id = id;

		PlayerAdd (id, player);

		return player;
	}

	public GameObject PlayerFindById(string id)
	{
		return players [id];
	}

	public void PlayerAdd(string id, GameObject player)
	{
		players.Add (id, player);
	}

	public void PlayerRemove (string id)
	{
		var player = players [id];
		Destroy (player);
		players.Remove (id);
	}
}
