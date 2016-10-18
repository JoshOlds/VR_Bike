using UnityEngine;
using System.Collections;

public class RMCInstructions : MonoBehaviour {
	

	void Update(){

		if(Input.GetKeyDown(KeyCode.R))
			Application.LoadLevel(Application.loadedLevel);

	}
	

	void OnGUI () {

		GUILayout.BeginArea (new Rect (50,50,1000,100));

			GUILayout.Label ("W, A, S, D for controls, CTRL for lean back, SPACE for rear wheel brake");
			GUILayout.Label ("G for siren, and L for headlight");
			GUILayout.Label ("Press ''C'' for change camera");
			GUILayout.Label ("Press ''R'' for reset scene");

		GUILayout.EndArea (); 

	}

}
