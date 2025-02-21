using UnityEngine;
using System.Collections;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab; // ���ǵ�Ԥ����
    public float minSpawnInterval = 0.5f; // ������ɼ��ʱ��
    public float maxSpawnInterval = 1.5f; // ����ɼ��ʱ��
    public float moveSpeed = 10.0f; // ���ǵ�ˮƽ�ƶ��ٶ�

    public int maxMeteorsOnScreen = 3; // ͬʱ���ڵ������������

    private Camera mainCamera; // �������
    private int currentMeteorCount = 0; // ��ǰ���ڵ���������

    private void Start()
    {
        // ��ȡ�������
        mainCamera = Camera.main;

        // ��ʼ��������
        StartCoroutine(SpawnMeteorRoutine());
    }

    private IEnumerator SpawnMeteorRoutine()
    {
        while (true)
        {
            // ������ɼ��ʱ��
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);

            // �����ǰ������������������ƣ�����������
            if (currentMeteorCount < maxMeteorsOnScreen)
            {
                SpawnMeteor();
            }

            // �ȴ�һ��ʱ��������
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnMeteor()
    {
        // ��̬��������Ŀ��ӷ�Χ
        Vector2 spawnPosition = GetRandomSpawnPosition();

        // �������Ƕ���
        GameObject meteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);

        // ��������Ϊ����
        meteor.transform.rotation = Quaternion.Euler(0, 0, -90); // ��ת 90 ��

        // �������ǵ�ˮƽ�����ٶ�
        Rigidbody2D rb = meteor.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(-moveSpeed, 0); // ˮƽ�����ƶ�
        }

        // ���ӵ�ǰ���Ǽ���
        currentMeteorCount++;

        // �Զ��������ǲ����ټ���
        Destroy(meteor, 5.0f);
        StartCoroutine(RemoveMeteorAfterDelay(5.0f)); // ȷ����������ͬ��
    }

    private IEnumerator RemoveMeteorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentMeteorCount--; // �������Ǽ���
    }

    private Vector2 GetRandomSpawnPosition()
    {
        // ��ȡ����ı߽磨�������꣩
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // ������ұ���������
        float spawnX = mainCamera.transform.position.x + cameraWidth / 2; // ����ұ�
        float spawnY = Random.Range(mainCamera.transform.position.y - cameraHeight / 2, mainCamera.transform.position.y + cameraHeight / 2); // ������������·�Χ

        return new Vector2(spawnX, spawnY);
    }
}
