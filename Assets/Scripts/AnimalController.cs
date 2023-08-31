using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    //動物のスピード
    [SerializeField] private float speed;
    //チェイン中の動物の動き
    [SerializeField] private float slowSpeed;
    //捕まえた判定
    private bool canGet = true;

    public enum DoMove
    {
        NULL,
        OK,
        NOT,
        SLOW
    };

    DoMove canMove = DoMove.OK;

    public bool CanGet
    {
        get { return this.canGet;}
        set { this.canGet = value;}
    }

    public DoMove Move
    {
        get { return this.canMove;}
        set { this.canMove = value;}
    }

    private void Update()
    {
        if(canMove == DoMove.OK)
        {
            //スワイプ中の動物の動き
            if(canMove == DoMove.SLOW)
            {
                this.transform.position += this.transform.up * slowSpeed;
            }
            //その他の時の動き
            if(canMove != DoMove.SLOW)
            {
                this.transform.position += this.transform.up * speed;
            }
            
        }
    }

    public void ResetPositionAnimal(int count)
    {

    }

    //スポナーに設置されている動物を動かすために各パラメーターを初期化する
    public void ResetPar()
    {
        canMove = DoMove.OK;
        canGet = true;
    }

    //行動停止時にする処理
    public void StopAnimal()
    {
        canMove = DoMove.NOT;
    }
}
