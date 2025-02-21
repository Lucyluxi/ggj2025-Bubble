using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float sprintMultiplier = 1.2f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isStrongAttacking = false; // 标志强攻击状态
    private bool isAttacking = false;      // 标志普通攻击状态

    private Vector2 originalVelocity; // 用于保存强攻击前的速度

    public float invincibilityDuration = 1.0f;
    public float flashInterval = 0.1f; // 闪烁的间隔时间
    private bool isInvincible = false; // 是否处于无敌状态
    private SpriteRenderer spriteRenderer; // 角色的 SpriteRenderer

    private float life = 10f; // 生命值
    public TMP_Text lifeText; // 生命值 UI 文本（TextMeshPro 组件）

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // 获取 Animator 组件
        spriteRenderer = GetComponent<SpriteRenderer>();

        UpdateLifeUI(); // 初始化 UI
    }

    void Update()
    {
        // 如果强攻击未执行，允许普通攻击、移动和动画更新
        if (!isStrongAttacking)
        {
            HandleAttack();    // 处理普通攻击
            HandleMovement();  // 处理移动逻辑
            UpdateAnimation(); // 更新普通动画
        }

        HandleStrongAttack(); // 处理强攻击逻辑
    }

    void HandleMovement()
    {
        // 获取水平和垂直输入
        float moveInputX = Input.GetAxis("Horizontal");
        float moveInputY = Input.GetAxis("Vertical");

        // 检查是否按住加速键
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        // 计算角色的移动
        if (!isStrongAttacking)
        {
            Vector2 moveVelocity = new Vector2(moveInputX * currentSpeed, moveInputY * currentSpeed);
            rb.linearVelocity = moveVelocity; // 这里修正 `linearVelocity` -> `velocity`
        }

        // 限制角色移动在摄像机视野内
        ClampPlayerPosition();

        // 翻转角色（仅针对水平移动）
        if (moveInputX > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInputX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void ClampPlayerPosition() // 不行
    {
        if (Camera.main == null) return; // 防止空引用异常

        // 获取摄像机的边界
        Vector3 minBounds = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)); // 左下角
        Vector3 maxBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)); // 右上角

        // 获取角色的碰撞体尺寸
        float playerWidth = GetComponent<Collider2D>().bounds.extents.x;
        float playerHeight = GetComponent<Collider2D>().bounds.extents.y;

        // 获取角色当前位置
        Vector3 playerPos = transform.position;

        // 限制角色移动范围（考虑角色大小）
        playerPos.x = Mathf.Clamp(playerPos.x, minBounds.x + playerWidth, maxBounds.x - playerWidth);
        playerPos.y = Mathf.Clamp(playerPos.y, minBounds.y + playerHeight, maxBounds.y - playerHeight);

        // 更新角色位置
        transform.position = playerPos;
    }


    void HandleAttack()
    {
        if (Input.GetKey(KeyCode.Space)) // 按住空格触发普通攻击
        {
            isAttacking = true; // 标记普通攻击状态
            animator.Play("attack_anim"); // 播放普通攻击动画
        }
        else
        {
            isAttacking = false; // 松开空格退出普通攻击状态
        }
    }

    void HandleStrongAttack()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // 按下回车键触发强攻击
        {
            isStrongAttacking = true; // 标记为强攻击状态

            // 保存当前速度并冻结角色
            originalVelocity = rb.linearVelocity;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic; // 暂停物理模拟

            animator.Play("strongattack_anim"); // 播放强攻击动画

            // 在攻击动画结束后恢复状态
            Invoke(nameof(ResetStrongAttack), 1.0f); // 根据 strongattack_anim 的实际时长调整时间
        }
    }

    void ResetStrongAttack()
    {
        isStrongAttacking = false; // 解除强攻击状态

        // 恢复物理模拟和原速度
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = originalVelocity;
    }

    void UpdateAnimation()
    {
        // 如果正在普通攻击或强攻击，直接返回，防止动画被覆盖
        if (isAttacking || isStrongAttacking)
        {
            return;
        }

        // 根据输入更新动画状态
        float moveInputX = Mathf.Abs(Input.GetAxis("Horizontal"));
        float moveInputY = Mathf.Abs(Input.GetAxis("Vertical"));

        if (moveInputX > 0 || moveInputY > 0)
        {
            animator.Play("fly_anim"); // 播放飞行动画
        }
        else
        {
            animator.Play("idle_anim"); // 播放静止动画
        }
    }


    public void TakeDamage()
    {
        life--;
        Debug.Log("当前生命值: " + life);
        UpdateLifeUI(); // 更新 TMP 显示

        if (isInvincible)
            return; // 如果无敌状态，不执行受伤逻辑

        // 开启无敌状态
        StartCoroutine(InvincibilityCoroutine());
    }

    void UpdateLifeUI()
    {
        if (lifeText != null)
        {
            lifeText.text = "Current life: " + life.ToString(); // 更新 TMP 文本
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        // 开始闪烁
        float elapsedTime = 0;
        while (elapsedTime < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled; // 切换可见性
            elapsedTime += flashInterval;
            yield return new WaitForSeconds(flashInterval);
        }

        // 恢复正常状态
        spriteRenderer.enabled = true; // 确保最后是可见的
        isInvincible = false;
    }


}
