using UnityEngine;
using System.Collections;

public class GuiDemo : MonoBehaviour {

	public GameObject lightRotate;
	public GameObject planetRotate;
	public GameObject atmosphere;

	public Texture2D[] diffuse;
	public Texture2D[] normal;
	public Texture2D noNormal;

	private Rotate rotLight;
	private Rotate rotPlanet;


	private int r;
	private int g;
	private int b;

	private bool bumped;

	private bool atmos;
	private float atmosSize;

	private int index;

	void Start(){
		rotLight = lightRotate.GetComponent<Rotate>();
		rotPlanet = planetRotate.GetComponent<Rotate>();
		atmos = true;

		Color col = atmosphere.GetComponent<Renderer>().material.GetColor("_AtmoColor");
		r = (int)(col.r * 255);
		g = (int)(col.g * 255);
		b = (int)(col.b * 255);

		rotLight.enable = false;

		atmosSize = atmosphere.GetComponent<Renderer>().material.GetFloat("_Size");
	}

	void OnGUI(){

		rotLight.enable = GUI.Toggle(new Rect(0,0,200,20),rotLight.enable,"Rotating light");
		rotPlanet.enable = GUI.Toggle(new Rect(0,20,200,20),rotPlanet.enable,"Rotating planet");

		bumped = GUI.Toggle(new Rect(0,60,200,20),bumped,"Bumped");

		if (GUI.Button( new Rect(50,290,100,20),"Prev")){
			index--;
			if (index<0) index=9;
		}

		if (GUI.Button( new Rect(810,290,100,20),"Next")){
			index++;
			if (index>9) index=0;
		}

		planetRotate.GetComponent<Renderer>().material.SetTexture("_MainTex",diffuse[index]);

		if (bumped){
			planetRotate.GetComponent<Renderer>().material.SetTexture("_Normals",normal[index]);
		}
		else{
			planetRotate.GetComponent<Renderer>().material.SetTexture("_Normals",noNormal);
		}


		// Atmos sphere
		atmos = GUI.Toggle(new Rect(0,100,200,20),atmos,"Atmosphere");

		GUI.backgroundColor = new Color((float)r/255f,0,0);
		r = (int)GUI.HorizontalSlider( new Rect(0,100+20,200,20),(float)r,0,255);
		GUI.backgroundColor = new Color(0,(float)g/255f,0);
		g = (int)GUI.HorizontalSlider( new Rect(0,100+40,200,20), (float)g,0,255);
		GUI.backgroundColor = new Color(0,0,(float)b/255f);
		b = (int)GUI.HorizontalSlider( new Rect(0,100+60,200,20),(float)b,0,255);
		GUI.backgroundColor = Color.white;
		atmosphere.GetComponent<Renderer>().material.SetColor("_AtmoColor", new Color( (float)r/255f,(float)g/255f,(float)b/255f ));

		GUI.Label( new Rect(0,100+80,200,20),"Atmosphere size");
		atmosSize = GUI.HorizontalSlider( new Rect(0,100+100,200,20), atmosSize,0,0.2f);
		atmosphere.GetComponent<Renderer>().material.SetFloat("_Size",atmosSize);

		planetRotate.GetComponent<Renderer>().material.SetColor("_AtmosNear", new Color( (float)r/255f,(float)g/255f,(float)b/255f ));

		atmosphere.SetActive( atmos);

		// Bumped


		//

	}
}
