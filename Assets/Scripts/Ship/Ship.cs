using System;
using Audio;
using UnityEngine;
using Utils;

namespace Ship
{
    public class Ship : MonoBehaviour
    {
        private static readonly float MaxSpeed = 10f;
        private static readonly float ThrustForce = 0.25f;

        // Drive the ship
        private Camera _cameraMain;
        private Rigidbody2D _rigidBody;

        public float WeaponWeight { get; set; }

        [SerializeField] private GameObject explosionPrefap = null;
        
        /// <summary>
        ///     Use this for initialization
        /// </summary>
        private void Start()
        {
            WeaponWeight = 1f;
            _cameraMain = Camera.main;

            //https://answers.unity.com/questions/38542/prevent-rigidbody-from-rotating.html
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.freezeRotation = true;
        }

        private void Update()
        {
            var yAxis = Input.GetAxis("Vertical");
            var xAxis = Input.GetAxis("Horizontal");

            //Mouse processing
            if (GameDataUtils.MouseEnabled)
            {
                // convert mouse position to world position
                var position = Input.mousePosition;

                position.z = -_cameraMain.transform.position.z;
                position = _cameraMain.ScreenToWorldPoint(position);

                // move character to mouse position and clamp in screen
                xAxis = position.x - transform.position.x;
            }

            if (xAxis > 0)
                _rigidBody.AddForce(ThrustForce * Vector2.right, ForceMode2D.Impulse);
            else if (xAxis < 0)
                _rigidBody.AddForce(ThrustForce * Vector2.left, ForceMode2D.Impulse);

            if (yAxis > 0)
                _rigidBody.AddForce(ThrustForce * Vector2.up, ForceMode2D.Impulse);
            else if (yAxis < 0)
                _rigidBody.AddForce(ThrustForce * Vector2.down, ForceMode2D.Impulse);

            Decelerate(xAxis, yAxis);
        }

        /// <summary>
        ///     Multiple calls to _rigidBody.velocity leads to misbehaviour.
        /// </summary>
        private void Decelerate(float x, float y)
        {
            var velocity = _rigidBody.velocity;

            if (velocity.magnitude > MaxSpeed / WeaponWeight)
                velocity = Normalize(velocity);

            if (x == 0) velocity = new Vector2(velocity.x * 0.8f, velocity.y);
            if (y == 0) velocity = new Vector2(velocity.x, velocity.y * 0.8f);

            _rigidBody.velocity = velocity;
        }

        private Vector2 Normalize(Vector2 velocity)
        {
            return MaxSpeed / WeaponWeight * velocity.normalized;
        }

        private void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.gameObject.CompareTag("Rock"))
            {
                AudioManager.Play(AudioClipName.Damage);
                GameDataUtils.CurrentScore -=
                    (int) Math.Floor(Math.Pow(coll.transform.localScale.x * (GameDataUtils.CurrentLevel + 1) * 2, 1.5));
            }

            if (GameDataUtils.CurrentLevel < 1 && GameDataUtils.CurrentScore < 0)
            {
                AudioManager.Play(AudioClipName.Explode);
                Instantiate(explosionPrefap, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}