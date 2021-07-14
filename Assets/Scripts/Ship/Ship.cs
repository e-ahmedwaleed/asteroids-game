using System;
using UnityEngine;
using Utils;

namespace Ship
{
    public class Ship : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
    
        // Drive the ship
        private Vector2 _direction;
        private static readonly float ThrustForce = 5f;

        // Make the ship wrap
        private float _colliderRadius;
    
        // Rotate the ship
        private static readonly float RotateDegreesPerSecond = 180f;

        /// <summary>
        /// Use this for initialization
        /// </summary>
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _direction = Vector2.right;

            _colliderRadius = GetComponent<CircleCollider2D>().radius;
        }

        /// <summary>
        /// FixedUpdate is called 50 times per second
        /// </summary>
        private void FixedUpdate()
        {
            var axis = Input.GetAxis("Thrust") + Input.GetAxis("Space Thrust");
            if (axis > 0)
                _rigidbody.AddForce(ThrustForce * _direction, ForceMode2D.Force);
            else if (axis < 0)
                _rigidbody.AddForce(-0.8f * ThrustForce * _direction, ForceMode2D.Force);
        }

        private void ShipWrap()
        {
            var position = transform.position;

            if (position.x + _colliderRadius < ScreenUtils.ScreenLeft)
                position.x = ScreenUtils.ScreenRight + _colliderRadius;
            else if (position.x - _colliderRadius > ScreenUtils.ScreenRight)
                position.x = ScreenUtils.ScreenLeft - _colliderRadius;

            if (position.y + _colliderRadius < ScreenUtils.ScreenBottom)
                position.y = ScreenUtils.ScreenTop + _colliderRadius;
            else if (position.y - _colliderRadius > ScreenUtils.ScreenTop)
                position.y = ScreenUtils.ScreenBottom - _colliderRadius;

            transform.position = position;
        }
    
        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            ShipWrap();
            
            var rotationInput = Input.GetAxis("Rotate");
            if (rotationInput.CompareTo(0) == 0) return;

            // calculate rotation amount and apply rotation
            var rotationAmount = RotateDegreesPerSecond * Time.deltaTime;
            if (rotationInput < 0) rotationAmount *= -1;
            transform.Rotate(Vector3.forward, rotationAmount);

            // Thrusting in correct direction 
            Vector3 dir = transform.eulerAngles;
            float angle = Mathf.Deg2Rad * dir.z;

            _direction.x = (float) Math.Cos(angle);
            _direction.y = (float) Math.Sin(angle);

            // Smother rotating
            if (_rigidbody.velocity == Vector2.zero) return; 
            _rigidbody.velocity = _rigidbody.velocity*0.9f;
            _rigidbody.angularVelocity = _rigidbody.angularVelocity*0.9f;
            _rigidbody.AddForce(ThrustForce * _direction, ForceMode2D.Force);
        }
    
    }
}