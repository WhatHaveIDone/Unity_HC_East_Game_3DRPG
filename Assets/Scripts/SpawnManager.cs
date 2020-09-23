using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("怪物")]
    public Transform enemy;
    [Header("生成間隔時間"), Range(0f, 10f)]
    public float interval = 5f;

    //<summary> 生成點陣列, 類型後方加上括號就是陣列 </summary>
    public GameObject[] points;

    private void Awake()
    {
        // 生成點 = 遊戲物件.透過標籤尋找所有物件("標籤") 有 s 為負數會傳回陣列
        points = GameObject.FindGameObjectsWithTag("SpawnPoint");
        // 重複延遲呼叫方法("方法名稱", 延遲時間, 間隔時間)
        InvokeRepeating("Spawn", 0, interval);
    }

    /// <summary> 生成 </summary>
    void Spawn()
    {
        // 隨機值 = 0 ~ 陣列的長度 (此範例 : 5)
        int r = Random.Range(0, points.Length);
        // 生成(物件,生成點陣列[隨機值].座標,生成點陣列[隨機值].座標)
        Instantiate(enemy, points[r].transform.position, points[r].transform.rotation);
    }
}
