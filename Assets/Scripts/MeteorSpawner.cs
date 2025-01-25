using UnityEngine;
using System.Collections;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab; // 流星的预制体
    public float minSpawnInterval = 0.5f; // 最短生成间隔时间
    public float maxSpawnInterval = 1.5f; // 最长生成间隔时间
    public float moveSpeed = 10.0f; // 流星的水平移动速度

    public int maxMeteorsOnScreen = 3; // 同时存在的最大流星数量

    private Camera mainCamera; // 主摄像机
    private int currentMeteorCount = 0; // 当前存在的流星数量

    private void Start()
    {
        // 获取主摄像机
        mainCamera = Camera.main;

        // 开始生成流星
        StartCoroutine(SpawnMeteorRoutine());
    }

    private IEnumerator SpawnMeteorRoutine()
    {
        while (true)
        {
            // 随机生成间隔时间
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);

            // 如果当前流星数量少于最大限制，则生成流星
            if (currentMeteorCount < maxMeteorsOnScreen)
            {
                SpawnMeteor();
            }

            // 等待一段时间再生成
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnMeteor()
    {
        // 动态计算相机的可视范围
        Vector2 spawnPosition = GetRandomSpawnPosition();

        // 创建流星对象
        GameObject meteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);

        // 调整流星为横向
        meteor.transform.rotation = Quaternion.Euler(0, 0, -90); // 旋转 90 度

        // 设置流星的水平飞行速度
        Rigidbody2D rb = meteor.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(-moveSpeed, 0); // 水平向左移动
        }

        // 增加当前流星计数
        currentMeteorCount++;

        // 自动销毁流星并减少计数
        Destroy(meteor, 5.0f);
        StartCoroutine(RemoveMeteorAfterDelay(5.0f)); // 确保计数减少同步
    }

    private IEnumerator RemoveMeteorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentMeteorCount--; // 减少流星计数
    }

    private Vector2 GetRandomSpawnPosition()
    {
        // 获取相机的边界（世界坐标）
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // 在相机右边生成流星
        float spawnX = mainCamera.transform.position.x + cameraWidth / 2; // 相机右边
        float spawnY = Random.Range(mainCamera.transform.position.y - cameraHeight / 2, mainCamera.transform.position.y + cameraHeight / 2); // 随机生成在上下范围

        return new Vector2(spawnX, spawnY);
    }
}
