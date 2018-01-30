using UnityEngine;
using System.Collections;

public class DemoController : MonoBehaviour {
	public Transform firePit;
	public Transform burnedTreeSpawner;
	public Transform FlameThrowerSpawner;
	public Transform TorchSpawner;
	public Transform torch;
	public Transform b_tree;
	public Transform sun;
	public Transform nightAdjuster;
	//private Vector3  sunStartPos;
	private Vector3  sunStartRot;
	private float levelOfRed=0f;
	private float levelOfRed_old=0f;
	public bool day = true;

	private Color light_col;
	private int cur_effect_n = 0;
	private Vector3 CameraStartPosition;
	private Quaternion CameraStartRotation;
	private Transform cur_effect;
	public Transform[] effects = new Transform[12];
	// Use this for initialization
	void Start () {
		CameraStartPosition = this.transform.position;
		CameraStartRotation = this.transform.rotation;
		//sunStartPos = sun.position;
		sunStartRot = sun.eulerAngles;

		Init();
	}

	void OnGUI(){
		if (GUI.Button(new Rect(20,50,100,20),"Restart"))
			Init();
		if (GUI.Button(new Rect(135,50,100,20),day == true ? "Switch to night" : "Switch to day"))
			SwitchTD();
		if (GUI.Button(new Rect(250,50,50,20),"<-")){
			cur_effect_n--;
			if (cur_effect_n<0)
				cur_effect_n = 11;
			Init();
		}
		GUI.Label(new Rect(125,80,200,20),"Yellow ... Orange");
		levelOfRed = GUI.HorizontalSlider(new Rect(125, 100, 100, 30), levelOfRed, 0.0f, .55f);
		if (levelOfRed_old != levelOfRed)
			UpdateColor();
		
		string label = "";
		if (cur_effect_n==0)
			label = "Basic Fire";
		else if (cur_effect_n==1)
			label = "Basic Fire with Smoke";
		else if (cur_effect_n==2)
			label = "Dense Fire";
		else if (cur_effect_n==3)
			label = "Dense Fire with Smoke";
		else if (cur_effect_n==4)
			label = "Burning tree effect";
		else if (cur_effect_n==5)
			label = "Torch Fire";
		else if (cur_effect_n==6)
			label = "Torch Fire with Fire";
		else if (cur_effect_n==7)
			label = "Oil Fire";
		else if (cur_effect_n==8)
			label = "Fire Burst";
		else if (cur_effect_n==9)
			label = "FlameThrower type 1";
		else if (cur_effect_n==10)
			label = "FlameThrower type 1 with smoke";
		else if (cur_effect_n==11)
			label = "FlameThrower type 2";
		
		GUI.Label(new Rect(325,50,200,20),label);
		GUI.Label(new Rect(20,80,100,100),"Everything from the demo scene is included into this package");

		if (GUI.Button(new Rect(550,50,50,20),"->")){
			cur_effect_n++;
			if (cur_effect_n>11)
				cur_effect_n = 0;
			Init();
		}
	}

	void UpdateColor(){
		//cur_effect.GetComponent<ParticleSystem>().startColor = new Color(1f,1f-levelOfRed,1f-levelOfRed);
		//cur_effect.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", new Color(1f,1f-levelOfRed,1f-levelOfRed));
		ParticleSystemRenderer[] psrs =  cur_effect.GetComponentsInChildren<ParticleSystemRenderer>();
		foreach(ParticleSystemRenderer psr in psrs){
			if (!psr.gameObject.name.Contains("Smoke"))
				psr.material.SetColor("_TintColor", new Color(1f,1f-levelOfRed,1f-levelOfRed));
			else 
				psr.material.SetColor("_TintColor", new Color(1f,1f-levelOfRed/2f,1f-levelOfRed/2f));
		}
		// smoke shouldn't be colored by fire that strong that's why  division by 2

		cur_effect.transform.Find("Light").GetComponent<Light>().color = new Color(light_col.r,light_col.g-levelOfRed/4f,light_col.b);
		levelOfRed_old = levelOfRed;
	}

	void SwitchTD(){ // switch time of day
		if (day)
			day = false;
		else 
			day = true;
		if (day){
			RenderSettings.ambientIntensity=1.25f;
			sun.eulerAngles = sunStartRot;
			sun.GetComponent<Light>().intensity = 1f;
			nightAdjuster.gameObject.SetActive(false);
			DynamicGI.UpdateEnvironment();  // need it for updating time of day
		}
		if (!day){
			RenderSettings.ambientIntensity=.5f;
		//	RenderSettings.
			sun.eulerAngles = new Vector3(270f,0f,0f);
			sun.GetComponent<Light>().intensity = 0f;
			nightAdjuster.gameObject.SetActive(true);
			DynamicGI.UpdateEnvironment();  // need it for updating time of day
	
		}
	}

	void Init(){
		if (cur_effect!=null)
			Destroy(cur_effect.gameObject);

		Vector3 rot = new Vector3(-90f,0f,0f);
		Vector3 pos = new Vector3(0f,0f,0f);
		b_tree.gameObject.SetActive(false);
		torch.gameObject.SetActive(false);
		if (cur_effect_n<4)
			pos = firePit.position;
		else if (cur_effect_n == 4){
			pos = burnedTreeSpawner.position;
			this.transform.position = CameraStartPosition;
			this.transform.rotation = CameraStartRotation;
			b_tree.gameObject.SetActive(true);
		}
		else if (cur_effect_n == 5 || cur_effect_n == 6) {
			pos = TorchSpawner.position;
			torch.gameObject.SetActive(true);
		}
		else if (cur_effect_n == 7 || cur_effect_n == 8) 
			pos = firePit.position;
		else {
			pos = FlameThrowerSpawner.position;
			rot = new Vector3(0f,0f,0f);
		}
		cur_effect = Instantiate(effects[cur_effect_n],pos,Quaternion.Euler(rot)) as Transform;
		light_col = cur_effect.transform.Find("Light").GetComponent<Light>().color;
		UpdateColor();
	}

	// Update is called once per frame
	void Update () {
		if (cur_effect_n != 4)
			this.transform.RotateAround(firePit.position,Vector3.up,-0.1f);
	}
}
