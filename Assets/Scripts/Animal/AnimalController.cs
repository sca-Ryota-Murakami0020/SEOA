using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManeger;
using Spine;
using Spine.Unity;

public class AnimalController : MonoBehaviour
{
    //動物のスピード
    [SerializeField] private float normalSpeed;
    //ラム酒を取得している時のスピード
    [SerializeField] private float powerdownSpeed;
    //チェイン中の動物の動き
    [SerializeField] private float slowSpeed;
    //捕まえた判定
    private bool canGet = false;
    //出現が許可された動物かを判定するフラグ
    private bool selectFag;
    //待機時間
    private float waitTime = 0.0f;
    //回転中
    private bool doTurn = false;
    //PlayerPlamate
    [SerializeField] private PlayerPalmate pp;
    //TimeManager
    [SerializeField] private TimeManager tm;
    //AnimalManager
    [SerializeField] private AnimalManager am;
    //SkeletonAnimation
    [SerializeField] private SkeletonAnimation sa;
    //タッチしたときに発光する色
    [SerializeField] private Color changeColor;
    //元の色
    [SerializeField] private Color normalColor;

    public enum DoMove
    {
        NULL,
        OK,
        NOT,
        SLOW
    };

    DoMove canMove = DoMove.NULL;

    #region//プロパティ
    public bool CanGet
    {
        get { return this.canGet;}
        set { this.canGet = value;}
    }

    public bool SelectFlag
    {
        get { return this.selectFag;}
        set { this.selectFag = value;}
    }

    public DoMove Move
    {
        get { return this.canMove;}
        set { this.canMove = value;}
    }

    private void Update()
    {
        CheckMove();
    }
    #endregion

    //状態管理
    private void CheckMove()
    {
        //出現が許可されているオブジェクトであるなら
        if(this.selectFag)
        {
            //状況判断関数
            CheckGame();
            //Debug.Log(tm.DoCount);
        }       
    }

    //ゲームの状態に応じた処理を行う関数
    private void CheckGame()
    {     
        //プレイヤーがスワイプ中なら
        if (pp.DoChain)
        {
            this.canMove = DoMove.SLOW;
        }

        //プレイヤーが繋げていない状態であり、自身がゲーム内に出ることが許可されている個体なら
        if(!pp.DoChain && this.selectFag)
        {
            this.canMove = DoMove.OK;
        }

        //捕まっている状態なら
        if(!this.canGet)
        {
            ChangeColor();
        }
        
        //上記の状態以外の状態なら
        if (canMove != DoMove.NOT && tm.DoCount)
        {
            MoveAnimal();

            //フィーバー演出中orポーズ中なら
            if (!tm.DoCount)
            {
                StopAnimal();
            }
        }
    }

    //当たり判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("animal") && this.selectFag)
        {
            ChangeAngleAnimal();
        }

        if (collision.gameObject.CompareTag("car") && this.selectFag)
        {
            //Debug.Log("弾かれた");
            am.ReturnAnimal(this.gameObject);
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //進行方向に動物がいれば
        if((collision.gameObject.CompareTag("plusAngle")||collision.gameObject.CompareTag("animal"))
            && this.selectFag && !doTurn)
        {
            ChangeAngleAnimal();
        }
    }

    //削除処理
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("OutStage") && this.selectFag)
        {
            am.BackAnimalList(this.gameObject);
        }
    }

    //移動
    private void MoveAnimal()
    {
        //ローカル変数
        float localSpeed = 0.0f;
      
        //プレイヤーがラム酒を取った場合
        if (pp.PlayerSituation == PlayerPalmate.PlayerState.POWERDOWN)
        {
            localSpeed = this.powerdownSpeed;
            //スワイプ中の動物の動き
            if (canMove == DoMove.SLOW)
            {
                localSpeed *= this.slowSpeed;
            }
        }

        //ニュートラルな動き
        if (this.canMove != DoMove.SLOW && pp.PlayerSituation == PlayerPalmate.PlayerState.NULL)
        {
            localSpeed = this.normalSpeed;
            //スワイプ中の動物の動き
            if (canMove == DoMove.SLOW)
            {
                localSpeed *= this.slowSpeed;
            }
        }

        //ここで移動処理を行う
        this.transform.position += this.transform.up * localSpeed;
    }

    //確保された際にオブジェクトのカラーを変更する
    public void ChangeColor() => sa.skeleton.SetColor(changeColor);

    //旋回処理
    public void ChangeAngleAnimal()
    {
        //動物が持つ方向を変数化する
        //StartCoroutine(TurnAnimalAction());
    }

    //スポナーに設置されている動物を動かすために各パラメーターを初期化する
    public void ResetPar()
    {
        this.canMove = DoMove.OK;
        this.canGet = true;
        this.selectFag = true;
        this.sa.skeleton.SetColor(normalColor);
        //Debug.Log("変更は欠けたよ");
    }

    //行動停止時にする処理
    public void StopAnimal()
    {
        this.canMove = DoMove.NOT;
        this.canGet = false;
        this.selectFag = false;
    }

    //ここはgetFlagのみをfalseにしたい時に使う関数
    public void NotGet() => this.canGet = false;

    /*
    //旋回行動
    private IEnumerator TurnAnimalAction()
    {
        //スピードを変更する（減速）
        this.canMove = DoMove.SLOW;
        doTurn = true;
        //追加する角度をランダムで決定する
        float randomNum = Random.Range(1, 2);
        bool pulsRad = false;
        if (randomNum % 2 == 0) pulsRad = true;
        if (randomNum % 2 != 0) pulsRad = false;
        //回転を変数化
        Quaternion animalAngle = this.transform.rotation;
        //決定した角度を動物に付与する（接触するのは同じタグのオブジェクトなので、相手側の判定を行う必要はない）
        int countRad = 0;
        float addRad = 1.0f;
        
        while(countRad < 120)
        {
            if(pulsRad)
            {
                //animalAngle.z += addRad;
                //this.transform.rotation = Quaternion.AngleAxis(addRad, this.transform.forward);Space.Self)
                transform.Rotate(0, 0, addRad);
            }
            if(!pulsRad)
            {
                //animalAngle.z -= addRad;
                //this.transform.rotation = Quaternion.AngleAxis(-addRad, this.transform.forward);
                transform.Rotate(0, 0, -addRad);
            }
            //this.transform.rotation = animalAngle;
            countRad ++;
            yield return new WaitForEndOfFrame();
        }

        //スピードを元の数値に直す
        this.canMove = DoMove.OK;
        doTurn = false;
        yield break;
    }*/
}