using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public Vector3 rot;
	public float speed;
	public bool enable=true;

	// Update is called once per frame
	void Update () {
		if (enable)
			transform.Rotate( rot * Time.deltaTime *speed);	
	}
}
