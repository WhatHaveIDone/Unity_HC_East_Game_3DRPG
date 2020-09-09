using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 列舉(下拉式選單)
public enum StateNPC
{
    Type, NoMission, Missioning, Finish
}

// ScriptableObject 腳本化物件 : 將資料儲存於專案
// CreateAssetMenu 建立菜單 : 檔案名稱和菜單名稱
[CreateAssetMenu(fileName ="NPC 資料",menuName ="腳本化物件")]
public class NPCData : ScriptableObject
{
    [Header("打字速度"), Range(0f, 3f)]
    public float txtSpeed = 0.5f;
    [Header("打字音效")]
    public AudioClip soundType;
    [Header("任務需求數量"), Range(5, 50)]
    public int misCount;
    [Header("對話"), TextArea(3, 10)]
    public string[] dialogs = new string[3];
    [Header("NPC狀態")]
    public StateNPC state;

}
