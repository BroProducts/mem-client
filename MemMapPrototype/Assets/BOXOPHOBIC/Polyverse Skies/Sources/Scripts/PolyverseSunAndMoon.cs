//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PolyverseSunAndMoon : MonoBehaviour {

	[Header("Sun and Moon Direction")]
	public GameObject sunDirection;
	public GameObject moonDirection;

	private Vector3 GlobalSunDirection = Vector3.zero;
	private Vector3 GlobalMoonDirection = Vector3.zero;

	void Update () {

		if (sunDirection != null) {
			GlobalSunDirection = -sunDirection.transform.forward;
			Shader.SetGlobalVector ("GlobalSunDirection", GlobalSunDirection);
		} else {
			GlobalSunDirection = Vector3.zero;
			Shader.SetGlobalVector ("GlobalSunDirection", GlobalSunDirection);
		}

		if (moonDirection != null) {
			GlobalMoonDirection = -moonDirection.transform.forward;
			Shader.SetGlobalVector ("GlobalMoonDirection", GlobalMoonDirection);
		} else {
			GlobalSunDirection = Vector3.zero;
			Shader.SetGlobalVector ("GlobalMoonDirection", GlobalMoonDirection);
		}


	}


}
