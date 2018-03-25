using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationIcons : MonoBehaviour {

	public void GoToMenu ()
	{
		SceneManager.LoadScene (0);
	}
}
