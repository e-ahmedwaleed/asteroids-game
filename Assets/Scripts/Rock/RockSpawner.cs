using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Utils;

namespace Rock
{
    [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
    public class RockSpawner : MonoBehaviour
    {
        // spawn control
        private const float SpawnDelay = 1;

        // collision-free spawn support
        private Vector2 _max;
        private Vector2 _min;
        private float _colliderHalfMaxWidth;
        private float _colliderHalfMaxHeight;

        // saved for efficiency
        private Camera _main;
        private Timer _spawnTimer;

        // needed for spawning
        [SerializeField] private GameObject rockPrefap = null;

        // saved for efficiency
        [SerializeField] private Sprite[] rockSprites = new Sprite[3];

        // Start is called before the first frame update
        private void Start()
        {
            _main = Camera.main;
            // create and start timer
            _spawnTimer = gameObject.AddComponent<Timer>();
            _spawnTimer.Duration = SpawnDelay;
            _spawnTimer.Run();

            // spawn and destroy a rock to cache collider values
            var tempRock = Instantiate(rockPrefap);
            var colliderSize = tempRock.GetComponent<CapsuleCollider2D>().size;
            _colliderHalfMaxWidth = colliderSize.x;
            _colliderHalfMaxHeight = colliderSize.y;
            Destroy(tempRock);
        }

        // Update is called once per frame
        private void Update()
        {
            // check for time to spawn a new rock
            if (!_spawnTimer.Finished) return;
            SpawnRock();

            // restart spawn timer
            _spawnTimer.Run();
        }

        private void SpawnRock()
        {
            // generate random location and create new rock
            var worldLocation = new Vector3(
                Random.Range(ScreenUtils.ScreenLeft, ScreenUtils.ScreenRight),
                Random.Range(ScreenUtils.ScreenTop/1.5f, ScreenUtils.ScreenTop),
                //Random.Range(ScreenUtils.ScreenBottom, ScreenUtils.ScreenTop),
                0);
            SetMinAndMax(worldLocation);

            while (!(Physics2D.OverlapArea(_min, _max) is null))
            {
                // change location and calculate new rectangle points
                worldLocation = new Vector3(
                    Random.Range(ScreenUtils.ScreenLeft, ScreenUtils.ScreenRight),
                    Random.Range(ScreenUtils.ScreenTop/2f, ScreenUtils.ScreenTop),
                    //Random.Range(ScreenUtils.ScreenBottom, ScreenUtils.ScreenTop),
                    0);
                SetMinAndMax(worldLocation);
            }

            var rock = Instantiate(rockPrefap);
            rock.transform.position = worldLocation;

            var spriteRenderer = rock.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = rockSprites[Random.Range(0, 3)];
        }

        /// <summary>
        ///     Sets min and max for a rock collision rectangle
        /// </summary>
        /// <param name="location">location of the teddy bear</param>
        private void SetMinAndMax(Vector3 location)
        {
            _min.x = location.x - _colliderHalfMaxWidth;
            _min.y = location.y - _colliderHalfMaxHeight;
            _max.x = location.x + _colliderHalfMaxWidth;
            _max.y = location.y + _colliderHalfMaxHeight;
        }
    }
}