using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectsC : MonoBehaviour
{
    [Header("格納するエフェクト")][SerializeField]
    private Animator effect = null;
    //初期位置のベクトル
    [Header("初期位置")][SerializeField]
    private Vector2 resetPos;

    private void Awake()
    {
        this.transform.position = resetPos;
    }

    public void PlayEffect(GameObject animal)
    {
        //エフェクトの位置を動物の座標に合わせて移動する
        this.transform.position = animal.transform.position;
        //ここでアニメーション内のTriggerを起動させる
        effect.SetTrigger("Play");
        //表示時間を定めるためのコルーチンを起動させる
        StartCoroutine(BackEffect());
    }

    private IEnumerator BackEffect()
    {
        //0.5秒間待機したのちに初期位置に戻る
        yield return new WaitForSeconds(0.5f);
        this.transform.position = resetPos;
    }
}
