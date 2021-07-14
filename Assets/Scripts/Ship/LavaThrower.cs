using System;
using UnityEngine;

namespace Ship
{
    public class LavaThrower : MonoBehaviour
    {
        private bool _iAmOnFire;
        private bool _leftDown;
        private bool _rightDown;
        
        private GameObject _leftGun;
        private GameObject _rightGun;

        [SerializeField] private GameObject lavaPrefap = null;

        // Start is called before the first frame update
        private void Start()
        {
            _iAmOnFire = false;
            _leftDown = true;
            _rightDown = true;
        }

        // Update is called once per frame
        private void Update()
        {
            var fire = Input.GetAxis("Fire Lava");
            if (fire > 0)
            {
                if (_iAmOnFire)
                {
                    MakeItLonger();
                    return;
                }

                _iAmOnFire = true;
                ThrowLava();
            }
            else
            {
                _iAmOnFire = false;
                CutItOut();
                _leftDown = true;
                _rightDown = true;
            }
        }

        private void ThrowLava()
        {
            var shipCenter = gameObject.transform.position;

            var leftLavaPosition = new Vector3(shipCenter.x - .4f, shipCenter.y + 0.6f, shipCenter.z);
            var rightLavaPosition = new Vector3(shipCenter.x + .4f, shipCenter.y + 0.6f, shipCenter.z);

            if (_leftDown)
            {
                _leftGun = Instantiate(lavaPrefap,
                    leftLavaPosition, Quaternion.identity);
                _leftDown = false;
            }

            if (_rightDown)
            {
                _rightGun = Instantiate(lavaPrefap,
                    rightLavaPosition, Quaternion.identity);
                _rightDown = false;
            }

            _leftGun.transform.localScale = new Vector3(0.5f, 0.05f, 1f);
            _rightGun.transform.localScale = new Vector3(0.5f, 0.05f, 1f);
        }

        private void MakeItLonger()
        {
            var left = _leftGun.transform;
            var right = _rightGun.transform;
            var shipCenter = gameObject.transform.position;

            var lavaScale = left.localScale;

            if (lavaScale.magnitude < Math.Sqrt(2.8))
            {
                lavaScale.x += 0.025f;
                lavaScale.y += 0.05f;
            }
            else
            {
                ActivateBoxColliders();
            }

            left.localScale = lavaScale;
            right.localScale = lavaScale;

            var yShift = lavaScale.y * 3.75f;

            var leftLavaPosition = new Vector3(shipCenter.x - .4f, shipCenter.y + yShift, shipCenter.z);
            var rightLavaPosition = new Vector3(shipCenter.x + .4f, shipCenter.y + yShift, shipCenter.z);

            left.position = leftLavaPosition;
            right.position = rightLavaPosition;
        }

        private void CutItOut()
        {
            if (_leftDown || _rightDown) return;
            if (_leftGun is null || _rightGun is null) return;
            
            var left = _leftGun.GetComponent<Rigidbody2D>();
            var right = _rightGun.GetComponent<Rigidbody2D>();

            left.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
            right.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
            
            left.AddForce(15f * Vector2.up, ForceMode2D.Impulse);
            right.AddForce(15f * Vector2.up, ForceMode2D.Impulse);

            _leftGun = null;
            _rightGun = null;
        }

        private void ActivateBoxColliders()
        {
            _leftGun.GetComponent<BoxCollider2D>().isTrigger = false;
            _rightGun.GetComponent<BoxCollider2D>().isTrigger = false;
        }

        public void HitSomething(GameObject lava)
        {
            if (lava == _leftGun)
            {
                _leftDown = true;
                _iAmOnFire = false;
            }
            
            if (lava == _rightGun)
            {
                _rightDown = true;
                _iAmOnFire = false;
            }
            
        }
    }
}