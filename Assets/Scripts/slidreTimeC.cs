using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slidreTimeC : MonoBehaviour
{
    //スワイプできる時間
    private float slidTime;
    //初期化用の変数
    private float defultTime = 3.0f;
    //プレイヤーの操作を管理しているスクリプト
    private PlayerPalmate pl;
    //スワイプできる時間を可視化するゲージ
    [SerializeField] private Slider swaipGaze;

    // Start is called before the first frame update
    void Start()
    {
        //ゲージの初期化
        slidTime = defultTime;
        pl = GetComponent<PlayerPalmate>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Slid()
    {
        slidTime -= Time.deltaTime;
        if(slidTime <= 0.0f)
        {
            pl.CanNotSwaip();
        }
    }

    private void ResetTime()
    {
        slidTime = defultTime;
    }
}
