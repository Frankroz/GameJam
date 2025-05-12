using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject[] objects;
    public float minSpawnTime;
    public float maxSpawnTime;
    private float timeUntilSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetTimeUntilSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilSpawn -= Time.deltaTime;

        if (timeUntilSpawn <= 0)
        {
            int rand = Random.Range(0, objects.Length);
            Instantiate(objects[rand], transform.position, Quaternion.identity);
            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minSpawnTime, maxSpawnTime);
    }
}
