using System;
using System.Collections.Generic;
using UnityEngine;

namespace Waypoints.Assets
{
    public class PlayerController
    {
        private readonly Bike _bike;
        private float
            _baseVelocity,
            _targetVelocity,
            _variableVelocity;

        public float CurrentVelocity { get; private set; }

        public float MaxVariableVelocity { get; set; }
        public float MinimumVelocity { get; set; }

        public float Acceleration { get; set; }

        public float VelocityDamp { get; set; }

        public float RotationSpeed { get; set; }

        public float AfterBurnerModifier { get; set; }

        public float StrafeModifier { get; set; }

        public PlayerController(Bike bike)
        {
            MaxVariableVelocity = 20;
            Acceleration = 70;
            VelocityDamp = 20;
            RotationSpeed = 0.3f;
            AfterBurnerModifier = 50;
            StrafeModifier = 30;
            _bike = bike;
        }

        public void Update()
        {
            UpdatePosition();
            UpdateRotation();
        }

        private void UpdatePosition()
        {
            _variableVelocity = Mathf.Clamp(_variableVelocity + Input.GetAxis("Vertical") * Time.deltaTime * Acceleration,
                0,
                MaxVariableVelocity);

            _targetVelocity = _variableVelocity + MinimumVelocity;

            if (Input.GetKey(KeyCode.B))
            {
                _targetVelocity += AfterBurnerModifier;
            }

            CurrentVelocity = Mathf.Lerp(CurrentVelocity, _targetVelocity, Time.deltaTime * VelocityDamp);
         
            _bike.transform.Translate(
                Input.GetAxis("Horizontal") * Time.deltaTime * StrafeModifier,
                0,
                CurrentVelocity * Time.deltaTime,
                Space.Self);
        }

        private void UpdateRotation()
        {
            if (Input.GetKey("e"))
            {
                _bike.transform.Rotate(0, 0, -90f * Time.deltaTime);
            }

            if (Input.GetKey("q"))
            {
                _bike.transform.Rotate(0, 0, 90f * Time.deltaTime);
            }
        }
    }
}