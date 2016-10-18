using System;
using System.Collections.Generic;
using UnityEngine;

namespace Waypoints.Assets
{
    public class Bike : MonoBehaviour
    {
        public Destroyable Destroyable;

        public Camera Camera;

        private PlayerCamera _camera;
        private PlayerController _controller;
        public float Health { get { return Destroyable.Health; } }
        public float MaxHealth { get { return Destroyable.MaxHealth; } }

        public IEnumerable<ParticleSystem> JetStreams;

        public void Awake()
        {
            _camera = new PlayerCamera(this, Camera);
            _controller = new PlayerController(this);
        }
        public void Update()
        {
            _controller.Update();
            _camera.Update();
        }

    }
}
