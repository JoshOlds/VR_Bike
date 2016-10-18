using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Waypoints.Assets
{
    public class Destroyable : MonoBehaviour
    {
        public float MaxHealth;
        public float Health;
        public GameObject Explosion;
        public bool Keep; 

        private Rigidbody _rb;

        public void Awake()
        {
            _rb = GetComponentInChildren<Rigidbody>();
        }

        public void TakeDamage(float damage, GameObject from)
        {
            Health -= damage;

            _rb.AddExplosionForce(damage, from.transform.position, damage);

            SendMessage("TookDamage", from, SendMessageOptions.DontRequireReceiver);

            if (Health > 0)
                return;

            Explode();
            SendMessage("Destroyed", from, SendMessageOptions.DontRequireReceiver);
        }

        private void Explode()
        {
            IEnumerable<ParticleSystem> systems = Explosion.GetComponentsInChildren<ParticleSystem>();
            foreach (var system in systems)
            {
                Instantiate(system);
                system.Play();
            }
            if (!Keep) {
                Destroy(gameObject, 1);
            }
        }
    }
}