using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManeger;

public class RumC : MonoBehaviour
{
    [SerializeField] private TimeManager tm;

    [SerializeField] private GameObject resetPos; 

    [SerializeField] private GameObject[] rumSponer;

    [SerializeField] private float _speed;

    private bool canGet = false;

    private bool moveLeft = false;

    public bool CanGet
    {
        get { return this.canGet;}
        set { this.canGet = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetRumPos();
    }

    // Update is called once per frame
    void Update()
    {
        if(canGet && tm.DoCount)
        {
            MoveRum();
        }
    }

    public void SetRum()
    {
        //スポナーの位置を決める
        int sponerPos = Random.Range(0, rumSponer.Length);
        //スポナーへ移動
        this.transform.position = rumSponer[sponerPos].transform.position;
        //選ばれたスポナーが右側の場合
        if (sponerPos == 2 || sponerPos == 3)
        {
            //カレーの進行方向を左方向に限定する
            MoveLeft();
        }
        //カレーの進行を許可する
        ActiveCanFlag();
    }

    private void MoveLeft()
    {
        moveLeft = true;
    }

    private void MoveRum()
    {
        //左方向
        if (moveLeft) this.transform.position += this.transform.right * _speed * -1;
        //右方向
        if (!moveLeft) this.transform.position += this.transform.right * _speed;
    }

    private void ActiveCanFlag()
    {
        canGet = true;
    }

    private void ResetRumPos()
    {
        this.transform.position = resetPos.transform.position;
    }

    //衝突判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //車に当たったらカレーを初期位置に戻す
        if (collision.gameObject.CompareTag("car"))
        {
            ResetRumPos();
        }
    }

    //削除処理
    private void OnTriggerExit2D(Collider2D collision)
    {
        //初期位置に戻す
        if (collision.gameObject.CompareTag("OutStage"))
        {
            ResetRumPos();
        }
    }
}
