using Ship;
using UnityEngine;

namespace Lava
{
    public class Lava : MonoBehaviour
    {
        private LavaThrower _shipLavaThrower;

        private float timer;

        private void Start()
        {
            _shipLavaThrower = GameObject.FindGameObjectWithTag("Space Ship").GetComponent<LavaThrower>();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer < 1) return;
            _shipLavaThrower.HitSomething(gameObject);
            Destroy(gameObject);
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        private void OnCollisionStay2D(Collision2D coll)
        {
            _shipLavaThrower.HitSomething(gameObject);
            Destroy(gameObject);
        }
    }
}