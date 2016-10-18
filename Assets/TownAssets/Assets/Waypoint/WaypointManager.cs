using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Waypoints.Assets
{
    public class WaypointManager : MonoBehaviour
    {
        public Waypoint FirstWaypoint;

        private Waypoint _currentWaypoint;

        public void Start()
        {
            _currentWaypoint = FirstWaypoint;

            var waypoint = _currentWaypoint.Next;
            while (waypoint != null)
            {
                waypoint.gameObject.SetActive(false);
                waypoint = waypoint.Next;
            }
        }

        public void PlayerHitWaypoint(Waypoint waypoint)
        {
            waypoint.Deactivate();

            _currentWaypoint = waypoint.Next;

            if (_currentWaypoint == null )
                return;
            
            _currentWaypoint.gameObject.SetActive(true);
        }
    }
}