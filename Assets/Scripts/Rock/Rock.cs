using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rock
{
    public class Rock : MonoBehaviour
    {
        private static GameObject _spaceShip;

        //GameOver Fields
        private static int _gameOverRockCount;
        private static readonly GameObject[] LastRocks = new GameObject[8];

        // Customizing initial force
        private Rigidbody2D _rigidBody;
        private Vector2 _spaceShipPosition;

        // Start is called before the first frame update
        private void Start()
        {
            _spaceShip = GameObject.FindWithTag("Space Ship");

            if (_spaceShip is null)
            {
                GameOver();
                return;
            }

            _spaceShipPosition = _spaceShip.transform.position;
            _rigidBody = GetComponent<Rigidbody2D>();

            var forceAccordingToSize = GenerateRandomSize();
            HitItHard(forceAccordingToSize);
        }

        private float GenerateRandomSize()
        {
            var size = transform.localScale;

            var scale = Random.Range(0.8f, 1.6f);
            size *= scale;

            transform.localScale = size;

            return (6 + Globals.CurrentLevel * 1.5f) / Mathf.Sqrt(scale);
        }

        private void HitItHard(float impulseForceMagnitude)
        {
            Vector2 position = transform.position;

            // calculate direction to Space ship
            var direction = new Vector2(
                _spaceShipPosition.x - position.x,
                _spaceShipPosition.y - position.y);
            direction.Normalize();

            _rigidBody.velocity = Vector2.zero;
            _rigidBody.AddForce(direction * (impulseForceMagnitude * Random.Range(0.8f, 1.2f)),
                ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D coll)
        {
            if (_gameOverRockCount > 7) return;

            if (coll.gameObject.CompareTag("Lava"))
                Globals.UpgradeScore += (int) Math.Floor(Math.Pow(transform.localScale.x * 2, 2));
            else if (!coll.gameObject.CompareTag("Space Ship"))
                Globals.UpgradeScore += (int) (transform.localScale.x * 2);

            Destroy(gameObject);
        }

        private void GameOver()
        {
            if (_gameOverRockCount == 0)
                Destroy(GameObject.FindWithTag("Finish"));

            LastRocks[_gameOverRockCount] = gameObject;
            _gameOverRockCount++;

            if (_gameOverRockCount > 7)
            {
                Camera.main.GetComponent<RockSpawner>().enabled = false;

                foreach (var rock in LastRocks)
                {
                    _rigidBody = rock.GetComponent<Rigidbody2D>();

                    var direction = new Vector2(
                        Random.Range(-1f, 1f),
                        Random.Range(-1f, 1f));
                    direction.Normalize();

                    _rigidBody.velocity = Vector2.zero;
                    _rigidBody.AddForce(direction * (15 * Random.Range(0.8f, 1.2f)),
                        ForceMode2D.Impulse);
                }
            }
        }
    }
}