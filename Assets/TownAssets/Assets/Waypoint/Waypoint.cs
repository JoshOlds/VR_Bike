using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Waypoints.Assets
{
    public class Waypoint : MonoBehaviour
    {
        private WaypointManager _manager;
        private IEnumerable<Renderer> _renderers;

        public Waypoint Next;

        public float
            TimeModifier,
            MinimumVelocity;

        private float _alpha;
        private bool _deactivate;

        public void Start()
        {
            _manager = (WaypointManager) FindObjectOfType(typeof (WaypointManager));
            _alpha = 1;
            _renderers = GetComponentsInChildren<Renderer>();
        }

        public void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.GetComponent<Player>() == null)
                return;

            _manager.PlayerHitWaypoint(this);
        }

        public void Deactivate()
        {
            _deactivate = true;
        }

        public void Update()
        {
            if (! _deactivate)
               return;

            _alpha = Mathf.Lerp(GetComponent<Renderer>().material.color.a, 0, Time.deltaTime*5);

            foreach (var render in _renderers)
            {
                render.material.color = new Color(1,1,1, _alpha);
            }

            if (_alpha < 0.1f)
            {
                gameObject.SetActive(false);
            }
        }

    }
}