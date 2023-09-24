using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManeger;

public class CurryMoverC : MonoBehaviour
{
    //カレーの挙動の許可
    private bool canMove = false;
    //左へ動くかの判定
    private bool moveLeft = false;
    //カレーのスピード
    [SerializeField] private float _speed;
    //カレーの出現回数
    private int sponeCurryCount = 0;
    //TimeManager
    [SerializeField] private TimeManager tm;
    //初期位置
    [SerializeField] private GameObject resetPosObject;
    //スポナー
    [SerializeField] private GameObject[] currySponer;

    //プロパティ
    public bool CanMove
    {
        get { return this.canMove;}
    }

    // Start is called before the first frame update
    void Start()
    {
        //初期位置に配置
        ResetCurryPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && tm.DoCount)
        {
            MoveCurry();
        }
    }

    //衝突判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //車に当たったらカレーを初期位置に戻す
        if(collision.gameObject.CompareTag("car"))
        {
            ResetCurryPos();
        }
    }

    //削除処理
    private void OnTriggerExit2D(Collider2D collision)
    {
        //初期位置に戻す
        if (collision.gameObject.CompareTag("OutStage"))
        {
            ResetCurryPos();
        }
    }

    //行動を許可する
    public void ActiveCanFlag()
    {
        canMove = true;
    }

    //行動を制限する
    public void NoActiveCanFlag()
    {
        canMove = false;
    }

    //進行方向を左にする
    public void MoveLeft()
    {
        moveLeft = true;
    }

    //カレーの挙動
    public void MoveCurry()
    {
        //左方向
        if(moveLeft) this.transform.position += this.transform.right * _speed * -1;
        //右方向
        if(!moveLeft) this.transform.position += this.transform.right * _speed ;
    }

    //初期位置に戻す
    public void ResetCurryPos()
    {
        Vector2 resetPos = resetPosObject.transform.position;
        this.transform.position = resetPos;
        //行動を制限する
        NoActiveCanFlag();
    }

    //スポナーの位置に配置する
    public void SetCurry()
    {
        if(sponeCurryCount <= 2)
        {
            //スポナーの位置を決める
            int sponerPos = Random.Range(0, currySponer.Length);
            //スポナーへ移動
            this.transform.position = currySponer[sponerPos].transform.position;
            //選ばれたスポナーが右側の場合
            if (sponerPos == 2 || sponerPos == 3)
            {
                //カレーの進行方向を左方向に限定する
                MoveLeft();
            }
            //カレーの進行を許可する
            ActiveCanFlag();
            //出現回数を増やす
            sponeCurryCount++;
        }     
    }
}
