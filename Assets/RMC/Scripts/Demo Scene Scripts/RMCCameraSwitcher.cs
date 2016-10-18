using UnityEngine;
using System.Collections;

public class RMCCameraSwitcher : MonoBehaviour { 

	private GameObject[] cameras;
	private int actCamera = 0;

	void Start () {
		
		cameras = GameObject.FindGameObjectsWithTag("MainCamera");

	}
	

	void Update () {

		if(Input.GetKeyDown(KeyCode.C)){

			if(actCamera < cameras.Length-1){
				cameras[actCamera].GetComponent<Camera>().enabled = false;
				cameras[actCamera+1].GetComponent<Camera>().enabled = true;
				actCamera++;
			}else{
				actCamera = 0;
				cameras[cameras.Length - 1].GetComponent<Camera>().enabled = false;
				cameras[0].GetComponent<Camera>().enabled = true;
			}

		}

	}

}
