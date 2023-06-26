using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerPalmate : MonoBehaviour
{
    //スワイプ中かの判定
    //private bool doSwaip;
    //カレーを取得し、バフを受けている様態
    private bool doPoworUp;
    //ラム酒を取得し、デバフを受けている状態
    private bool doPoworDwon;
    //1ゲーム中の獲得スコア
    private int _score;
    //
    //private Touch touch;
    //GameManager
    private GameManager gm;
    //scoreManager
    private scoreManager sm;
    //効果音関係
    //牛の鳴き声
    [SerializeField] private AudioClip cowSE;
    //ネズミの鳴き声
    [SerializeField] private AudioClip mouseSE;
    //タッチ音
    [SerializeField] private AudioClip touchSE;

    //プロパティ
    public int Score {
        get { return this._score; }
        set { this._score = value; }
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
                AnimalController animalController = GetComponent<AnimalController>();
                int score = animalController.Score;
                Destroy(hit2d.collider.gameObject);
            }
        }
    }

    public void CanNotSwaip()
    {
        //doSwaip = false;
    }
    void GetAnimal(int score)
    {
        _score += score;
    }
}
