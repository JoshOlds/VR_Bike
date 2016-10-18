using UnityEngine;
using System.Collections;

public class Missile_Target_Select : MonoBehaviour {

    public GameObject[] targets;
    public bool autoTarget;
    private GameObject currentTarget;
    private bool debug = true;

    // Use this for initialization
    void Awake () {
        if (autoTarget)
        {
            targets = GameObject.FindGameObjectsWithTag("Target");
        }
        int index = Random.Range(0, targets.Length);
        currentTarget = targets[index];
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public GameObject getTarget()
    {
        if (debug) { Debug.Log("Missile Target Selector:" + currentTarget.name); }
        return currentTarget;
    }

}
