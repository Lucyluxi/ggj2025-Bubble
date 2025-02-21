using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab; // 预制体
    public float spawnRate = 0.01f; // 生成间隔
    public float spawnRangeX = 4f; // X轴随机范围

    void Start()
    {
        InvokeRepeating("SpawnBubble", 1f, spawnRate);
    }

    void SpawnBubble()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, transform.position.y, 0);
        Instantiate(bubblePrefab, spawnPos, Quaternion.identity);
    }
}

