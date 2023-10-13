using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using GameManager;

public class PlayerPalmate : MonoBehaviour
{
    #region//参照するスクリプト
    //scoreManager
    [SerializeField] private scoreManager scoreManager;
    //TimeManager
    [SerializeField] private GameManager.TimeManager timeManager;
    //Audiosource
    private AudioSource audios = null;
    //AnimalManager
    [SerializeField] private AnimalManager animalManager;
    //ActiveManager
    [SerializeField] private ActiveManager activeManager;
    //エフェクト
    [Header("呼び出すエフェクト")] [SerializeField]
    private effectsC[] effect;
    #endregion

    #region//変数
    //スワイプ中かの判定
    private bool doSwaip = false;
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
    //獲得した動物
    private Queue<GameObject> animalInfo = new Queue<GameObject>();
    //フィーバー中に獲得した動物
    private Queue<GameObject> fiverAnimalInfo = new Queue<GameObject>();
    #endregion

    #region//インスペクターで変更する値
    //牛のスコア
    [SerializeField] private int cowScore;
    //ネズミのスコア
    [SerializeField] private int mouseScore;
    //エフェクト出現間隔時間
    [SerializeField] private float waitEffectTime;
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
        set { this.doSwaip = value;}
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

    public static PlayerPalmate instance;

    // Start is called before the first frame update
    void Start()
    {
        audios = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeManager.DoCount)
        {
            //操作
            if (Input.GetMouseButton(0))
            {
                TryCatchingAnimals();
            }
            //マウスを離したor指を離した場合
            if (Input.GetMouseButtonUp(0))
            {
                if(timeManager.DoingFiver)
                {
                    StartCoroutine(FiverActiveEffect());
                    //Debug.Log("コルーチン開始");
                }

                else
                {
                    //スコアを格納したスクリプトをここで参照する
                    StartCoroutine(ActiveEffect());
                }
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
        
        //通常の動物判定
        if (hit2d.collider.tag == "animal")
        {
            //Debug.Log("繋げてる");
            doSwaip = true;
            //レイヤーマスクを獲得する
            string nowName = LayerMask.LayerToName(hit2d.collider.gameObject.layer);
            //もし保持している名前が無かったら
            if(this.getAnimalName == GetAnimals.NULL)
            {
                //最初のデータを牛に変更
                if (nowName == LayerMask.LayerToName(6))
                {
                    this.getAnimalName = GetAnimals.Cow;
                }
                //最初のデータをネズミに変更
                if(nowName == LayerMask.LayerToName(7))
                {
                    this.getAnimalName = GetAnimals.Mouse;
                }
            }

            if (this.getAnimalName != GetAnimals.NULL && this.getAnimalName.ToString() == nowName)
            {
                //捕まえた動物の関数を取得する
                //FiverAnimalC fiverAnimalC = hit2d.collider.gameObject.GetComponent<FiverAnimalC>();
                AnimalController ac = hit2d.collider.gameObject.GetComponent<AnimalController>();
                //同じ動物に当たってしまっていたら以降の処理はしない
                if (ac.AlreadyGet) return;

                //もしつなげようとしている動物が一番最初の動物と同じ場合
                if (!ac.AlreadyGet)
                {
                    PlaySE(touchSE);
                    ac.ChangeColor();
                    ac.GetAnimal();
                    //要素の末端に追加する
                    animalInfo.Enqueue(hit2d.collider.gameObject);                    
                    //つなげている数を更新
                    chainCount += 1;
                }
            }
        }

        //フィーバー用の動物を捕まえたら
        if(hit2d.collider.tag == "fiverAnimal")
        {
            doSwaip = true;
            //レイヤーマスクを獲得する
            string getAnimalName = LayerMask.LayerToName(hit2d.collider.gameObject.layer);
            //もし保持している名前が無かったら
            if (this.getAnimalName == GetAnimals.NULL)
            {
                //最初のデータを牛に変更
                if (getAnimalName == LayerMask.LayerToName(6))
                {
                    this.getAnimalName = GetAnimals.Cow;
                }
                //最初のデータをネズミに変更
                if (getAnimalName == LayerMask.LayerToName(7))
                {
                    this.getAnimalName = GetAnimals.Mouse;
                }
            }

            if (this.getAnimalName != GetAnimals.NULL && this.getAnimalName.ToString() == getAnimalName)
            {
                //捕まえた動物の関数を取得する
                //AnimalController animalController = hit2d.collider.gameObject.GetComponent<AnimalController>();
                FeverAnimalC fiverAnimalC = hit2d.collider.gameObject.GetComponent<FeverAnimalC>();

                //同じ動物に当たってしまっていたら以降の処理はしない
                if (fiverAnimalC.AlreadyGet) return;

                //もしつなげようとしている動物が一番最初の動物と同じ場合
                if (!fiverAnimalC.AlreadyGet)
                {
                    PlaySE(touchSE);
                    //Debug.Log("色の変更開始");
                    fiverAnimalC.ChangeColor();
                    fiverAnimalC.NotGet();
                    //要素の末端に追加する
                    fiverAnimalInfo.Enqueue(hit2d.collider.gameObject);
                    Debug.Log("できた:" + fiverAnimalInfo.Peek());
                    //つなげている数を更新
                    chainCount += 1;
                }
            }
        }

        //触れたオブジェクトがアイテムなら
        if(hit2d.collider.tag == "item" && !timeManager.DoingFiver)
        {
            //if(!doSwaip) doSwaip = true;
            //Debug.Log("つながっている");
            //レイヤーマスクを獲得する
            string getItemName = LayerMask.LayerToName(hit2d.collider.gameObject.layer);
            //ラム酒を取得した場合
            if (getItemName == LayerMask.LayerToName(8))
            {
                PlaySE(touchSE);
                getRum = true;
            }
            //カレーを取得した場合
            if (getItemName == LayerMask.LayerToName(9))
            {
                PlaySE(touchSE);
                getCurry = true;
            }           
        }
    }

    //BGMや効果音を呼び出す関数
    public void PlaySE(AudioClip clip)
    {
        if (audios != null)
            audios.PlayOneShot(clip);
    }

    //通常のエフェクト処理
    private void Effect(GameObject animal)
    {
        //動物の位置にエフェクトを呼び出す
        effect[effectCount].PlayEffect(animal);
        effectCount = (effectCount + 1) % effect.Length;
    }

    //フィーバー用のエフェクト表示・スコア加算
    private void FiverEffect(GameObject getAnimal)
    {
        //捕まえた動物の場所にエフェクトを呼び出す
        effect[effectCount].PlayEffect(getAnimal);
        effectCount = (effectCount + 1) % effect.Length;
    }

    //通常の動物の獲得
    private IEnumerator ActiveEffect()
    {
        doSwaip = false;
        AudioClip bgm = null;
        //スコア加算
        //牛のカウント
        if (getAnimalName == GetAnimals.Cow)
        {
            scoreManager.AddScore(cowScore, chainCount);
            bgm = cowSE;
        }
        //ネズミのカウント
        if(getAnimalName == GetAnimals.Mouse)
        {
            scoreManager.AddScore(mouseScore, chainCount);
            bgm = mouseSE;
        }

        //繋げた数を初期化
        chainCount = 0;

        //エフェクトを呼び出すオブジェクトの数が0になるまで行う
        while (animalInfo.Count != 0)
        {
            Effect(animalInfo.Dequeue());
            //動物に応じて呼び出す鳴き声を変更する
            PlaySE(bgm);
            yield return new WaitForSeconds(waitEffectTime);
        }
        //記憶する名前の初期化
        animalName = null;
        getAnimalName = GetAnimals.NULL;
        //カレーを取得している場合
        if(getCurry)
        {
            //フィーバータイムのアニメーションを動かす
            activeManager.ActiveFiverTime();
            getCurry = false;
        }
        yield return null;
    }

    //フィーバー用のエフェクト・スコア処理
    private IEnumerator FiverActiveEffect()
    {
        doSwaip = false;
        //スコア加算
        //牛のカウント
        if (getAnimalName == GetAnimals.Cow)
        {
            scoreManager.FiverAddScore(cowScore, chainCount);
        }
        //ネズミのカウント
        if (getAnimalName == GetAnimals.Mouse)
        {
            scoreManager.FiverAddScore(mouseScore, chainCount);
        }
        //繋げた数を初期化
        chainCount = 0;
        //Debug.Log("animalinfo:" + fiverAnimalInfo.Count);
        //エフェクトを呼び出すオブジェクトの数が0になるまで行う
        while (fiverAnimalInfo.Count != 0)
        {
            //エフェクト再生
            //Debug.Log(animalInfo.Count);
            Debug.Log("kore" + fiverAnimalInfo.Peek());
            FiverEffect(fiverAnimalInfo.Dequeue());
            //動物に応じて呼び出す鳴き声を変更する
            if (getAnimalName == GetAnimals.Cow) PlaySE(cowSE);
            if (getAnimalName == GetAnimals.Mouse) PlaySE(mouseSE);
            yield return new WaitForSeconds(waitEffectTime);
        }
        //記憶する名前の初期化
        animalName = null;
        getAnimalName = GetAnimals.NULL;
        yield return null;
    }
}
