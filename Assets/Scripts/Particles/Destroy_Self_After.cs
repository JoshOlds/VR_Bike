using UnityEngine;
using System.Collections;

public class Destroy_Self_After : MonoBehaviour {

    public float lifeTime = 0;

    private float startTime;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Time.time - startTime > lifeTime)
        {
            Destroy(gameObject);
        }
	}
}
