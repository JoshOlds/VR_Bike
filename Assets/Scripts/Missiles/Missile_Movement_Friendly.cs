using UnityEngine;
using System.Collections;

public class Missile_Movement_Friendly : MonoBehaviour {

    public float maxSpeed;
    public float thrust;
    public float maxTurnSpeed;

    private Rigidbody rb;
    private GameObject target;
    private Vector3 targetVector;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * (0.5f * maxSpeed);
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * thrust);
        clampSpeed();
        turnToTarget();
    }

    void turnToTarget()
    {
        if (target != null)
        {
            Vector3 targetDir = target.transform.position - transform.position;
            float step = maxTurnSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
        else if( targetVector != null)
        {
            Vector3 targetDir = targetVector - transform.position;
            float step = maxTurnSpeed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    void clampSpeed()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public void setTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    public void setTargetVector(Vector3 newTarget)
    {
        targetVector = newTarget;
    }
}
