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
    [Header("技能音效")]
    public AudioClip sndSkillA;
    [Header("攻擊音效")]
    public AudioClip sndAtk;
    [Header("受傷音效")]
    public AudioClip sndHit;
    [Header("任務數量")]
    public Text textMission;
    [Header("角色吧條")]
    public Image barHp;
    public Image barMp;
    public Image barExp;
    

    public float exp;
    public int lv = 1;
    public Text textLv;

    [Header("技能")]
    public GameObject rock;
    public Transform pointRock;
    public float costRock;
    public float damageRock = 50;

    /// <summary> 攝影機根物件 </summary>
    private Transform cam;
  
    private int count;
    public float maxHp ;
    public float maxMp ;
    public float maxExp ;
    // 浮點數陣列
    public float[] exps;

    private Animator anim;
    private Rigidbody rig;
    private AudioSource aud;
    private NPC npc;

    [HideInInspector]
    /// <summary> 是否停止 </summary>
    public bool stop;

    #endregion
    // ---------------------------------------------
    #region 方法:功能
    /// <summary> 角色移動 </summary>
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

    /// <summary> 拾取道具 </summary> 
    void EatProp()
    {
        count++;                                                        // 遞增
        textMission.text = "道具:" + count + "/" + npc.data.misCount;   // 更新內容      
        aud.PlayOneShot(soundProp);
        // 
        if (count == npc.data.misCount) npc.Finish();
    }

    /// <summary> 玩家被擊中受傷時 </summary>
    /// <param name="damage"> 傷害值 </param>
    /// <param name="direction"> 擊退方向 </param>
    public void Hit(float damage, Transform direction)
    {
        aud.PlayOneShot(sndHit);
        // 扣血
        hp -= damage;
        // 對玩家的剛體 施加一個力  -> 玩家起飛
        rig.AddForce(direction.forward*100 + direction.up*150);

        barHp.fillAmount = hp / maxHp;
        anim.SetTrigger("HurtTrigger");

        if (hp <= 0) Dead();            // 如果 血量 <= 0 就 死亡
    }

    /// <summary> 死亡 </summary>
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
            aud.PlayOneShot(sndAtk);
            anim.SetTrigger("AtkTrigger");
        }
    }

    /// <summary>
    /// 經驗值
    /// </summary>
    /// <param name="expGet"> 取得經驗值 </param>
    public void Exp(float expGet)
    {
        exp += expGet;                      // 經驗值累加
        barExp.fillAmount = exp / maxExp;   // 更新經驗值吧條
        while (exp >= maxExp) LevelUp();    // 如果 目前經驗值>=最大經驗值 就 升級
    }

    /// <summary> 升級 </summary>
    public void LevelUp()
    {
        lv++;                               // 等級遞增
        textLv.text = "Lv" + lv;            // 更新等級介面

        // 升級後數值改變 
        maxHp += 20;
        maxMp += 5;
        attack += 10;

        // 升級後血量魔力全滿
        hp = maxHp;
        mp = maxMp;

        barHp.fillAmount = 1;
        barMp.fillAmount = 1;

        // 把多餘的經驗值補回去   120 -= 100(20) exp-maxExp=exp
        exp -= maxExp;
        // 最大經驗值 = 經驗值需求[等級-1]
        maxExp = exps[lv - 1];
        // 更新經驗值長度 = 目前經驗值 / 最大經驗值
        barExp.fillAmount = exp / maxExp;
    }

    ///<summary> </summary>
    void SkillRock()
    {
        // 如果 按下右鍵 並且 魔力 >= 技能消耗
        if(Input.GetKeyDown(KeyCode.Mouse1) && mp>= costRock)
        {
            aud.PlayOneShot(sndSkillA);
            // 播放動畫
            anim.SetTrigger("Skill");
            // 生成(物件,座標,角度)
            Instantiate(rock, pointRock.position, pointRock.rotation);
            // 扣除消耗量
            mp -= costRock;
            // 更新魔力吧條
            barMp.fillAmount = mp / maxMp;
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

        // 經驗值需求 總共有99筆
        exps = new float[99];

        // 迴圈執行每一筆經驗值需求 = 100*等級
        // 陣列.Length 為陣列的數量 此範例為 99
        for (int i = 0; i < exps.Length; i++) exps[i] = 100 * (i + 1);
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
        Attack();                               // 攻擊
        SkillRock();                            // 技能
        Restore(hp,restoreHp,maxHp,barHp);      // 回血
        Restore(mp, restoreMp, maxMp, barMp);   // 回魔
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

    /*
    [Header("回魔")]
    public float restoreMp = 5;
    void RestoreMp()
    {
        mp += restoreMp * Time.deltaTime;   // 每秒回復
        mp = Mathf.Clamp(mp, 0, maxMp);     // 夾住數值(數值,0,最大值)
        barMp.fillAmount = mp / maxMp;      // 更新介面
    }
    */
    [Header("回魔量/每秒")]
    public float restoreMp = 5;
    [Header("回血量/每秒")]
    public float restoreHp = 10;

    /// <summary>
    /// 恢復數值
    /// </summary>
    /// <param name="value">要恢復的值</param>
    /// <param name="restore">每秒恢復多少</param>
    /// <param name="max">要恢復的值最大值</param>
    /// <param name="bar">要更新的吧條</param>
    void Restore(float value,float restore, float max, Image bar)
    {
        value += restore * Time.deltaTime;
        value = Mathf.Clamp(value, 0, max);
        bar.fillAmount = value / max;
    }


    #endregion 



}
