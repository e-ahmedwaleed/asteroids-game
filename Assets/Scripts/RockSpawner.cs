using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
public class RockSpawner : MonoBehaviour
{
    // spawn control
    private const float SpawnDelay = 1;

    // spawn location support
    // save spawn boundaries for efficiency
    private static readonly float
        SpawnBorderSize = 100,
        MinSpawnX = SpawnBorderSize,
        MaxSpawnX = Screen.width - SpawnBorderSize,
        MinSpawnY = SpawnBorderSize,
        MaxSpawnY = Screen.height - SpawnBorderSize;

    // saved for efficiency
    private Camera _main;
    private Timer _spawnTimer;

    // needed for spawning
    [SerializeField] private GameObject rockPrefap = null;

    // saved for efficiency
    [SerializeField] private Sprite rockSprite0 = null;

    [SerializeField] private Sprite rockSprite1 = null;

    [SerializeField] private Sprite rockSprite2 = null;

    // Start is called before the first frame update
    private void Start()
    {
        _main = Camera.main;
        // create and start timer
        _spawnTimer = gameObject.AddComponent<Timer>();
        _spawnTimer.Duration = SpawnDelay;
        _spawnTimer.Run();
    }

    // Update is called once per frame
    private void Update()
    {
        // check for time to spawn a new rock
        if (!_spawnTimer.Finished) return;
            SpawnRock();

            // change spawn timer duration and restart
            _spawnTimer.Run();
        
    }

    void SpawnRock()
    {
        // generate random location and create new rock
        var location = new Vector3(
            Random.Range(MinSpawnX, MaxSpawnX),
            Random.Range(MinSpawnY, MaxSpawnY),
            -_main.transform.position.z);
        
        var worldLocation = _main.ScreenToWorldPoint(location);

        var numOfRocks = GameObject.FindGameObjectsWithTag("Rock").Length;
        if(numOfRocks>=3) return;
        
        var rock = Instantiate(rockPrefap);
        rock.transform.position = worldLocation;

        var spriteRenderer = rock.GetComponent<SpriteRenderer>();

        var spriteNumber = Random.Range(0, 3);
        switch (spriteNumber)
        {
            case 0:
                spriteRenderer.sprite = rockSprite0;
                break;
            case 1:
                spriteRenderer.sprite = rockSprite1;
                break;
            default:
                spriteRenderer.sprite = rockSprite2;
                break;
        }

        
    }
}