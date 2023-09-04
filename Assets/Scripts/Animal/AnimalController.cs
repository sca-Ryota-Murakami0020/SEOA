using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    //ステージに残留できる時間
    [SerializeField] private float stageAlive;
    //捕まえた判定
    private bool canGet = false;
    //出現が許可された動物かを判定するフラグ
    private bool selectFag = false;
    //映っているかのフラグ
    private bool renderFlag = false;
    //PlayerPlamate
    [SerializeField] private PlayerPalmate pp;
    //AnimalManager
    [SerializeField] private AnimalManager am;
    //SkeletonAnimation
    [SerializeField] private SkeletonAnimation sa;
    //タッチしたときに発光する色
    [SerializeField] private Color changeColor;
    //元の色
    [SerializeField] private Color normalColor;
    //動物のレンダー
    [SerializeField] private Renderer animalRenderer;

    public enum DoMove
    {
        NULL,
        OK,
        NOT,
        SLOW
    };

    DoMove canMove = DoMove.NULL;

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

    public bool RenderFlag
    {
        get { return this.renderFlag;}
        set { this.renderFlag = value;}
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

    //状態管理
    private void CheckMove()
    {
        //出現が許可されているオブジェクトであるなら
        if(this.selectFag)
        {
            //状況判断関数
            CheckGame();   
            //Debug.Log(renderFlag);
        }       
    }

    //ゲームの状態に応じた処理を行う関数
    private void CheckGame()
    {
        //ゲーム開始時点またはフィーバー演出中なら
        if (pp.DontStart && pp.OpenMenu)
        {
            StopAnimal();
        }

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
        if (canMove == DoMove.OK)
        {
            MoveAnimal();
        }

        if(this.animalRenderer.isVisible)
        {
            this.renderFlag = true;
        }

        if(!this.animalRenderer.isVisible)
        {
            this.renderFlag = false;
            Debug.Log("映ってないよ");
        }

        //もし捕まらずに画面外に出た場合
        if(!renderFlag && this.selectFag)
        {
            am.ReturnAnimal(this.gameObject);
        }
    }

    //当たり判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("animal") && this.selectFag)
        {
            //Debug.Log("反転開始");
            //追加する角度をランダムで決定する
            int randomRad = Random.Range(-90, 90);
            //動物が持つ方向を変数化する
            Quaternion animalAngle = this.transform.rotation;
            //決定した角度を動物に付与する（接触するのは同じタグのオブジェクトなので、相手側の判定を行う必要はない）
            animalAngle.z += randomRad;
            //Debug.Log("反転完了");
        }

        if (collision.gameObject.CompareTag("car") && this.selectFag)
        {
            //Debug.Log("弾かれた");
            am.ReturnAnimal(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("OutStage") && this.selectFag)
        {
            am.BackAnimalList(this.gameObject);
            Debug.Log("動物呼び出し");
        }
    }

    //挙動
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

    //スポナーに設置されている動物を動かすために各パラメーターを初期化する
    public void ResetPar()
    {
        this.canMove = DoMove.OK;
        this.canGet = true;
        this.sa.skeleton.SetColor(normalColor);
    }

    //行動停止時にする処理
    public void StopAnimal()
    {
        this.canMove = DoMove.NOT;
        this.canGet = false;
        this.selectFag = false;
    }

    //ここはgetFlagのみをfalseにしたい時に使う関数
    public void CanselGet() => this.canGet = false;

    //確保された際にオブジェクトのカラーを変更する
    public void ChangeColor() => sa.skeleton.SetColor(changeColor);

    //見えている時
    private void OnBecameVisible()
    {
        
    }

    //見えていない時
    private void OnBecameInvisible()
    {
        
    }

}