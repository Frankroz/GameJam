using UnityEngine;

public class LevelSpawn : MonoBehaviour
{
    public GameObject[] objects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int rand = Random.Range(0, objects.Length);
        Instantiate(objects[rand], transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
