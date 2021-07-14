using UnityEngine;

namespace Ship
{
    public class Ship : MonoBehaviour
    {
        private static readonly float MaxSpeed = 10f;
        private static readonly float ThrustForce = 0.25f;


        // Drive the ship
        private Rigidbody2D _rigidbody;

        /// <summary>
        ///     Use this for initialization
        /// </summary>
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            //https://answers.unity.com/questions/38542/prevent-rigidbody-from-rotating.html
            _rigidbody.freezeRotation = true;
        }

        private void Update()
        {
            var yAxis = Input.GetAxis("Vertical");
            var xAxis = Input.GetAxis("Horizontal");

            if (xAxis > 0)
                _rigidbody.AddForce(ThrustForce * Vector2.right, ForceMode2D.Impulse);
            else if (xAxis < 0)
                _rigidbody.AddForce(ThrustForce * Vector2.left, ForceMode2D.Impulse);

            if (yAxis > 0)
                _rigidbody.AddForce(ThrustForce * Vector2.up, ForceMode2D.Impulse);
            else if (yAxis < 0)
                _rigidbody.AddForce(ThrustForce * Vector2.down, ForceMode2D.Impulse);

            Decelerate(xAxis, yAxis);
        }

        /// <summary>
        ///     Multiple calls to _rigidbody.velocity leads to misbehaviour.
        /// </summary>
        private void Decelerate(float x, float y)
        {
            var velocity = _rigidbody.velocity;

            if (velocity.magnitude > MaxSpeed)
                velocity = Normalize(velocity);

            if (x == 0) velocity = new Vector2(velocity.x * 0.8f, velocity.y);
            if (y == 0) velocity = new Vector2(velocity.x, velocity.y * 0.8f);

            _rigidbody.velocity = velocity;
        }

        private Vector2 Normalize(Vector2 velocity)
        {
            return MaxSpeed * velocity.normalized;
        }
    }
}