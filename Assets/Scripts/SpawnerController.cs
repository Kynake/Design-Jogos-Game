using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public List<GameObject> normalAsteroidPrefabs;
    public GameObject flamingAsteroidPrefab;
    public GameObject targetCamera;
    public float asteroidLifetime;

    // Asteroid properties (min & max);
    public Vector2 spawnRate;
    public Vector2 size;
    public Vector2 speed;
    public Vector2 rotation;

    public float flamingAsteroidChance;

    private List<ObjectPool> _asteroidPools;
    private ObjectPool _flamingAsteroidPool;
    // private Renderer rend;
    private Camera cam;

    private void Start()
    {
        if(_asteroidPools == null)
        {
            _asteroidPools = new List<ObjectPool>(normalAsteroidPrefabs.Count);
            normalAsteroidPrefabs.ForEach(prefab => _asteroidPools.Add(new ObjectPool(prefab)));
        }

        _flamingAsteroidPool = new ObjectPool(flamingAsteroidPrefab, 5);


        // rend = GetComponent<Renderer>();
        cam = targetCamera.GetComponent<Camera>();

        StartCoroutine(ScheduleAsteroids());
    }

    private IEnumerator ScheduleAsteroids()
    {
        while(gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(Random.Range(spawnRate[0], spawnRate[1]));

            // Don't spawn if visible
            // if(rend.isVisible)
            // {
            //     continue;
            // }

            GameObject asteroid;

            if(Random.value < flamingAsteroidChance) {
                asteroid = _flamingAsteroidPool.getPooledGameObject();
            } else {
                asteroid = _asteroidPools[Random.Range(0, _asteroidPools.Count)].getPooledGameObject();
            }


            if(asteroid != null)
            {
                SpawnAsteroid(asteroid);
            }

        }
    }

    private void SpawnAsteroid(GameObject asteroid) {
        asteroid.transform.position = transform.position;
        var script = asteroid.GetComponent<AsteroidController>();
        script.lifetime = asteroidLifetime;
        asteroid.SetActive(true);

        // Assign random scale to spawned asteroid
        var randomSize = Random.Range(size[0], size[1]);
        asteroid.transform.localScale = new Vector3(randomSize, randomSize, randomSize);

        var body = asteroid.GetComponent<Rigidbody2D>();

        // Add Velocity
        var heading = GetSpotOnView() - asteroid.transform.position;
        body.AddForce(heading * Random.Range(speed[0], speed[1]) * body.mass, ForceMode2D.Impulse);

        // Add Rotation
        var clockwiseRotation = Random.value < 0.5? -1 : 1;
        body.AddTorque(Random.Range(rotation[0], rotation[1]) * clockwiseRotation);
    }

    private Vector3 GetSpotOnView()
    {
        var randomSpot = new Vector2(Random.value, Random.value);
        return cam.ViewportToWorldPoint(randomSpot);
    }
}
