using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab; // Ԥ����
    public float spawnRate = 0.01f; // ���ɼ��
    public float spawnRangeX = 4f; // X�������Χ

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

