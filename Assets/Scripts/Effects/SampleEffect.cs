using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEffect : MonoBehaviour
{
    //エフェクト
    [Header("呼び出すエフェクト")][SerializeField] private PlayEffect[] effects;
    //現在のエフェクト
    private int effectCurrent = 0;
    //エフェクト再生する場所
    private Queue<Vector2> animalInfo = new Queue<Vector2>();

    //エフェクト再生中か
    private bool isEffect = false;


    private void Update()
    {
        if (isEffect) return; // エフェクト再生中は処理しない

        if (Input.GetKeyDown(KeyCode.A))
        {
            //座標の記憶
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            animalInfo.Enqueue(pos);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //演出処理開始
            Debug.Log("StartEffect");
            StartCoroutine(ActiveEffect());
        }
    }
    private void Effect(Vector2 pos)
    {
        // エフェクト再生する(座標)
        effects[effectCurrent].Play(pos);
        // 次のエフェクト再生する準備
        effectCurrent++;
        //エフェクトの呼び出しが一通り終わったらカウントを0にする
        if (effectCurrent == effects.Length) effectCurrent = 0;
    }

    private IEnumerator ActiveEffect()
    {
        //エフェクト再生中の判定にする
        isEffect = true;
        //Queueの中身の要素数が0ではない限り処理を行う
        while (animalInfo.Count != 0)
        {
            //エフェクト再生
            Effect(animalInfo.Dequeue());
            yield return new WaitForSeconds(0.1f);
        }
        //処理が終了したらエフェクト再生を終了する
        isEffect = false;
    }
}
