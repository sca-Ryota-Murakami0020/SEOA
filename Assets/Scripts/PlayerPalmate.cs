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
    //GameManager
    private GameManager gm;
    //scoreManager
    private scoreManager sm;

    //プロパティ
    public int Score {
        get { return this.score; }
        set { this.score = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        sm = GameObject.Find("Scorecounter").GetComponent<scoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))//Input.touchCount == 1 && touch.phase == TouchPhase.Began
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin,(Vector2)ray.direction,100);
            //if(Physics2D.Raycast(ray,out hit2d,))
            if(hit2d.collider && hit2d.collider.tag == "animal")
            {
                if(gm.Mouse)
                {
                    GetMouse();
                }
                if(!gm.Mouse)
                {
                    GetCow();
                }
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
        sm.UpdateScore(score);
    }

    public void GetMouse()
    {
        score += 30;
        sm.UpdateScore(score);
    }
}
