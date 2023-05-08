using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slidreTimeC : MonoBehaviour
{
    //スワイプできる時間
    private float slidTime;
    //初期化用の変数
    private float defultTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        slidTime = defultTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Slid()
    {
        slidTime -= 
    }

    private void ResetTime()
    {
        slidTime = defultTime;
    }
}
