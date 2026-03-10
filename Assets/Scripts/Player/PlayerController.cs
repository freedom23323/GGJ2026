using UnityEngine;
using System.Collections;
using static UnityEngine.RuleTile.TilingRuleOutput;
using System;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    // 组件引用
    public Animator Animator { get; private set; }
    private Rigidbody2D rb;

    // 状态管理
    private IPlayerState currentState;
    private AngelState angelState = new AngelState();
    private DemonState demonState = new DemonState();

    [SerializeField] private float moveSpeed = 5f;
    private Vector2 moveInput;

    // 在 PlayerController.cs 中添加
    [Header("背包系统")]
    public int keyCount = 0; // 当前持有的钥匙数量
    public int DestroyScore = 10; // 销毁物体获得的分数

    public GameObject startPoint;

    private Vector3 localScale;

    public static Action<string> PlayerExecuteSkillEvent;
    public void AddKey()
    {
        keyCount++;
    }

    // 在 PlayerController.cs 中
    public bool TryConsumeKey()
    {
        if (keyCount > 0)
        {
            keyCount--;
            return true; // 消耗成功
        }
        return false; // 消耗失败（没有钥匙）
    }
    void Awake()
    {
        Animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
    }

    void Start()
    {
        TransitionTo(angelState); // 初始为天使
    }

    void Update()
    {
        //if (GameManager.Instance.CurrentState != GameState.Playing) return;
        //Debug.Log(Time.timeScale);
        // 1. 移动输入
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // 2. 形态切换 (Space)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleState();
        }

        // 3. 技能触发 (K)
        if (Input.GetKeyDown(KeyCode.K))
        {
            ExecuteSkill();
        }

        UpdateAnimation();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void ExecuteSkill()
    {
        currentState.ExecuteSkill(this);
        PlayerExecuteSkillEvent?.Invoke(currentState.ToString());
    }
    private void ToggleState()
    {
        if (currentState == angelState) TransitionTo(demonState);
        else TransitionTo(angelState);
    }

    private void TransitionTo(IPlayerState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    private void UpdateAnimation()
    {
        Animator.SetBool("Speed", moveInput.magnitude != 0);

        // 镜像翻转
        if (moveInput.x != 0)
        {
            transform.localScale = new Vector3(moveInput.x > 0 ? -localScale.x : localScale.x, localScale.y, localScale.z);
        }
    }

    public string GetCurrentState()
    {
        return currentState.ToString();
    }
    public void TryDestroyGameObject()
    {
        // 使用射线检测或范围检测寻找周围的 ball 或者 door
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, 1.5f);
        foreach (var obj in hitObjects)
        {
            if (obj.CompareTag("ball"))
            {
                // 执行销毁
                obj.GetComponent<ConsumableItem>().Collect(); 
                // 联动：增加分数
                ScoreManager.Instance.AddScore(DestroyScore);
            }
            else if (obj.CompareTag("door"))
            {
                obj.GetComponent<ConsumableItem>().Collect();
            }
        }
    }
    public void Reset()
    {
        transform.position = startPoint.transform.position;
        keyCount = 0;
        TransitionTo(angelState);
    }
    public void PlayAttackAnimation()
    {
        Animator.SetBool("Attack", true);
    }

    public void OnAttackComplete()
    {
        Animator.SetBool("Attack", false);
    }
}

// 状态接口
public interface IPlayerState
{
    void EnterState(PlayerController player);
    void ExecuteSkill(PlayerController player);
}

// 天使状态
public class AngelState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        player.Animator.SetLayerWeight(1, 0f);
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Demon"));
    }
    public void ExecuteSkill(PlayerController player)
    {
        Debug.Log("释放【净化】波！");
        // 这里可以实例化净化特效
        player.PlayAttackAnimation();
    }
}

// 恶魔状态
public class DemonState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        player.Animator.SetLayerWeight(1, 1f);
        Camera.main.cullingMask |= (1 << LayerMask.NameToLayer("Demon"));
    }
    public void ExecuteSkill(PlayerController player)
    {
        Debug.Log("挥舞【恶魔之爪】！");
        // 这里可以执行物理攻击判定
        player.PlayAttackAnimation();
        player.TryDestroyGameObject();
    }
}


