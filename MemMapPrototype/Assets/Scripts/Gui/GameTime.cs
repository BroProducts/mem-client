using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Colyseus;
using GameDevWare.Serialization;

public class GameTime : MonoBehaviour {

	public ColyseusClient colyseusClient;

	TextMeshProUGUI textmeshPro;

	// Use this for initialization
	void Start () {
		textmeshPro = GetComponent<TextMeshProUGUI> ();
		
        colyseusClient.onDeleyedTimeChange += onDeleyedTimeChange;
	}

    void onDeleyedTimeChange(DataChange change)
    {
        var delayedTime = change.value;
        print(delayedTime);
        //textmeshPro.SetText(deleyedTime);
    }

    // Update is called once per frame   
    void Update () {
	}
}
