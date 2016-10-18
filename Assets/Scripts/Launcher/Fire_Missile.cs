using UnityEngine;
using System.Collections;

public class Fire_Missile : MonoBehaviour {

    public GameObject missileToFire;
    public float coolDown = 0.5f;
    public string inputAxis = "Fire1";

    private float lastFireTime;
    private Vector3 cursorPosition;

	// Use this for initialization
	void Start () {
        lastFireTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown(inputAxis) && Time.time - lastFireTime > coolDown)
        {
            fireMissile();
            lastFireTime = Time.time;
        }
	}

    void setCursorPosition(Vector3 position) // Receives message from TrackCursor
    {
        cursorPosition = position;
    }

    void fireMissile()
    {
        Debug.Log("Firing!");
        GameObject newMissile = Instantiate(missileToFire, transform.position, transform.rotation) as GameObject;
        newMissile.GetComponent<Missile_Movement_Friendly>().setTargetVector(cursorPosition);
    }
}
