using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab; // 流星的预制体
    public float spawnInterval = 2.0f; // 生成间隔时间
    public float moveSpeed = 5.0f; // 流星的水平移动速度

    public int minMeteors = 1; // 每次最少生成的流星数量
    public int maxMeteors = 5; // 每次最多生成的流星数量

    private Camera mainCamera; // 主摄像机

    private void Start()
    {
        // 检查是否绑定了 MeteorPrefab
        if (meteorPrefab == null)
        {
            Debug.LogError("Meteor Prefab is not assigned in the Inspector! Please assign a valid prefab.");
            enabled = false; // 禁用脚本，防止后续错误
            return;
        }

        // 获取主摄像机
        mainCamera = Camera.main;

        // 定时调用生成流星的方法
        InvokeRepeating(nameof(SpawnMeteors), 1.0f, spawnInterval);
    }

    void SpawnMeteors()
    {
        // 随机决定本次生成的流星数量
        int meteorCount = Random.Range(minMeteors, maxMeteors + 1);

        for (int i = 0; i < meteorCount; i++)
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

            // 自动销毁流星以防资源泄露
            Destroy(meteor, 5.0f); // 5 秒后销毁
        }
    }

    Vector2 GetRandomSpawnPosition()
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
