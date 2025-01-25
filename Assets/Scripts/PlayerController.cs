using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float sprintMultiplier = 1.2f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isStrongAttacking = false; // 标志强攻击状态
    private bool isAttacking = false;      // 标志普通攻击状态

    private Vector2 originalVelocity; // 用于保存强攻击前的速度

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // 获取 Animator 组件
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

        // 设置角色的速度（强攻击时不改变速度）
        if (!isStrongAttacking)
        {
            Vector2 moveVelocity = new Vector2(moveInputX * currentSpeed, moveInputY * currentSpeed);
            rb.linearVelocity = moveVelocity;
        }

        // 翻转角色（仅针对水平移动）
        if (moveInputX > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInputX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
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
            rb.isKinematic = true; // 暂停物理模拟

            animator.Play("strongattack_anim"); // 播放强攻击动画

            // 在攻击动画结束后恢复状态
            Invoke(nameof(ResetStrongAttack), 1.0f); // 根据 strongattack_anim 的实际时长调整时间
        }
    }

    void ResetStrongAttack()
    {
        isStrongAttacking = false; // 解除强攻击状态

        // 恢复物理模拟和原速度
        rb.isKinematic = false;
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
}
