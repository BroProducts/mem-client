using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
[InitializeOnLoad]
public class LayerController{
	//STARTUP
	static LayerController()
	{
		CreateLayer();

	}

	static void ModifyFireLight(int layer_n){
		try {
			GameObject FireLights = AssetDatabase.LoadAssetAtPath("Assets/LP_Fire/Prefabs/Night_Fire_Light  !!Check_Guide!!.prefab", typeof(GameObject)) as GameObject; 
			string[] paths = AssetDatabase.FindAssets("t:prefab",new string[] {"Assets/LP_Fire/Prefabs"});
		
			for (int j = 0; j< paths.Length; j++){
				GameObject prefab = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(paths[j]), typeof(GameObject)) as GameObject; 
				prefab.gameObject.layer = layer_n;
				for (int i = 0; i< prefab.transform.childCount;i++){
					prefab.transform.GetChild(i).gameObject.layer = layer_n;
				}
			}
			for (int i=0;i<3;i++){
				GameObject child = FireLights.transform.GetChild(i).gameObject;
				child.GetComponent<Light>().cullingMask = 1 << LayerMask.NameToLayer("LP_Fire");
			}
		}
		catch (Exception e) {
			Debug.Log("It seems that you imported this asset not in the root assets folder, please, consult the included GUIDE");
		}  
	}
	//creates a new layer
	static void CreateLayer(){
		SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

		SerializedProperty layers = tagManager.FindProperty("layers");
		if (layers == null || !layers.isArray)
		{
			Debug.LogWarning("Can't set up the layers.  It's possible the format of the layers and tags data has changed in this version of Unity.");
			Debug.LogWarning("Please read the guide for manual layer setup");
			return;
		}
		bool exist = false;
		int layer_n = -1;
		for (int i=8;i<32;i++)
		{
			SerializedProperty layer = layers.GetArrayElementAtIndex(i);
			if (layer.stringValue == "LP_Fire"){
				exist = true;
				layer_n = i;
			}
		}
		if (!exist){
			for (int i=8;i<32;i++)
			{
				SerializedProperty layer = layers.GetArrayElementAtIndex(i);
				if (layer.stringValue == ""){
					layer.stringValue = "LP_Fire";
					layer_n = i;
					break;
				}
			}
		}
	
		tagManager.ApplyModifiedProperties();
		if (layer_n == -1)
			Debug.Log("Please make one layer field free and consult the guide");
		else
			ModifyFireLight(layer_n);
	}
}
