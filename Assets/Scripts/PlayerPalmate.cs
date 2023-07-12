using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class PlayerPalmate : MonoBehaviour
{
    #region//参照するスクリプト
    //scoreManager
    private scoreManager sm;
    //GameManager
    private GameManager gm;
    //Audiosource
    private AudioSource audios = null;
    #endregion

    #region//変数
    //スワイプ中かの判定
    private bool doSwaip;
    //カレーを取得し、バフを受けている様態
    private bool doPoworUp;
    //ラム酒を取得し、デバフを受けている状態
    private bool doPoworDwon;
    //ポーズ中orゲーム開始３カウント前の操作できない状態
    private bool doStop;
    //獲得した動物の名前
    private string animalName;
    //1ゲーム中の獲得スコア
    private int _score;
    //ゲーム時間(画像で数字の表示をおこなうため、整数型の方が実装しやすいから)
    private int gameTime;
    //表示する画像の番地数を格納する変数
    private int spriteCount;
    //細かい時間経過をおこなうために用いる小数型の変数
    private float countTime;
    //スワイプできる残りの距離
    private float swaipRange = 5.0f;
    //1フレーム前のスワイプの位置
    private Vector2 oldFlameTouchPos;
    //現在のスワイプ位置
    private Vector2 nowTouchPos;
    //現在つなげる始点になっている動物の位置
    private Vector2 oldTouchPos;
    #endregion

    #region//効果音関係
    //牛の鳴き声
    [SerializeField] private AudioClip cowSE;
    //ネズミの鳴き声
    [SerializeField] private AudioClip mouseSE;
    //タッチ音
    [SerializeField] private AudioClip touchSE;
    //スワイプしたときにゲージが減る効果音
    [SerializeField] private AudioClip gazeSE;
    //６０～２０秒のＢＧＭ
    [SerializeField] private AudioClip beginBGM;
    //ラストスパートBGＭ
    [SerializeField] private AudioClip lastBGM;
    //数字のイメージ
    [Header("表示する数字のImage")][SerializeField] private Sprite[] numberImage;
    //数字の配置位置
    [Header("表示するImageの配置位置")][SerializeField] private Image[] imageNumber;
    #endregion

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
        audios = GetComponent<AudioSource>();

        //初期化
        _score = 0;
        gameTime = 60;
        countTime = 0.0f;
        doStop = true;
        doPoworUp = false;
        doPoworDwon = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(doStop == true)
        {
            //時間計測
            CountTime();
            //BGMを流す関数
            PlayBGM();
            //スワイプ操作
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 100);
                //if(Physics2D.Raycast(ray,out hit2d,))
                //何もないところをタップしたら
                if (hit2d.collider == null)
                {
                    return;
                }
                //タッチしたオブジェクトが動物なら
                if (hit2d.collider && hit2d.collider.tag == "animal")
                {
                    oldTouchPos = hit2d.collider.transform.position;
                    doSwaip = false;
                }
            }

            if(Input.GetMouseButton(0) && doSwaip == true)
            {
                //1フレーム前のマウスの位置を保持
                oldFlameTouchPos = nowTouchPos;
                //現在のスワイプ位置の更新
                nowTouchPos = Input.mousePosition;
                //ここで移動距離の計算を行う
                NowVectorPosition(nowTouchPos);
                float maveCount = nowTouchPos - oldFlameTouchPos;
                swaipRange -= 

                //始点の更新
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 100);
                //何もないところをタップしたら
                if (hit2d.collider == null)
                {
                    return;
                }
                if (hit2d.collider && hit2d.collider.tag == "animal")
                {
                    //ここでスワイプ先に動物がいたときに、触れた動物を次の始点に変更する
                    oldTouchPos = hit2d.collider.transform.position;
                }
            }

            //タッチ処理
            if (Input.GetMouseButton(0))//Input.touchCount == 1 && touch.phase == TouchPhase.Began
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 100);
                //if(Physics2D.Raycast(ray,out hit2d,))
                //何もないところをタップしたら
                if (hit2d.collider == null)
                {
                    return;
                }
                //タッチしたオブジェクトが動物なら
                if (hit2d.collider && hit2d.collider.tag == "animal")
                {
                    if(Input.GetMouseButtonUp(0) && doSwaip == false)
                    {
                        AnimalController animalController = GameObject.FindWithTag("animal").GetComponent<AnimalController>();
                        int score = animalController.Score;
                        animalName = hit2d.collider.name;
                        GetAnimal(score, animalName);
                        Destroy(hit2d.collider.gameObject);
                    }
                }
            }
        }
    }

    private float NowVectorPosition(Vector2 vec)
    {
        
    }

    private void CountTime()
    {
        countTime += Time.deltaTime;
        if (countTime >= 1.0f)
        {
            --gameTime;
            countTime = 0.0f;
            //10秒単位の計算(トータルのゲーム時間/10(小数点以下は切り捨て))
            imageNumber[1].sprite = numberImage[gameTime / 10];
            //1秒単位の計算(トータルのゲーム時間ー10秒単位の数値＊10)
            int secondTimer = gameTime - ((gameTime / 10) * 10);
            imageNumber[0].sprite = numberImage[secondTimer];
        }
    }

    private void PlayBGM()
    {
        if (gameTime >= 20 && gameTime <= 60)
        {
            PlayBGM(beginBGM);
        }
        if (gameTime < 20)
        {
            PlayBGM(lastBGM);
        }
    }

    void GetAnimal(int score,string name)
    {
        _score += score;
        PlayBGM(touchSE);
        //動物に応じて呼び出す鳴き声を変更する
        if(name == "Cow") PlayBGM(cowSE);
        if(name == "Mouse") PlayBGM(mouseSE);
        sm.UpdateScore(_score);
    }

    //BGMや効果音を呼び出す関数
    public void PlayBGM(AudioClip clip)
    {
        if (audios != null)
            audios.PlayOneShot(clip);
    }
}
