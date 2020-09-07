using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region 欄位 : 基本資料
    [Header("移動速度"),Range(0,500)]
    public float speed;
        [Header("旋轉速度"), Range(0, 500)]
    public float turn;
        [Header("攻擊力"), Range(0, 500)]
    public float attack;
        [Header("血量"), Range(0, 500)]
    public float hp;
        [Header("魔力"), Range(0, 500)]
    public float mp;

    public float exp;
    public int lv;

    private Animator anim;
    private Rigidbody rig;

    #endregion
    // ---------------------------------------------
    #region 方法:功能
    private void Move()
    {
        float h = Input.GetAxis("Horizontal");  // (A D & 左 右) : A 左 -1 , D 右 1 , 沒按 0
        float v = Input.GetAxis("Vertical");    // (S W & 上 下) : S 下 -1 , W 上 1 , 沒按 0

        // Vector3 pos = new Vector3(h, 0, v);                          // 要移動的座標 - 世界座標版本
        // Vector3 pos = transform.forward * v + transform.right * h;   // 要移動的座標 - 區域座標版本
        Vector3 pos = cam.forward * v + cam.right * h;                  // 要移動的座標 - 攝影機的區域座標版本

        // 剛體.移動座標( 本身的座標 + 要移動的座標 * 速度 * 1/50 )
        rig.MovePosition(transform.position + pos * speed * Time.fixedTime);
        // 動畫.設定浮點數( "參數名稱" , 前後 + 左右 的 絕對值 )
        anim.SetFloat("Move", Mathf.Abs(v) + Mathf.Abs(h));
    }


    #endregion
    // ---------------------------------------------
    
    /// <summary> 攝影機根物件 </summary>
    private Transform cam;

    #region 事件:入口
    /// <summary> 喚醒 : 會在 Start 前執行一次 </summary>
    private void Awake()
    {
        // 取得元件<泛型>() - 泛型 : 任何類型
       anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();

        // 遊戲物件.尋找("物件名稱") --> 建議不要在 Update 內使用 會吃效能
        cam = GameObject.Find("攝影機根物件").transform;
    }

    /// <summary>
    /// 固定更新 : 固定50fps
    /// 有物理運動在這裡呼叫rigidbody
    /// </summary>
    private void FixedUpdate()
    {
        Move();


    }

    #endregion
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
