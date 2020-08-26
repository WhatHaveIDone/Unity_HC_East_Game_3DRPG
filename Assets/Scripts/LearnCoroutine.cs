using System.Collections;   // 使用'協程'時必須引用此 API(系統.集合)
using UnityEngine;

public class LearnCoroutine : MonoBehaviour
{
    // 一般方法:無傳回
    void MethodA()
    {
        
    }

    // 有傳回方法
    int MethodB()
    {
        return 10;
    }

    // 協程方法
    // 1.傳回類型:IEnumerator
    // 2.yield return 時間 new WaitForSeconds(秒數) null 一個影格的時間 
    IEnumerator Test()
    {
        print("我是協程的第一行");
        yield return new WaitForSeconds(2);
        print("我是二秒後的程式"); 
    }

    private void Start()
    {
        // 呼叫協程
        StartCoroutine(Test());
        // 啟動協程( Big() )
        StartCoroutine(Big());
    }

    public Transform cube;

    private IEnumerator Big()
    {
        // 迴圈 for 0 ~ 10
        for(int i= 0; i<10; i++)
        { cube.localScale += Vector3.one;       // 尺寸 += 三維向量的 1
        yield return new WaitForSeconds(0.5f);  // 等待半秒
        }       
    }

}
