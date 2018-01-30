using UnityEngine;
using System.Collections;

public class LightTimer : MonoBehaviour {
	public float lifetime;
	private float startIntensity;
	// Use this for initialization
	void Start () {
		startIntensity = this.GetComponent<Light>().intensity;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.GetComponent<Light>().intensity>0f)
			this.GetComponent<Light>().intensity -= startIntensity*Time.deltaTime/(lifetime > 0 ? lifetime : lifetime = .1f);
	}
}
