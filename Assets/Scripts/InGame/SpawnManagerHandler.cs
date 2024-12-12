using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SpawnManagerHandler : MonoBehaviour
{
    // Array to hold the game objects that will be spawned
    [SerializeField] private GameObject _monster;
    [SerializeField] private MapHandler _mapHandler;
    [SerializeField] private List<EnemyController> _enemyList = new List<EnemyController>();
    private Bounds _bounds;
    private Vector2 _spawnPosition;
    private bool _isPositionValid = false;

    // Time between spawns
    private float spawnInterval = 3f;
    // Start is called before the first frame update
    private void Start()
    {
        enemyListAddListener();
        // Start the spawning process
        if (!IsInvoking("SpawnRandomObject"))
        {
            InvokeRepeating("SpawnRandomObject", 5f, spawnInterval);
        }
        _bounds = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<TilemapRenderer>().bounds;

    }

    // Function to spawn a random object at a random position
    private void SpawnRandomObject()
    {
        GetValidSpawnPosition();
        
        if (_enemyList.Count >= _mapHandler.MaximumEnemyCount || !_isPositionValid)
        {
            return;
        }

        // Attempt to spawn at a valid position (not overlapping other objects)

        // Instantiate the object at the random position
        GameObject enemyObject = Instantiate(_monster, _spawnPosition, Quaternion.identity);
        EnemyController enemy = enemyObject.GetComponent<EnemyController>();
        enemy.OnDeathEvent.AddListener(CheckEnemyList);
        _enemyList.Add(enemy);
        _isPositionValid = false;
    }

    private void GetValidSpawnPosition()
    {
        float randomX = Random.Range(_bounds.min.x, _bounds.max.x);
        float randomY = Random.Range(_bounds.min.y, _bounds.max.y);
        _spawnPosition = new Vector2(randomX, randomY);

        // Check if there is any collider at the spawn position
        Collider2D hitCollider = Physics2D.OverlapCircle(_spawnPosition, 2f); // You can adjust the radius

        // If no collider is found, it's a valid spawn position
        if (hitCollider == null)
        {
            _isPositionValid = true;
        }
        else
        {
            _isPositionValid = false;
        }
    }

    private void enemyListAddListener()
    {
        foreach(EnemyController enemy in _enemyList)
        {
            enemy.OnDeathEvent.AddListener(CheckEnemyList);
        }
    }

    public void CheckEnemyList()
    {
        foreach(EnemyController enemy in _enemyList)
        {
            if(enemy.HP <= 0)
            {
                _enemyList.Remove(enemy);
                break;
            }
        }
    }
}
