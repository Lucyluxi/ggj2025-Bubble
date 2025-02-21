using UnityEngine;

public class Meteor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ����Ƿ���ײ����ɫ
        if (collision.CompareTag("Player"))
        {
            // ��������
            Destroy(gameObject);

            // ���ý�ɫ�������߼�
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(); // ������ɫ�������߼�
            }
        }
    }
}
