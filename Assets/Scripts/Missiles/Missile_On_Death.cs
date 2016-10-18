using UnityEngine;
using System.Collections;

public class Missile_On_Death : MonoBehaviour {

    public GameObject deathParticle;
    public AudioSource explosion;

	// Use this for initialization
	void Start () {
 
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void onDeath()
    {
        Instantiate(deathParticle, transform.position, transform.rotation);
        gameObject.SendMessage("explode", SendMessageOptions.DontRequireReceiver);
        explosion.Play();
        Destroy(gameObject, explosion.clip.length);
    }
}
