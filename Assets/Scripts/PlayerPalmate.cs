using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using GameManeger;

public class PlayerPalmate : MonoBehaviour
{
    #region//参照するスクリプト
    //scoreManager
    [SerializeField] private scoreManager sm;
    //TimeManager
    [SerializeField] private TimeManager tm;
    //GameManager
    [SerializeField] private GameManager gm;
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
    //カレーを取得した場合
    private bool getCurry = false;
    //ラム酒を取得した場合
    private bool getRum = false;
    //獲得した動物の名前
    private string animalName = null;
    //エフェクトを呼び出した回数
    private int effectCount = 0;
    //つなげた数
    private int chainCount = 0;
    //獲得した
    private Queue<GameObject> animalInfo = new Queue<GameObject>();
    #endregion

    #region//インスペクターで変更する値
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
    private PlayerState playerState = PlayerState.NULL;
    private GetAnimals getAnimalName = GetAnimals.NULL;
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

    #endregion

    #region//プロパティ
    public int EffectCount
    {
        get { return this.effectCount;}
        set { this.effectCount = value;}
    }

    public int ChainCount
    {
        get { return this.chainCount;}
        set { this.chainCount = value;}
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
        audios = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(tm.DoCount)
        {
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
                    animalController.NotGet();
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

    private IEnumerator ActiveEffect()
    {
        doSwaip = false;
        //スコア加算
        //牛のカウント
        if(getAnimalName == GetAnimals.Cow)
        {
            sm.AddScore(cowScore, chainCount);
        }
        //ネズミのカウント
        if(getAnimalName == GetAnimals.Mouse)
        {
            sm.AddScore(mouseScore, chainCount);
        }
        //繋げた数を初期化
        chainCount = 0;
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
