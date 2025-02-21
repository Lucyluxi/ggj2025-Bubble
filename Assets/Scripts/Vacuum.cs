using UnityEngine;
using System.Collections;
using TMPro;

public class Vacuum : MonoBehaviour
{
    public float suctionRadius = 0.5f;
    public float suctionSpeed = 5f;
    public KeyCode suctionKey = KeyCode.Space;

    public float strongSuctionRadius = 1f;
    public float strongSuctionSpeed = 10f;
    public KeyCode strongSuctionKey = KeyCode.Return;

    public float strongAttackDuration = 1.5f;

    public int score = 0;
    public TMP_Text scoreText;

    void Update()
    {
        UpdateScoreUI();

        if (Input.GetKeyDown(strongSuctionKey))
        {
            StartCoroutine(StrongSuctionCoroutine()); // 开始强吸附
        }
        else if (Input.GetKey(suctionKey))
        {
            SuctionBubbles(suctionRadius, suctionSpeed);
        }
    }

    void SuctionBubbles(float radius, float speed)
    {
        Collider2D[] bubbles = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D bubble in bubbles)
        {
            if (bubble.CompareTag("Bubble"))
            {
                bubble.transform.position = Vector3.Lerp(bubble.transform.position, transform.position, speed * Time.deltaTime);
                if (Vector3.Distance(bubble.transform.position, transform.position) < 0.1f)
                {
                    Destroy(bubble.gameObject); // 吸收泡泡
                    score++;
                    UpdateScoreUI();
                }
            }
        }
    }

    void UpdateScoreUI()
    {
        if(scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    IEnumerator StrongSuctionCoroutine()
    {
        float duration = strongAttackDuration;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            SuctionBubbles(suctionRadius * 1.5f, suctionSpeed * 2); // 持续执行吸附
            elapsedTime += Time.deltaTime;
            yield return null; // 等待下一帧继续运行
        }
    }
}

