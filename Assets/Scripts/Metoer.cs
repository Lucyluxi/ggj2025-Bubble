using UnityEngine;

public class Meteor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检测是否碰撞到角色
        if (collision.CompareTag("Player"))
        {
            // 销毁流星
            Destroy(gameObject);

            // 调用角色的受伤逻辑
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(); // 触发角色的受伤逻辑
            }
        }
    }
}
