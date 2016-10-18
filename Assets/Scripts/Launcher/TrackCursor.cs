using UnityEngine;
using System.Collections;

public class TrackCursor : MonoBehaviour
{

    // speed is the rate at which the object will rotate
    public float rotateSpeed;
    public Vector3 planePoint;
    public Vector3 planeNormal;
    public Camera controlCamera;

    private Vector3 lastTargetPoint = new Vector3(0,0,0);

    void FixedUpdate()
    {
        // Generate a plane that intersects the transform's position with an upwards normal.
        Plane targetPlane = new Plane(planeNormal, planePoint);

        // Generate a ray from the cursor position
        Ray ray = controlCamera.ScreenPointToRay(Input.mousePosition);

        // Determine the point where the cursor ray intersects the plane.
        // This will be the point that the object must look towards to be looking at the mouse.
        // Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
        //   then find the point along that ray that meets that distance.  This will be the point
        //   to look at.
        float hitdist = 0.0f;
        // If the ray is parallel to the plane, Raycast will return false.
        if (targetPlane.Raycast(ray, out hitdist))
        {
            // Get the point along the ray that hits the calculated distance.
            Vector3 targetPoint = ray.GetPoint(hitdist);
            lastTargetPoint = targetPoint;

            // Determine the target rotation.  This is the rotation if the transform looks at the target point.
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

            // Smoothly rotate towards the target point.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            gameObject.SendMessage("setCursorPosition", lastTargetPoint, SendMessageOptions.DontRequireReceiver); //Sends out messages of cursor position
        }
        
    }

    public Vector3 getTargetPoint()
    {
        return lastTargetPoint;
    }

}