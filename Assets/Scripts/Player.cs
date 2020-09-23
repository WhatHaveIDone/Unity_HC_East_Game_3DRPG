using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region 欄位 : 基本資料
    [Header("移動速度"),Range(0,500)]
    public float speed;
    [Header("旋轉速度"), Range(0, 500)]
    public float turn;
    [Header("攻擊力"), Range(0, 500)]
    public float attack = 500;
    [Header("血量"), Range(0, 500)]
    public float hp;
    [Header("魔力"), Range(0, 500)]
    public float mp;
    [Header("吃道具音效")]
    public AudioClip soundProp;
    [Header("任務數量")]
    public Text textMission;

    private int count;

    public float exp;
    public int lv = 1;

    private Animator anim;
    private Rigidbody rig;
    private AudioSource aud;
    private NPC npc;

    /// <summary> 攝影機根物件 </summary>
    private Transform cam;

    public Image barHp;
    public Image barMp;
    public Image barExp;

    private float maxHp;
    private float maxMp;
    private float maxExp;

    [HideInInspector]
    /// <summary> 是否停止 </summary>
    public bool stop;

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

    void EatProp()
    {
        count++;                                                        // 遞增
        textMission.text = "道具:" + count + "/" + npc.data.misCount;   // 更新內容      

        // 
        if (count == npc.data.misCount) npc.Finish();
    }

    /// <summary> 玩家被擊中受傷時 </summary>
    /// <param name="damage"> 傷害值 </param>
    /// <param name="direction"> 擊退方向 </param>
    public void Hit(float damage, Transform direction)
    {
        // 扣血
        hp -= damage;
        // 對玩家的剛體 施加一個力  -> 玩家起飛
        rig.AddForce(direction.forward*100 + direction.up*150);

        barHp.fillAmount = hp / maxHp;
        anim.SetTrigger("HurtTrigger");

        if (hp <= 0) Dead();            // 如果 血量 <= 0 就 死亡
    }

    /// <summary> 死亡 </summary>S
    void Dead()
    {
        // this.enabled = false;        // 第一種寫法 ,this 此腳本 
        enabled = false;                // 此腳本.啟動
        anim.SetBool("Wasted", true);   // 死亡動畫
    }

    ///<summary> 攻擊 </summary>
    void Attack()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("AtkTrigger");
        }
    }

    #endregion
    // ---------------------------------------------

   
    #region 事件:入口
    /// <summary> 喚醒 : 會在 Start 前執行一次 </summary>
    private void Awake()
    {
        // 取得元件<泛型>() - 泛型 : 任何類型
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();

        // 遊戲物件.尋找("物件名稱") --> 建議不要在 Update 內使用 會吃效能
        cam = GameObject.Find("攝影機根物件").transform;

        npc = FindObjectOfType<NPC>();      // 取得NPC

        maxHp = hp;
        maxMp = mp;
    }

    /// <summary>
    /// 固定更新 : 固定50fps
    /// 有物理運動在這裡呼叫rigidbody
    /// </summary>
    private void FixedUpdate()
    {
        if (stop) return;       // 如果 停止 就跳出

        Move();
    }

    public void Update()
    {
        Attack();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Skull")     // 當碰到"Skull"時
        {
            aud.PlayOneShot(soundProp);             // 播放音效
            Destroy(collision.gameObject);          // 刪除道具
            EatProp();                              // 呼叫吃道具
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().Hit(attack,transform);
        }
    }

    #endregion 
}
