using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTime : MonoBehaviour {

	public ColyseusClient colyseusClient;

	TextMeshProUGUI textmeshPro;

	// Use this for initialization
	void Start () {
		textmeshPro = this.GetComponent<TextMeshProUGUI> ();
		textmeshPro.SetText ("hallo");
	}
	
	// Update is called once per frame
	void Update () {
	}
}
