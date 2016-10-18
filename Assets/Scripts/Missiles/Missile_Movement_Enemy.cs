using UnityEngine;
using System.Collections;

public class Missile_Movement_Enemy : MonoBehaviour {

    public float maxSpeed;
    public float thrust;
    public float maxTurnSpeed;
    public GameObject enemyController;
    public bool randomTarget;
    public bool lockZ = false;
    public int zDepth;

    
    private Rigidbody rb;
    private Target_Select_Generic selector;
    private GameObject target;


    // Use this for initialization
    void Awake()
    {
        
    }

    void Start () {
        rb = GetComponent<Rigidbody>();
        if(selector == null) { enemyController = findEnemyController(); }
        selector = enemyController.GetComponent<Target_Select_Generic>();
        getTarget();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        rb.AddForce(transform.forward * thrust);
        clampSpeed();
        turnToTarget();
        if (lockZ) { clampZ(zDepth); }
    }

    void turnToTarget()
    {
        if(target != null)
        {
            Vector3 targetDir = target.transform.position - transform.position;
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

    void getTarget()
    {
        if (randomTarget)
        {
            target = selector.getRandomTarget();
        }else
        {
            target = selector.getDistributedTarget();
        }
    }

    GameObject findEnemyController()
    {
        return GameObject.Find("EnemyController");
    }

    void clampZ(float zDepth)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, zDepth);
    }
}
