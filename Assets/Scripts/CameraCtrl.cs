﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [Header("目標")]
    public Transform target;
    [Header("追蹤速度"), Range(0, 500)]
    public float speed = 1;
    [Header("旋轉速度"), Range(0, 500)]
    public float turn = 1;
    [Header("上下角度限制")]
    public Vector2 limit = new Vector2(45,-30);

    /// <summary> 攝影機要旋轉的角度 </summary>
    public Quaternion rot;

    /// <summary> 攝影機追蹤玩家 </summary>
    void Track()
    {
        // 追蹤 : 利用差值讓攝影機前往玩家位置
        Vector3 posA = transform.position;
        Vector3 posB = target.position;

        transform.position = Vector3.Lerp(posA,posB,Time.deltaTime*speed);

        // 旋轉攝影機
        rot.x -= Input.GetAxis("Mouse Y") * turn * Time.deltaTime;  // 利用滑鼠 Y 控制上下角度
        rot.y += Input.GetAxis("Mouse X") * turn * Time.deltaTime;  // 利用滑鼠 X 控制左右角度

        rot.x = Mathf.Clamp(rot.x, limit.y, limit.x);               // 夾住 X軸 在限制角度內

        // 將 四元角度 轉為 歐拉角度(0 ~ 360度)
        transform.rotation = Quaternion.Euler(rot.x, 180 +rot.y, rot.z);
    }


    private void FixedUpdate()
    {
        Track();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
