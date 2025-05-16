using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject[] objects;
    public float minSpawnTime = 3f; // Initial minimum spawn time
    public float maxSpawnTime = 5f; // Initial maximum spawn time
    public float spawnRateIncreaseInterval = 10f; // Time interval in seconds to increase spawn rate
    public float spawnRateDecreaseFactor = 0.9f; // Factor to decrease min and max spawn time (e.g., 0.9 makes it 10% faster)
    private float timeUntilSpawn;
    private float timer;

    public LogicScript logicScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetTimeUntilSpawn();
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (logicScript.isGamePaused)
        {
            return;
        }
        timeUntilSpawn -= Time.deltaTime;
        timer += Time.deltaTime;

        if (timeUntilSpawn <= 0)
        {
            int rand = Random.Range(0, objects.Length);
            Instantiate(objects[rand], transform.position, Quaternion.identity);
            SetTimeUntilSpawn();
        }

        // Increase spawn rate over time
        if (timer >= spawnRateIncreaseInterval)
        {
            IncreaseSpawnRate();
            timer = 0f;
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void IncreaseSpawnRate()
    {
        minSpawnTime *= spawnRateDecreaseFactor;
        maxSpawnTime *= spawnRateDecreaseFactor;

        // Optional: Add a minimum clamp to prevent spawn times from becoming zero or negative
        minSpawnTime = Mathf.Max(minSpawnTime, 0.1f);
        maxSpawnTime = Mathf.Max(maxSpawnTime, 0.2f);
    }
}