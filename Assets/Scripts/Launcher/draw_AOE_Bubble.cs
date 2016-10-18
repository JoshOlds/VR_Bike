using UnityEngine;
using System.Collections;

public class draw_AOE_Bubble : MonoBehaviour {

    public Material transparentMaterial;
    public GameObject explosiveObject;

    private Vector3 cursorPosition = new Vector3(0,0,0);
    private GameObject bubble;
    private float AOESize = 0;

	// Use this for initialization
	void Start () {
        setAOESize();
	}
	
	// Update is called once per frame
	void Update () {
	    if(bubble == null)
        {
            bubble = createBubble();
            bubble.transform.localScale = new Vector3(AOESize, AOESize, AOESize);
        }
        bubble.transform.position = cursorPosition;
        bubble.transform.localScale = new Vector3(AOESize, AOESize, AOESize);

    }

    GameObject createBubble()
    {
        GameObject newBubble = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(newBubble.GetComponent<SphereCollider>());
        MeshRenderer mesh = newBubble.GetComponent<MeshRenderer>();
        mesh.material = transparentMaterial;
        return newBubble;
    }

    public void setAOESize()
    {
        Explosive explosive = explosiveObject.GetComponent<Explosive>();
        AOESize = explosive.getExplosionRadius();
    }

    public void setCursorPosition(Vector3 position)
    {
        cursorPosition = position;
    }
}
