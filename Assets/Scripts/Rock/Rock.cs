using UnityEngine;
using Utils;

namespace Rock
{
    public class Rock : MonoBehaviour
    {
        private static GameObject _spaceShip;

        // Customizing initial force
        private Rigidbody2D _rigidbody;
        private Vector2 _spaceShipPosition;
        
        // Make the ship wrap
        private Vector2 _colliderRadius;

        // Start is called before the first frame update
        private void Start()
        {
            _spaceShip = GameObject.FindGameObjectsWithTag("Space Ship")[0];
            
            _spaceShipPosition = _spaceShip.transform.position;
            _rigidbody = GetComponent<Rigidbody2D>();
            
            _colliderRadius = GetComponent<CapsuleCollider2D>().size;

            var forceAccordingToSize = GenerateRandomSize();
            HitItHard(forceAccordingToSize);
        }

        private float GenerateRandomSize()
        {
            var size = transform.localScale;

            var scale = Random.Range(0.5f, 2f);
            size *= scale;

            transform.localScale = size;

            return 8 / Mathf.Sqrt(scale);
        }

        private void HitItHard(float impulseForceMagnitude)
        {
            Vector2 position = transform.position;

            // calculate direction to Space ship
            var direction = new Vector2(
                _spaceShipPosition.x - position.x,
                _spaceShipPosition.y - position.y);
            direction.Normalize();

            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(direction * (impulseForceMagnitude * Random.Range(0.8f, 1.2f)),
                ForceMode2D.Impulse);
        }

        private void OnBecameInvisible()
        {
            /*var position = transform.position;

            // check left, right, top, and bottom sides
            if (position.x + _colliderRadius.x < ScreenUtils.ScreenLeft ||
                position.x - _colliderRadius.x > ScreenUtils.ScreenRight)
            {
                position.x *= -1;
            }
            if (position.y - _colliderRadius.y > ScreenUtils.ScreenTop ||
                position.y + _colliderRadius.y < ScreenUtils.ScreenBottom)
            {
                position.y *= -1;
            }
        
            transform.position = position;*/
            
            Destroy(gameObject);
        }
    }
}