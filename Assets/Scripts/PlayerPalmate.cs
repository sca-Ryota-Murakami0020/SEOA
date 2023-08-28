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
    [Header("呼び出すエフェクト")] [SerializeField]
    private effectsC[] effect;
    //カウントダウン演出用アニメーション
    //[Header("カウントダウンアニメーション")][SerializeField]
    //private Animation countDwonAnim;
    //feberC
    //[Header("フィーバー演出関係を管理するスクリプト")][SerializeField]
    //private feberC fc;
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
    //カレーを取得した場合
    private bool getCurry;
    //ラム酒を取得した場合
    private bool getRum;
    //確保間隔を設ける
    private bool canAnimal;
    //獲得した動物の名前
    private string animalName;
    //エフェクトを呼び出した回数
    private int effectCount;
    //1ゲーム中の獲得スコア
    private int _score;
    //獲得予定のスコア
    private int getScore;
    //つなげた数
    private int chainCount;
    //ゲーム時間(画像で数字の表示をおこなうため、整数型の方が実装しやすいから)
    private int gameTime;
    //表示する画像の番地数を格納する変数
    private int spriteCount;
    //確保間隔時間
    [SerializeField] private float waitTime;
    //細かい時間経過をおこなうために用いる小数型の変数
    private float countTime;
    //エフェクト出現間隔時間
    [SerializeField] private float waitEffectTime;
    //1フレーム前のスワイプの位置
    private Vector2 oldFlameTouchPos;
    //現在のスワイプ位置
    private Vector2 nowTouchPos;
    //現在つなげる始点になっている動物の位置
    private Vector2 oldTouchPos;
    //捕まえた動物の場所（動物は常に動くのでオブジェクトで格納する方が正確な位置にエフェクトを呼び出せる）
    private Queue<GameObject> animalInfo;
    //数字のイメージ
    [Header("表示する数字のImage")] [SerializeField] 
    private Sprite[] numberImage;
    //数字の配置位置
    [Header("表示するImageの配置位置")] [SerializeField] 
    private Image[] imageNumber;
    //タッチ関数
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

    public int ChainCount
    {
        get { return this.effectCount;}
        set { this.effectCount = value;}
    }

    public bool DoStop
    {
        get { return this.doStop;}
        set { this.doStop = value;}
    }

    public Queue<GameObject> Animals
    {
        get { return this.animalInfo;}
        set { this.animalInfo = value;}
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
        effectCount = 0;
        chainCount = 0;
        doStop = true;
        doPoworUp = false;
        doPoworDwon = false;
        animalName = null;
        canAnimal = true;
        animalInfo = new Queue<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(doStop)
        {
            StartCoroutine(StartCountDown());
        }

        if(!doStop)
        {
            //時間計測
            CountTime();
            //BGMを流す関数
            PlayBGM();
            //操作
            if (Input.GetMouseButton(0))
            {
                TryCatchingAnimals();
            }
            //マウスを離したor指を離した場合
            if (Input.GetMouseButtonUp(0))
            {
                //スコアを格納したスクリプトをここで参照する
                //AnimalController animalController = GameObject.FindWithTag("animal").GetComponent<AnimalController>();
                //int score = animalController.Score;
                //animalName = hit2d.collider.name;
                //if(getCurry) DoPowerUp();
                //if(getRum) DoPowerDown();
                StartCoroutine(ActiveEffect());
            }
        }
        //Debug.Log($"connectCount:{effectCount}");
    }

    private void TryCatchingAnimals()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 100);
        //何もないところをタップorスワイプ中なら
        if (hit2d.collider == null)
        {
            return;
        }
        if (hit2d.collider.tag == "animal")
        {
            AnimalController animalController = 
            //記憶する名前の更新
            string localName = animalController.Name;
            //animalNameに格納されている名前が無い場合またはanimalNameの名前とlocalNameが異なる場合
            if (animalName == null)// || animalName != localName)
            {
                animalName = localName;
            }

            //同じ動物に当たってしまっていたら以降の処理はしない
            if(!animalController.CanGet) return;

            //もしつなげようとしている動物が一番最初の動物と同じ場合
            if (hit2d.collider.name == animalName && canAnimal)
            {
                PlayBGM(touchSE);

                getScore = animalController.Score;
                //要素の末端に追加する
                animalInfo.Enqueue(hit2d.collider.gameObject);
                //am.GetAnimals.Enqueue(hit2d.collider.gameObject);
                //getScore = localScore;
                //つなげている数を記憶
                chainCount += 1;
                //Debug.Log(chainCount);
                StartCoroutine(DerayTime());
            }

        }
        //カレーを取得した場合
        if (hit2d.collider.tag == "Curry")
        {
            PlayBGM(touchSE);
            getCurry = true;
        }
        //ラム酒を取得した場合
        if (hit2d.collider.tag == "Rum")
        {
            PlayBGM(touchSE);
            getRum = true;
        }
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
        //if(doPoworUp)
        //{

        //}
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
    void GetAnimal(int score, int count)
    {
        //フィーバー中のスコア計算
        //if(doPoworUp) _score += (int)(score + score * 0.5);
        //デバフ付与中のスコア計算
        //if(doPoworDwon) _score += (int)(score - score * 0.3);

        //通常のスコア加算
        //タップで終わらせている時
        if(count == 1)
        {
            _score += score;
        }
        //つなげている状態
        if(count >= 2)
        {
            _score += score * count + (int)((score * count) * 0.1);
        }
        
        sm.UpdateScore(_score);
        //チェイン数を初期化
        chainCount = 0;
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
        //Debug.Log(animalInfo.Count);
        //要素の削除
        am.SponeAnimal(animalInfo.Dequeue());
        //Debug.Log("処理完了");
        //ここで出力する個数が配列以上になったら0に戻し終了させる
        if (effect.Length == effectCount) effectCount = 0;
    }

    private void DoPowerUp()
    {
        doPoworUp = true;
        //fc.StartFeber();
    }

    private void DoPowerDown()
    {
        doPoworDwon = false;
    }

    private IEnumerator StartCountDown()
    {
        for(int count = 3; count >= 0; count--)
        {
            Debug.Log(count);
            yield return new WaitForSeconds(1);
        }
        doStop = false;
    }

    public void StartCountDownAnim()
    {

    }

    public void ReturnGame()
    {
        doStop = false;
    }

    private IEnumerator ActiveEffect()
    {
        //スコア加算
        Debug.Log("chainCount" + chainCount);
        GetAnimal(getScore, chainCount);
        
        //エフェクトを呼び出すオブジェクトの数が0になるまで行う
        while (animalInfo.Count != 0)
        {
            //エフェクト再生
            Effect(animalInfo.Peek());
            //動物に応じて呼び出す鳴き声を変更する
            if (animalName == "Cow") PlayBGM(cowSE);
            if (animalName == "Mouse") PlayBGM(mouseSE);
            yield return new WaitForSeconds(waitEffectTime);
        }
        //記憶する名前の初期化
        animalName = null;
        yield return null;
    }

    private IEnumerator DerayTime()
    {
        canAnimal = false;
        yield return new WaitForSeconds(waitTime);
        canAnimal = true;
        yield return null;
    }
}
