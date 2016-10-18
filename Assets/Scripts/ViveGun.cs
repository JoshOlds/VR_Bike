using UnityEngine;
using System.Collections;

public class ViveGun : MonoBehaviour {

    public GameObject viveControllerObject;
    public LineRenderer lineRenderer;
    public GameObject particleOnShot;
    public float maxDistance;
    public float forceApplied;
    public bool StaticExplosion;
    public bool MissileFire;
    public bool drawLaser = true;
    public GameObject MissileToFire;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit rayHit;

        Vector3 gunOrigin = viveControllerObject.transform.position;
        Vector3 gunDirection = viveControllerObject.transform.forward;
        if (Physics.Raycast(gunOrigin, gunDirection, out rayHit))
        {
            
        };
        if (drawLaser)
        {
            lineRenderer.SetPosition(0, gunOrigin);
            lineRenderer.SetPosition(1, gunOrigin + (gunDirection * 50));
        }
        
    }

    public void shoot() //This method is called by messaging from TestController script
    {
        RaycastHit rayHit;

        Vector3 gunOrigin = viveControllerObject.transform.position;
        Vector3 gunDirection = viveControllerObject.transform.forward;
        if (Physics.Raycast(gunOrigin, gunDirection, out rayHit)) {

            if (StaticExplosion)
            {
                Instantiate(particleOnShot, rayHit.point, Quaternion.Euler(new Vector3(0, 0, 0)));
                Rigidbody bodyHit = rayHit.rigidbody;
                if (bodyHit != null)
                {
                    bodyHit.AddExplosionForce(forceApplied, rayHit.point, 10.0f, 0.0f);
                }
                Debug.Log(rayHit.point);
            }

            if (MissileFire)
            {
                GameObject newMissile = Instantiate(MissileToFire, gunOrigin + (gunDirection * 2),viveControllerObject.transform.rotation) as GameObject;
                newMissile.GetComponent<Missile_Movement_Friendly>().setTargetVector(rayHit.point);
            }
        }
    }
}
