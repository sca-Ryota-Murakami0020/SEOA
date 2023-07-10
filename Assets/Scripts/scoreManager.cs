using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreManager : MonoBehaviour
{
    //PlayerPalmate
    private PlayerPalmate pp;
    //数字の画像
    [SerializeField] private Sprite[] numberImage;
    //数字の配置位置
    [SerializeField] private Image[] imageNumber;
    //カンマの配置
    [SerializeField] private Image[] commaImage;

    // Start is called before the first frame update
    void Start()
    {
        //参照
        pp = GameObject.Find("Player").GetComponent<PlayerPalmate>();

        //数値の初期化
        for(int count = 0; count < imageNumber.Length; count++)
        { 
            imageNumber[count].sprite = numberImage[0];
            //スコアを初期表示(0,000)にする
            if(count < 3)imageNumber[count].enabled = true;
            if(count >= 4) imageNumber[count].enabled = false;
        }

        //コンマ表示の初期化
        commaImage[1].enabled = false;
    }

    //スコア表示の更新
    public void UpdateScore(int score)
    {
        Debug.Log(score);
        //各桁の計算をおこなうために使う変数
        int count = 10;
        //１桁目から計算をおこなう
        for(int imageCount = 0;imageCount < imageNumber.Length;imageCount++)
        {
            //１の位の計算
            if(imageCount <= 0)
            {
                //仮に計算結果が9を超えているor0を下回っている場合0にする値
                int showScore = score / count;
                if(showScore < 0　|| showScore > imageNumber.Length)showScore =0;
                imageNumber[imageCount].sprite = numberImage[showScore];
                Debug.Log("１の桁：" + showScore);
            }
            //１０以上の位の計算
            if(imageCount >= 1)
            {
                //仮に計算結果が9を超えているor0を下回っている場合0にする値
                int showScore = score / count;
                if (showScore < 0 || showScore > imageNumber.Length) showScore = 0;
                imageNumber[imageCount].sprite = numberImage[showScore];
                Debug.Log(count + "の桁：" + showScore);
                count *= 10;
            }
            //ここで計算する桁が増加するのでcountも増加させる
            
        }
    }

    //表示するスコアの桁数・コンマの表示を更新
    public void ShowScore(int score)
    {
        //獲得スコアに応じて表示するスコアの桁数を更新する
        int showScoreNumber = score / 10;
        if(showScoreNumber >= 4)
        {
            if(showScoreNumber >= 6 && commaImage[1].enabled == false)
            {
                commaImage[1].enabled = true;
            }
            imageNumber[showScoreNumber].enabled = true;
        }
    }
}
