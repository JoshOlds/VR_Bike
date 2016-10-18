using UnityEngine;
using System.Collections;

public class Missile_Health : MonoBehaviour {

    public float maxHealth = 0.0f;
    
    private float currentHealth;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool applyDamage(float damage) //Returns true if dead
    {
        currentHealth = currentHealth - damage;
        if(currentHealth <= 0.0f)
        {
            die();
            return true;
        }
        return false;
    }

    void die()
    {
        gameObject.SendMessage("onDeath", SendMessageOptions.DontRequireReceiver);
    }
}
