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
    //AnimalManager
    [SerializeField]private AnimalManager am;
    //エフェクト
    [Header("呼び出すエフェクト")] [SerializeField] private effectsC[] effect;
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
    //エフェクトを呼び出した回数
    private int effectCount;
    //1ゲーム中の獲得スコア
    private int _score;
    //ゲーム時間(画像で数字の表示をおこなうため、整数型の方が実装しやすいから)
    private int gameTime;
    //表示する画像の番地数を格納する変数
    private int spriteCount;
    //細かい時間経過をおこなうために用いる小数型の変数
    private float countTime;
    //1フレーム前のスワイプの位置
    private Vector2 oldFlameTouchPos;
    //現在のスワイプ位置
    private Vector2 nowTouchPos;
    //現在つなげる始点になっている動物の位置
    private Vector2 oldTouchPos;
    //捕まえた動物の情報
    private Queue<GameObject> animalInfo;
    //数字のイメージ
    [Header("表示する数字のImage")] [SerializeField] private Sprite[] numberImage;
    //数字の配置位置
    [Header("表示するImageの配置位置")] [SerializeField] private Image[] imageNumber;
    private Touch touch;
    #endregion

    #region//状態クラス
    //プレイヤーの状態
    private enum PlayerState
    {
        NULL,
        NORMAL,
        POWERUP,
        POWERDOWM
    };
    private PlayerState playerState;
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
        am = GetComponent<AnimalManager>();

        //初期化
        _score = 0;
        gameTime = 60;
        countTime = 0.0f;
        effectCount = 0;
        doStop = true;
        doPoworUp = false;
        doPoworDwon = false;
        animalName = null;
        animalInfo = new Queue<GameObject>();
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
            //操作
            if(Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 100);
                //何もないところをタップorスワイプ中なら
                if(hit2d.collider == null)
                {
                    return;
                }
                if(hit2d.collider.tag == "animal")
                {
                    //記憶する名前の更新
                    string localName = hit2d.collider.name;
                    //animalNameに格納されている名前が無い場合またはanimalNameの名前とlocalNameが異なる場合
                    if(animalName == null)// || animalName != localName)
                    {
                        animalName = localName;
                    }
                    //もしつなげようとしている動物が一番最初の動物と同じ場合
                    if(hit2d.collider.name == animalName)
                    {
                        //要素の末端に追加する
                        animalInfo.Enqueue(hit2d.collider.gameObject);
                    }
                    ////名前が異なる場合
                    //if (hit2d.collider.name != animalName)
                    //{
                    //    return;
                    //}
                    
                }               
            }
            //マウスを離したor指を離した場合
            if (Input.GetMouseButtonUp(0))
            {
                //スコアを格納したスクリプトをここで参照する
                AnimalController animalController = GameObject.FindWithTag("animal").GetComponent<AnimalController>();
                int score = animalController.Score;
                //animalName = hit2d.collider.name;
                StartCoroutine(ActiveEffect(score, animalName));
            }
        }
        //Debug.Log($"connectCount:{effectCount}");
    }


    //時間計測
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
            if(gameTime <= 0)
            {
                gm.UpdateRanking(_score);
            }
        }
    }

    //BGMプレイ
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

    //スコア処理
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

    private void Effect(GameObject animal)
    {
        //動物の位置にエフェクトを呼び出す
        effect[effectCount].PlayEffect(animal);
        //つなげた数を増やす
        effectCount++;
        Debug.Log(animalInfo.Count);
        //要素の削除
        am.SponeAnimal(animalInfo.Dequeue());
        Debug.Log("処理完了");
        //ここで出力する個数が配列以上になったら0に戻し終了させる
        if (effect.Length == effectCount) effectCount = 0;
    }

    private IEnumerator ActiveEffect(int score, string name)
    {
        //エフェクトを呼び出すオブジェクトの数が0になるまで行う
        while(animalInfo.Count != 0)
        {
            //エフェクト再生
            Effect(animalInfo.Peek());         
            //スコア加算
            GetAnimal(score,name);
        }
        //記憶する名前の初期化
        animalName = null;
        yield return null;
    }
}
