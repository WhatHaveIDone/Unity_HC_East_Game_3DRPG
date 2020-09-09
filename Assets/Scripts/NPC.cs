using System.Collections;   // 協程
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       // UI

public class NPC : MonoBehaviour
{
    [Header("NPC 資料")]
    public NPCData data;
    [Header("對話資訊")]
    public GameObject panel;
    [Header("對話名稱")]
    public Text txtName;
    [Header("對話內容")]
    public Text textContent;

    private AudioSource aud;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    /// <summary> 打字效果 </summary>
    private IEnumerator Type()
    {
        textContent.text = "";                                  // 對話內容清空        
        string dialog = data.dialogs[0];                        // 取得要顯示的對話
        for (int i = 0; i < dialog.Length; i++)                 // 迴圈執行對話每個字
        {
            textContent.text += dialog[i];                      // 遞增對話內容
            aud.PlayOneShot(data.soundType, 0.5f);              // 播放打字音效(調整音量)    
            yield return new WaitForSeconds(data.txtSpeed);     // 等待
        }
    }

    void NoMission()
    {

    }

    void Missioning()
    {

    }

    void Finish()
    {

    }

    /// <summary> 對話開始 </summary> 
    void DialogStart()
    {
        print("進入觸發區");
        panel.SetActive(true);      // 顯示對話資訊
        txtName.text = name;        // 更新對話名稱
        StartCoroutine(Type());     // 啟動打字效果
    }

    /// <summary> 對話結束 </summary> 
    void DialogStop()
    {
        panel.SetActive(false);
    }

    /// <summary> 面向玩家 </summary>
    /// <param name="other"> 玩家 </param>
    private void LookAtPlayer(Collider other)
    {
        // 取得面向玩家的方向
        Quaternion angle = Quaternion.LookRotation(other.transform.position - transform.position);
        // 角度差值
        transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime * 5);
    }

    /// <summary> 玩家進入觸發區域 </summary> 
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "CharaPlayer")
        {
            DialogStart();
        }
    }

    /// <summary> 玩家離開觸發區域 </summary> 
    private void OnTriggerExit(Collider other)
    {
        if(other.name == "CharaPlayer")
        {
            DialogStop();
        }
    }

    /// <summary> 玩家 </summary>
    private void OnTriggerStay(Collider other)
    {
        if(other.name == "CharaPlayer")
        {
           // ???
        }
    }
}
