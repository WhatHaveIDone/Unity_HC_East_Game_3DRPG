using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;               // 引用人工智慧API

public class Enemy : MonoBehaviour
{
    #region  欄位     
    [Header(" 怪物移動速度 "),Range(0,1000)]
    public float speed = 1.5f;
    [Header(" 怪物攻擊力 "), Range(0, 1000)]
    public float attack = 20f;
    [Header(" 怪物血量 "), Range(0, 1000)]
    public float hp = 350f;
    [Header(" 道具掉落機率 "), Range(0, 1f)]
    public float prop = 0.3f;
    [Header(" 怪物掉落經驗值 "), Range(0, 1000)]
    public float exp = 30f;
    [Header(" 掉落道具 ")]
    public Transform skull;
    [Header(" 停止距離:攻擊距離 "), Range(0, 10)]
    public float rangeAttack = 1.0f;
    [Header(" 攻擊冷卻時間 "), Range(0, 10)]
    public float cd = 3.5f;

    private NavMeshAgent navm;  // AI尋路組件
    private Player ply;
    private Animator anim;

    private float timer;        // 計時器
    #endregion

    #region  方法
    /// <summary> 移動方法 : 追蹤玩家 </summary>
    void Move()
    {
        // 代理器.設定目的地(玩家.變形.座標)
        navm.SetDestination(ply.transform.position);
        anim.SetFloat("Move",navm.velocity.magnitude);      // navm.velocity.magnitude 加速度.長度
        // 如果 剩下的距離 <= 攻擊範圍 就 攻擊
        if (navm.remainingDistance <= rangeAttack) Attack();
    }

    /// <summary> 攻擊 </summary>
    void Attack()
    {
        timer += Time.deltaTime;            // 累加時間
        if(timer >=cd)                      // 如果計時器
        {
            timer = 0;                      // 
            anim.SetTrigger("AtkTrigger");  // 
        }
        }

    void Hit()
    { }

    void Dead()
    { }

    void DropProp()
    { }


    #endregion

    #region  事件
    void Awake()    // 
    {
        navm = GetComponent<NavMeshAgent>();    // 
        anim = GetComponent<Animator>();        // 
        ply = FindObjectOfType<Player>();       // 

        navm.speed = speed;                     // 更新速度
        navm.stoppingDistance = rangeAttack;    // 更新停止距離
    }

 void Update()
    {
        Move();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.8f, 0, 0.8f, 0.5f);
        Gizmos.DrawSphere(transform.position,rangeAttack);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);

        if(other.name == "CharaPlayer")
        {
            other.GetComponent<Player>().Hit(attack, transform);
        }
    }

    #endregion





}
