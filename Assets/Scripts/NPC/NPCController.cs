using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;


public class NPCController : MonoBehaviour
{
    public StateMachine StateMachine { get; private set; }
    public Animator Anim { get; private set; }

    public NPCIdleState IdleState { get; private set; }
    public NPCAngryState AngryState { get; private set; }
    public NPCMoveState MoveState { get; private set; }

    [Header("引用与设置")]
    public float detectDistance = 4f;
    public Transform patrolPoint;
    private bool _isGameOver = false;

    public PlayerController _player;
    public bool IsPlayerExecuteSkill = false;
    public GameObject AngrySprite;
    private void Awake()
    {
        StateMachine = new StateMachine();
        Anim = GetComponent<Animator>();

        IdleState = new NPCIdleState(this,_player);
        AngryState = new NPCAngryState(this, _player);
        MoveState = new NPCMoveState(this, _player);
    }

    private void OnEnable()
    {
        // 监听玩家技能事件
        PlayerController.PlayerExecuteSkillEvent += SetPlayerExecuteSkill;
    }

    public void SetPlayerExecuteSkill(string Message)
    {
        Debug.Log("NPC 收到玩家技能事件: " + Message);
        if(Message== "AngelState") IsPlayerExecuteSkill = true;
    }

    public void UnsetPlayerExecuteSkill()
    {
        IsPlayerExecuteSkill = false;
    }

    private void Start() => StateMachine.TransitionTo(IdleState);

    private void Update()
    {
        if (_isGameOver) return; // 游戏结束停止逻辑
        StateMachine.Update();
    }

    private void OnDisable()
    {
        // 取消监听
        PlayerController.PlayerExecuteSkillEvent -= SetPlayerExecuteSkill;
    }
    public void GameOver()
    {
        if (_isGameOver) return;
        _isGameOver = true;

        Debug.LogError("GAME OVER: NPC 被彻底激怒了！");
        // 这里可以调用你的 UI 画面或重启场景
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 辅助判定
    public bool IsPlayerTooClose() => Vector3.Distance(transform.position, _player.transform.position) < detectDistance;
    public void MoveToTarget() => transform.position = Vector3.MoveTowards(transform.position, patrolPoint.position, 2f * Time.deltaTime);
    public bool HasReachedDestination() => Vector3.Distance(transform.position, patrolPoint.position) < 0.2f;
}
public interface IState
{
    void OnEnter();   // 进入状态时执行一次
    void OnUpdate();  // 每帧执行
    void OnExit();    // 离开状态时执行一次
}
public class StateMachine
{
    public IState CurrentState { get; private set; }

    // 切换状态
    public void TransitionTo(IState newState)
    {
        if (newState == null || newState == CurrentState) return;

        CurrentState?.OnExit();
        CurrentState = newState;
        CurrentState.OnEnter();
    }

    // 在 MonoBehaviour 的 Update 中调用
    public void Update()
    {
        CurrentState?.OnUpdate();
    }
}
public class NPCIdleState : IState
{
    private NPCController _npc;
    PlayerController _player;

    public NPCIdleState(NPCController npc,PlayerController player)
    {
        _npc= npc;
        _player= player;
    }

    public void OnEnter()
    {
        _npc.Anim.SetInteger("State", 0); // 切换动画
    }

    public void OnUpdate()
    {
        if (!_npc.IsPlayerTooClose()) return;

        if (_player.GetCurrentState()== "DemonState")
        {
            _npc.StateMachine.TransitionTo(_npc.AngryState);
        }
        else if(_player.GetCurrentState() == "AngelState"&& _npc.IsPlayerExecuteSkill)
        {
            _npc.StateMachine.TransitionTo(_npc.MoveState);
        }
    }

    public void OnExit() {
        _npc.UnsetPlayerExecuteSkill();
    }
}

public class NPCAngryState : IState
{
    private NPCController _npc;
    PlayerController _player;

    public NPCAngryState(NPCController npc, PlayerController player)
    {
        _npc = npc;
        _player = player;
    }

    public void OnEnter()
    {
        _npc.AngrySprite.SetActive(true);
        Debug.Log("NPC 生气了！快跑！");
    }

    public void OnUpdate()
    {
        if (!_npc.IsPlayerTooClose())
        {
            _npc.StateMachine.TransitionTo(_npc.IdleState);
        }
    }

    public void OnExit()
    {
        Debug.Log("NPC 消气了。");
        _npc.UnsetPlayerExecuteSkill();
        _npc.AngrySprite.SetActive(false);
    }
}
public class NPCMoveState : IState
{
    private NPCController _npc;

    PlayerController _player;

    bool can_move = false;

    public NPCMoveState(NPCController npc, PlayerController player)
    {
        _npc = npc;
        _player = player;
    }

    public void OnEnter()
    {
        _npc.Anim.SetInteger("State", 1); // 切换动画
        can_move = false;
    }

    public void OnUpdate()
    {
        _npc.MoveToTarget();

        if (_npc.HasReachedDestination()||!_npc.IsPlayerTooClose())
        {
            _npc.StateMachine.TransitionTo(_npc.IdleState);
        }
    }

    public void OnExit() {
        _npc.UnsetPlayerExecuteSkill();
    }
}
