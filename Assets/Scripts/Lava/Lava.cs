using Ship;
using UnityEngine;

namespace Lava
{
    public class Lava : MonoBehaviour
    {
        private LavaThrower _shipLavaThrower;

        private void Start()
        {
            var spaceShip = GameObject.FindGameObjectWithTag("Space Ship");
            _shipLavaThrower = spaceShip.GetComponent<LavaThrower>();

            var boxCollider2D = GetComponent<BoxCollider2D>();
            Physics2D.IgnoreCollision(spaceShip.GetComponent<PolygonCollider2D>(), boxCollider2D);
            
            EdgeCollider2D[] cameraColliders = Camera.main.GetComponents<EdgeCollider2D>();
            foreach(EdgeCollider2D coll in cameraColliders)
            {
                Physics2D.IgnoreCollision(coll, boxCollider2D);
            }
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D coll)
        {
            DestroyAndNotifyShip();
        }

        private void DestroyAndNotifyShip()
        {
            _shipLavaThrower.HitSomething(gameObject);
            Destroy(gameObject);
        }
    }
}