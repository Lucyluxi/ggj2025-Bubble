using UnityEngine;

public class BubbleMovement : MonoBehaviour
{
    public float speed = 1.1f; // 泡泡水平移动速度
    public float floatStrength = 0.1f; // 上下浮动幅度
    public float floatSpeed = 2f; // 上下浮动速度

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // 水平持续向右移动
        transform.position += Vector3.right * speed * Time.deltaTime;

        // 上下浮动效果
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatStrength;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    // 检测碰撞（确保泡泡的 Collider2D 设置为 IsTrigger）
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // 确保玩家有“Player”标签
        {
            Debug.Log("Player hit the bubble!");
            Destroy(gameObject);  // 销毁泡泡
        }
    }
}
