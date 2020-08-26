using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("載入畫面")]
    public GameObject panel;
    [Header("載入進度")]
    public Text textLoading;
    [Header("載入吧條")]
    public Image imgLoading;
    [Header("提示文字")]
    public Text textTip;

    /// <summary> 怒關遊戲 </summary>
    public void QuitGame()
    {
        Application.Quit(); // 應用程式.離開
    }

    /// <summary> 開始被玩 </summary>
    public void StartGame()
    {
        StartCoroutine(Loading());  // 啟動協程
    }

    void Start()
    {
        
    }

    private IEnumerator Loading()
    {
        panel.SetActive(true);
        textTip.enabled = false;
        AsyncOperation ao = SceneManager.LoadSceneAsync("GameStage01");    // 非同步載入資訊 = 場景管理器.非同步載入("場景名稱")
        ao.allowSceneActivation = false;                        // 載入資訊.允許自動載入 = 否

        // ao.isDone == true    簡寫 ao.isDone
        // ao.isDone == false   簡寫 !ao.isDone
        // 當 載入資訊.完成 == false -> 尚未載入完成時 執行迴圈
        while (!ao.isDone)
        {
            // progress 載入場景的進度值為 0 - 1 ,如果 allow 設定為 false 會卡在 0.9
            // ToString("F小數點點位數") : 小數點兩位 F2  小數點零位 F0 - F 大小寫皆可
            textLoading.text = "載入進度 : " + (ao.progress / 0.9f * 100).ToString("F2") + "%"; // 載入文字 = "載入進度" + ao.進度*100 + "%"
            imgLoading.fillAmount = ao.progress / 0.9f;                        // 載入吧條 = ao.進度 
            yield return null;

            if(ao.progress == 0.9f)     // 如果 ao.進度 等於 0.9
            {
                textTip.enabled = true; // 提示文字.啟動 = 是 - 顯示提示文字

                if (Input.anyKeyDown)   ao.allowSceneActivation = true; // 如果按下任意鍵 允許自動載入
            }  
        }
    }

    void Update()
    {
        
    }
}
