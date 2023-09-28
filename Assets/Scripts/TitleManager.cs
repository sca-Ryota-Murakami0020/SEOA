using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    //フェードイン・フェードアウトを行うCanvas
    [Header("フェード関係用アニメーション")][SerializeField]
    protected Animator fadeAnimation;
    //タイトル用アニメーション
    [Header("タイトル用アニメーション")][SerializeField]
    protected Animator logAnimation;
    //ボタン用のアニメーション
    [Header("ボタン用アニメーション")][SerializeField]
    protected Animator buttonAnimation;

    //タイトルロゴのアニメーションを再生する
    public void StartLogAnimation()
    {
        logAnimation.SetTrigger("StartLogAnimation");
    }

    //ループするボタンのアニメーションを再生する
    public void StartLoopAnimation()
    {
        logAnimation.SetTrigger("LoopingAni");
    }

    //ボタン用のアニメーションを行う
    public void StartButtonAnimation()
    {
        buttonAnimation.SetTrigger("StartButtonAnimation");
    }

    //フェードインを行う
    public void StartFadeIn()
    {
        fadeAnimation.SetTrigger("StartFadeIn");
    }

    //フェードアウトを行う
    public void StartFadeOut()
    {
        fadeAnimation.SetTrigger("StartFadeOut");
    }

    //タイトルからゲームへ
    public void GoGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
