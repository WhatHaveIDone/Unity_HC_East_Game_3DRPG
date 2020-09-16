using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [Header("目標位置")]
    public Transform target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "CharaPlayer")
            target.GetComponent<CapsuleCollider>().enabled = false; // 關閉 要前往的 目標傳送門碰撞器 - 避免無限迴圈
            other.transform.position = target.position;             // 玩家傳送到目標位置
            Invoke("OpenCollider", 3f);                             // 延遲三秒
    }

    /// <summary> 開啟碰撞器 </summary>
    private void OpenCollider()
    {
        target.GetComponent<CapsuleCollider>().enabled = true;
    }

}
