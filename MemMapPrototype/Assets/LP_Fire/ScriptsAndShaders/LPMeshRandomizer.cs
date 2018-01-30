using UnityEngine;
using System.Collections;

public class LPMeshRandomizer : MonoBehaviour {

	Vector3[] disp;
	int[,] sharedVertices;
	[Header("Displacement Rnd Strength")]
	[Range(0.0f, 5.0f)]
	public float displacement;



	[Header("Randomize vertex displacement?")]
	public bool Rnd_disp_myself;
	public bool Rnd_disp_children;


	[Header("Randomize color?")]
	public bool Rnd_color_myself;
	public bool Rnd_color_children;

	[Header("Color Rnd strength 0...10")]
	[Range(0.0f, 10.0f)]
	public float color_str;
	[Header("Randomize scale?")]
	public bool Rnd_scale_myself;
	public bool Rnd_scale_children;
	[Header("Scale Rnd Strength")]
	[Range(1.0f, 5.0f)]
	public float scale_str;
	[Header("Randomize rotation?")]
	public bool Rnd_rot_x_myself;
	public bool Rnd_rot_y_myself;
	public bool Rnd_rot_z_myself;
	public bool Rnd_rot_x_children;
	public bool Rnd_rot_y_children;
	public bool Rnd_rot_z_children;
	[Header("Rotation Rnd strength (-value ... value)")]
	public Vector3 rot_str;

	// Use this for initialization
	void Start () {
		
		if (Rnd_scale_myself)
			this.transform.localScale *= Random.Range(1/(scale_str),scale_str);
		if (Rnd_color_myself)
			this.GetComponent<Renderer>().material.SetColor("_TintColor", this.GetComponent<Renderer>().material.GetColor("_TintColor") + new Color(Random.Range(-1f,1f),Random.Range(-1f,1f),Random.Range(-1f,1f),1)*color_str/10f);
		if (Rnd_rot_x_myself || Rnd_rot_y_myself || Rnd_rot_z_myself)
			this.transform.eulerAngles += new Vector3(Rnd_rot_x_myself?Random.Range(-rot_str.x,rot_str.x):0f,Rnd_rot_y_myself?Random.Range(-rot_str.y,rot_str.y):0f,Rnd_rot_z_myself?Random.Range(-rot_str.z,rot_str.z):0f);
		
		if (Rnd_disp_myself)
		{	
			//Mesh mesh = GetComponent<MeshFilter>().mesh;
			//Vector3[] vertices = mesh.vertices;
			getSharedVerts(this.transform);
			Displacement(this.transform);
		}

		for (int i=0;i<this.transform.childCount;i++){
			
			Transform child =  this.transform.GetChild(i);
			if (Rnd_scale_children)
				child.localScale *= Random.Range(1/(scale_str),scale_str);
			if (Rnd_color_children)
				child.GetComponent<Renderer>().material.SetColor("_TintColor",child.GetComponent<Renderer>().material.GetColor("_TintColor") + new Color(Random.Range(-1f,1f),Random.Range(-1f,1f),Random.Range(-1f,1f),1)*color_str/10f);
			if (Rnd_rot_x_children || Rnd_rot_y_children || Rnd_rot_z_children)
				child.transform.localEulerAngles += new Vector3(Rnd_rot_x_children?Random.Range(-rot_str.x,rot_str.x):0f,Rnd_rot_y_children?Random.Range(-rot_str.y,rot_str.y):0f,Rnd_rot_z_children?Random.Range(-rot_str.z,rot_str.z):0f);
			
			if (Rnd_disp_children)
			{
				//Mesh mesh = child.GetComponent<MeshFilter>().mesh;
				//Vector3[] vertices =  mesh.vertices;
				getSharedVerts(child);
				Displacement(child);
			}
	}
	}
	





	void GetDispValue(Vector3[] vert_pos, Transform obj){
		for (int i=0;i<disp.Length;i++)
			disp[i] = vert_pos[i] + (vert_pos[i])*Random.Range(-2f*displacement,1f*displacement)/20f +(Random.insideUnitSphere)*displacement/10f;
	}

	void getSharedVerts(Transform obj){
		int i = 0;
		int ind =0;
		Vector3[] vert_pos_processing = new Vector3[1000];
		Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		while (i < vertices.Length) {
			int n=0;
			for (int j=0;j<i;j++){
				if (Vector3.Distance(vertices[i],vertices[j])<.01f*this.transform.localScale.x){
					n++;
				}
			}
			if (n == 0 ){
				vert_pos_processing[ind] = vertices[i];
				ind++;
			}
			i++;
		}

		sharedVertices = new int[10,ind];
		Vector3[] vert_pos;
		vert_pos = new Vector3[ind];
		for (int j =0;j<ind;j++)
			vert_pos[j] = vert_pos_processing[j];

		i = 0;
		while (i < ind) {
			int n = 0;
			for (int j = 0;j<vertices.Length;j++){
				if (Vector3.Distance(vert_pos[i],vertices[j])<.01f*this.transform.localScale.x){
					
					sharedVertices[n,i] = j!=0? j : 9999;

					n++;
				}
			}
			i++;
		}

		disp = new Vector3[ind];
		GetDispValue(vert_pos, obj);
	}

	void Displacement(Transform obj){
		int i = 0;
		Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
	
		while (i < (sharedVertices.Length/10)) {
			for (int j=0; j < 10; j++){
				
				if(sharedVertices[j,i]!=0){
					
					int vert_n = sharedVertices[j,i]!=9999 ? sharedVertices[j,i] : 0;

					vertices[vert_n] = disp[i];//
				}
			}
			i++;
		}

		mesh.vertices = vertices;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
	}
}
