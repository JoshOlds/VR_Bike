using UnityEngine;
using System.Collections;

public class Missile_Death_On_ZDepth : MonoBehaviour {


    public Missile_On_Death deathScript;
    public float zDepth = 50;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(transform.position.z >= zDepth)
        {
            gameObject.SendMessage("onDeath", SendMessageOptions.DontRequireReceiver);
        }
	}

    
}
