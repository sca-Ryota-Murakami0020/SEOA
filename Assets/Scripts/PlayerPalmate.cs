using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPalmate : MonoBehaviour
{
    //スワイプ中かの判定
    private bool doSwaip;
    //カレーを取得し、バフを受けている様態
    private bool doPoworUp;
    //ラム酒を取得し、デバフを受けている状態
    private bool doPoworDwon;
    //1ゲーム中の獲得スコア
    private int score;
    //
    private Touch touch;

    //プロパティ
    public int Score {
        get { return this.score; }
        set { this.score = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))//Input.touchCount == 1 && touch.phase == TouchPhase.Began
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin,(Vector2)ray.direction,100);
            if(hit2d && hit2d.collider.tag == "牛")
            {
                //角度と距離の計算
                GetCow();
                Debug.Log("いったん");
                Destroy(hit2d.collider.gameObject);
            }
        }
    }

    public void CanNotSwaip()
    {
        doSwaip = false;
    }

    public void GetCow()
    {
        score += 100;
    }

    public void GetMouse()
    {
        score += 30;
    }
}
