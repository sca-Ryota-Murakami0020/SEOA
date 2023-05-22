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
        }

        //コンマ表示の初期化
        for(int count = 0;count < commaImage.Length;count++)
        {
            commaImage[count].enabled = false;
        }
    }

    //スコア表示の更新
    public void UpdateScore(int score)
    {
        int count = 10;
        for(int imageCount = 0;imageCount < imageNumber.Length;imageCount++)
        {
            //１の位の計算
            if(imageCount == 0)
            {
                imageNumber[imageCount].sprite = numberImage[score % count];
            }
            //１０以上の位の計算
            else
            {
                imageNumber[imageCount].sprite = numberImage[score / count];
                Debug.Log(imageCount);
            }
            count *= 10;
        }
    }

    //コンマの表示
    public void ShowComma()
    {
        int commaCount = 0;
        for(int checkScore = 1000000;checkScore >= 1000;checkScore /= 1000)
        {
            if (pp.Score / checkScore >= 1)
            {
                commaImage[commaCount].enabled = true;
                commaCount++;
            }
        }
    }
}
