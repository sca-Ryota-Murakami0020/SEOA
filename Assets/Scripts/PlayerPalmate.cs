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
    //ゲーム開始３カウント前の操作できない状態
    private bool dontStart;
    //メニューを開いている状態
    private bool openMenu;
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
    //つなげた数
    private int chainCount;
    //ゲーム時間(画像で数字の表示をおこなうため、整数型の方が実装しやすいから)
    private int gameTime;
    //表示する画像の番地数を格納する変数
    private int spriteCount;
    //細かい時間経過をおこなうために用いる小数型の変数
    private float countTime;
    /*
    //1フレーム前のスワイプの位置
    private Vector2 oldFlameTouchPos;
    //現在のスワイプ位置
    private Vector2 nowTouchPos;
    //現在つなげる始点になっている動物の位置
    private Vector2 oldTouchPos;
    */
    //捕まえた動物の場所（動物は常に動くのでオブジェクトで格納する方が正確な位置にエフェクトを呼び出せる）
    private Queue<GameObject> animalInfo;
    
    //タッチ関数
    private Touch touch;
    #endregion

    #region//インスペクターで変更する値
    //確保間隔時間
    [SerializeField] private float waitTime;
    //牛のスコア
    [SerializeField] private int cowScore;
    //ネズミのスコア
    [SerializeField] private int mouseScore;
    //エフェクト出現間隔時間
    [SerializeField] private float waitEffectTime;
    //パワーアップ時間
    [SerializeField] private float powerupTime;
    //パワーダウン時間
    [SerializeField] private float powerdownTime;
    //数字のイメージ
    [Header("表示する数字のImage")]
    [SerializeField]
    private Sprite[] numberImage;
    //数字の配置位置
    [Header("表示するImageの配置位置")]
    [SerializeField]
    private Image[] imageNumber;
    #endregion

    #region//クラス
    //プレイヤーの状態
    public enum PlayerState
    {
        NULL,
        POWERUP,
        POWERDOWN
    };

    private enum GetAnimals
    {
        NULL,
        Cow,
        Mouse
    };

    //変数
    private PlayerState playerState;
    private GetAnimals getAnimalName;
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

    #region//プロパティ
    public int Score {
        get { return this._score; }
        set { this._score = value; }
    }

    public int ChainCount
    {
        get { return this.effectCount;}
        set { this.effectCount = value;}
    }

    public bool DontStart
    {
        get { return this.dontStart;}
        set { this.dontStart = value;}
    }

    public bool OpenMenu
    {
        get { return this.openMenu;}
        set { this.openMenu = value;}
    }

    public bool DoChain
    {
        get { return this.doSwaip;}
    }

    public PlayerState PlayerSituation
    {
        get { return this.playerState;}
    }

    public Queue<GameObject> Animals
    {
        get { return this.animalInfo;}
        set { this.animalInfo = value;}
    }
    #endregion

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

        dontStart = true;
        doPoworUp = false;
        doPoworDwon = false;
        canAnimal = true;
        openMenu = false;

        animalName = null;
        playerState = PlayerState.NULL;
        getAnimalName = GetAnimals.NULL;
        animalInfo = new Queue<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if((!dontStart) && (!openMenu))
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
                //if(getCurry) DoPowerUp();
                //if(getRum) DoPowerDown();
                StartCoroutine(ActiveEffect());
            }
        }
    }

    //スワイプ処理
    private void TryCatchingAnimals()
    {       
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 100 );
        //何もないところをタップorスワイプ中なら
        if (hit2d.collider == null)
        {
            return;
        }
        
        if (hit2d.collider.tag == "animal")
        {
            //Debug.Log("繋げてる");
            doSwaip = true;
            //レイヤーマスクを獲得する
            string getAnimalName = LayerMask.LayerToName(hit2d.collider.gameObject.layer);
            //もし保持している名前が無かったら
            if(this.getAnimalName == GetAnimals.NULL)
            {
                //最初のデータを牛に変更
                if (getAnimalName == LayerMask.LayerToName(6))
                {
                    this.getAnimalName = GetAnimals.Cow;
                }
                //最初のデータをネズミに変更
                if(getAnimalName == LayerMask.LayerToName(7))
                {
                    this.getAnimalName = GetAnimals.Mouse;
                }
            }

            if (this.getAnimalName != GetAnimals.NULL && this.getAnimalName.ToString() == getAnimalName)
            {
                //捕まえた動物の関数を取得する
                AnimalController animalController = hit2d.collider.gameObject.GetComponent<AnimalController>();

                //同じ動物に当たってしまっていたら以降の処理はしない
                if (!animalController.CanGet) return;

                //もしつなげようとしている動物が一番最初の動物と同じ場合
                if (animalController.CanGet)
                {
                    PlayBGM(touchSE);
                    //Debug.Log("色の変更開始");
                    animalController.ChangeColor();
                    animalController.CanselGet();
                    //捕まえた動物の捕まえたフラグをfalseにする
                    //animalController.CanGet = false;
                    //Debug.Log("canGet:" + animalController.CanGet);
                    //要素の末端に追加する
                    animalInfo.Enqueue(hit2d.collider.gameObject);                    
                    //つなげている数を更新
                    chainCount += 1;
                }
            }
        }

        //触れたオブジェクトがアイテムなら
        if(hit2d.collider.tag == "Item")
        {
            if(!doSwaip) doSwaip = true;
            //レイヤーマスクを獲得する
            string getItemName = LayerMask.LayerToName(hit2d.collider.gameObject.layer);
            //ラム酒を取得した場合
            if (getItemName == LayerMask.LayerToName(8))
            {
                PlayBGM(touchSE);
                getRum = true;
            }
            //カレーを取得した場合
            if (getItemName == LayerMask.LayerToName(9))
            {
                PlayBGM(touchSE);
                getCurry = true;
            }           
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

    //牛のスコア処理
    void GetCow(int count)
    {
        //フィーバー中のスコア計算
        //if(doPoworUp) _score += (int)(score + score * 0.5);
        //デバフ付与中のスコア計算
        //if(doPoworDwon) _score += (int)(score - score * 0.3);

        //通常のスコア加算
        //タップで終わらせている時
        if(count == 1)
        {
            _score += cowScore;
        }
        //つなげている状態
        if(count >= 2)
        {
            _score += cowScore * count + (int)((cowScore * count) * 0.1);
        }
        
        sm.UpdateScore(_score);
        //チェイン数を初期化
        chainCount = 0;
    }

    //ネズミのスコア加算
    void GetMouse(int count)
    {
        //通常のスコア加算
        //タップで終わらせている時
        if (count == 1)
        {
            _score += mouseScore;
        }
        //つなげている状態
        if (count >= 2)
        {
            _score += mouseScore * count + (int)((mouseScore * count) * 0.1);
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
        //要素の削除
        am.SponeAnimal(animalInfo.Dequeue());
        //Debug.Log("処理完了");
        //ここで出力する個数が配列以上になったら0に戻し終了させる
        if (effect.Length == effectCount) effectCount = 0;
    }

    //パワーアップ処理
    private void DoPowerUp()
    {
        StartCoroutine(PowerUpTime());
    }

    //パワーダウン処理
    private void DoPowerDown()
    {
        StartCoroutine(PowerDownTime());
    }

    //メニュー中
    public void PlayOpenMenu()
    {
        openMenu = true;
    }

    //メニューからゲームに戻る
    public void BackGameFromMenu()
    {
        openMenu = false;
    }

    private IEnumerator ActiveEffect()
    {
        doSwaip = false;
        //スコア加算
        //Debug.Log("chainCount" + chainCount);
        if(getAnimalName == GetAnimals.Cow)
        {
            GetCow(chainCount);
        }
        if(getAnimalName == GetAnimals.Mouse)
        {
            GetMouse(chainCount);
        }
        
        //エフェクトを呼び出すオブジェクトの数が0になるまで行う
        while (animalInfo.Count != 0)
        {
            //エフェクト再生
            //Debug.Log(animalInfo.Count);
            Effect(animalInfo.Peek());
            //動物に応じて呼び出す鳴き声を変更する
            if (getAnimalName == GetAnimals.Cow) PlayBGM(cowSE);
            if (getAnimalName == GetAnimals.Mouse) PlayBGM(mouseSE);
            yield return new WaitForSeconds(waitEffectTime);
        }
        //記憶する名前の初期化
        animalName = null;
        getAnimalName = GetAnimals.NULL;
        yield return null;
    }

    //パワーアップ処理（指定秒数間行う処理）
    private IEnumerator PowerUpTime()
    {
        playerState = PlayerState.POWERUP;
        yield return new WaitForSeconds(powerupTime);
        getCurry = false;
        playerState = PlayerState.NULL;
    }

    //パワーダウン処理（指定秒数間行う処理）
    private IEnumerator PowerDownTime()
    {
        playerState = PlayerState.POWERDOWN;
        yield return new WaitForSeconds(powerdownTime);
        getRum = false;
        playerState = PlayerState.NULL;
    }
}
