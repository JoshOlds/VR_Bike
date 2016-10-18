using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour {
    // Update is called once per frame

    public AudioSource pickup;

	void Update () {
		transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime); 
	}

    void OnTriggerEnter(Collider collider) {
        pickup.Play();
        Destroy(gameObject, pickup.clip.length);
    }
}