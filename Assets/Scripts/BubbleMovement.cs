using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float fallSpeed = 0.2f;

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y < -5) // ³¬³öÆÁÄ»Ïú»Ù
        {
            Destroy(gameObject);
            
        }
    }
}
