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
    [Header(" 面向玩家的速度 "), Range(0, 100)]
    public float turn = 10;

    private NavMeshAgent navm;  // AI尋路組件
    private Player ply;         // 玩家角色
    private Animator anim;
    private Rigidbody rig;      // 剛體

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
        // 取得面向的的角度 B 角度 = 四元.面向角度(玩家 - 自己)
        Quaternion look = Quaternion.LookRotation(ply.transform.position - transform.position);
        // 怪物.角度 = 四元.差值(A角度, B角度, 百分比)
        transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime*turn);

        timer += Time.deltaTime;            // 累加時間
        if(timer >=cd)                      // 如果計時器
        {
            timer = 0;                      // 計時器歸零
            anim.SetTrigger("AtkTrigger");  // 攻擊動畫
        }
        }

    /// <summary> 受傷 </summary>
    public void Hit(float damage, Transform direction)
    {
        hp -= damage;
        rig.AddForce(direction.forward * 30 + direction.up * 50);

        anim.SetTrigger("HurtTrigger");
        if (hp <= 0) Dead();
    }

    /// <summary> 死亡 </summary>
    void Dead()
    {
        // this.enabled = false;                    // 第一種寫法 ,this 此腳本 
        GetComponent<Collider>().enabled = false;   // 此腳本.啟動
        anim.SetBool("Wasted", true);               // 死亡動畫
        DropProp();                                 // 掉落道具
        ply.Exp(exp);                               // 
    }

    /// <summary> 掉落道具 </summary>
    void DropProp()                     
    {
        float r = Random.Range(0f,1f);  // 取得隨機值介於 0~1

        if (r <= prop)                    // 如果 隨機值 小於等於 機率
        {
            // 生成(物件, 座標, 角度)
            Instantiate(skull, transform.position + transform.up * 1.5f, transform.rotation);
        }
    }
    #endregion

    #region  事件
    void Awake()    // 
    {
        navm = GetComponent<NavMeshAgent>();    // 
        anim = GetComponent<Animator>();        // 
        rig = GetComponent<Rigidbody>();        // 

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

    /// <summary>
    /// 有勾選 Collision 與 Send Collision Messages 的粒子碰到後會執行一次
    /// </summary>
    /// <param name="other"></param>
    private void OnParticleCollision(GameObject other)
    {
        if(other.name == "Dust")
        {
            float damage = ply.damageRock;  // 取得流星雨的傷害值
            Hit(damage, ply.transform);     // 受傷
        }
    }

    #endregion





}
