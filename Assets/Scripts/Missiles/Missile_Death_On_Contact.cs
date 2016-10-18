using UnityEngine;
using System.Collections;

public class Missile_Death_On_Contact : MonoBehaviour {

    public float collisionDamage = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        GameObject objectHit = collision.gameObject;
        objectHit.SendMessage("applyDamage", collisionDamage, SendMessageOptions.DontRequireReceiver);
        gameObject.SendMessage("onDeath", SendMessageOptions.DontRequireReceiver);
    }
}
