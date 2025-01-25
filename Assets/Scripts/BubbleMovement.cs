using UnityEngine;

public class BubbleMovement : MonoBehaviour
{
    public float speed = 1.1f; // ����ˮƽ�ƶ��ٶ�
    public float floatStrength = 0.1f; // ���¸�������
    public float floatSpeed = 2f; // ���¸����ٶ�

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // ˮƽ���������ƶ�
        transform.position += Vector3.right * speed * Time.deltaTime;

        // ���¸���Ч��
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatStrength;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    // �����ײ��ȷ�����ݵ� Collider2D ����Ϊ IsTrigger��
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // ȷ������С�Player����ǩ
        {
            Debug.Log("Player hit the bubble!");
            Destroy(gameObject);  // ��������
        }
    }
}
