using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManager;
using Spine;
using Spine.Unity;

public class AnimalController : MonoBehaviour
{
    //
    private TimeManager tm;
    //
    private PlayerPalmate pp;
    //
    private AnimalManager am;

    //捕まえられたフラグ
    private bool alreadyGet = false;
    //速度
    [SerializeField] private float normalSpeed;
    //デバフ中の速度
    [SerializeField] private float upSpeed;
    //スワイプ中のスピード
    [SerializeField] private float swaipSpeed;

    //SkeletonAnimation
    [SerializeField] private SkeletonAnimation sa;
    //タッチしたときに発光する色
    [SerializeField] private Color changeColor;
    //元の色
    [SerializeField] private Color normalColor;
    //動物の当たり判定
    [SerializeField] private Collider2D animalCollider2d;
    //動物のRigidbody2d
    [SerializeField] private Rigidbody2D rb2d;

    //ステータス（プレイヤーの状態による）
    public enum DoMove
    {
        NOT,
        MOVE,
        QUICK
        //SLOW
    };

    DoMove canMove = DoMove.NOT;

    #region//プロパティ
    public bool AlreadyGet
    {
        get { return this.alreadyGet; }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        tm = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        pp = GameObject.Find("Player").GetComponent<PlayerPalmate>();
        am = GameObject.Find("AnimalManagerObject").GetComponent<AnimalManager>();
    }

    private void Update()
    {
        //ゲームが進行中なら
        if (tm.DoCount)
        {
            SelectMove();
        }

        //Debug.Log("debag" + TimeManager.instance.DoCount);

        //ゲーム進行が中断されたら
        if (!tm.DoCount && !tm.DoingFiver)
        {
            this.canMove = DoMove.NOT;
        }

        if(tm.DoCount && !tm.DoingFiver)
        {
            this.canMove = DoMove.MOVE;
        }

        //フィーバーになったらこのオブジェクトを破壊する
        if (tm.DoingFiver)
        {
            Destroy(this.gameObject);
        }
    }

    //行動の条件分岐を行う
    private void SelectMove()
    {
        //行動方法の選択
        switch (canMove)
        {
            //ゲーム停止中
            case DoMove.NOT:
                StopAnimal();
                break;
            //通所の動き
            case DoMove.MOVE:
                Move();
                break;
            //プレイヤーがデバフ中なら
            case DoMove.QUICK:
                QuickMove();
                break;
            //繋げている状態
            //case DoMove.SLOW:
                //SlowMove();
                //break;
        }

        //捕まっている状態なら
        if (this.alreadyGet)
        {
            ChangeColor();
        }
    }

    //メニュー開示中の動き
    private void StopAnimal() => this.transform.position += this.transform.up * 0.0f;

    //通常の動き
    private void Move() => this.transform.position += this.transform.up * normalSpeed;

    //デバフ中の動き
    private void QuickMove() => this.transform.position += this.transform.up * upSpeed;

    //繋げている時の動き
    //private void SlowMove() => this.transform.position += this.transform.up * normalSpeed * swaipSpeed;

    //捕まっている状態の時に色を変える
    //確保された際にオブジェクトのカラーを変更する
    public void ChangeColor() => sa.skeleton.SetColor(changeColor);

    //確保済みにする
    public void GetAnimal() => this.alreadyGet = true;

    //当たり判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("animal"))
        {
            ActiveisTriggerAnimal();
        }

        if (collision.gameObject.CompareTag("car"))
        {
            //次のスポナーに動物を生成させる
            am.SponeAnimal();
            Destroy(this.gameObject);
        }
    }
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //進行方向に動物がいれば
        if((collision.gameObject.CompareTag("plusAngle")||collision.gameObject.CompareTag("animal")))
        {
            NoActiveisTriggerAnimal();
        }
    }

    //削除処理
    private void OnTriggerExit2D(Collider2D collision)
    {
        //画面外に出た場合、リストの末端に戻る
        if(collision.gameObject.CompareTag("OutStage"))
        {
            am.SponeAnimal();
            Destroy(this.gameObject);
        }
        //動物同士がすり抜けあっていたら
        if (collision.gameObject.CompareTag("animal"))
        {
            ActiveisTriggerAnimal();
        }
    }

    //当たり判定付与
    public void ActiveisTriggerAnimal()
    {
        //動物が持つ方向を変数化する
        animalCollider2d.isTrigger = true;
        rb2d.isKinematic = true;
    }

    //当たり判定消去
    public void NoActiveisTriggerAnimal()
    {
        animalCollider2d.isTrigger = false;
        rb2d.isKinematic = true;
        //Debug.Log("当たり判定消滅");
    }
}