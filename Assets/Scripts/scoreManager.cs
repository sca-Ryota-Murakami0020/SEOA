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
        Debug.Log("第二関門");
        int count = 10;
        for(int imagecount = 0;count < imageNumber.Length;imagecount++)
        {
            Debug.Log("処理中");
            //１の位の計算
            if(imagecount == 0)
            {
                imageNumber[imagecount].sprite = numberImage[score % count];
                Debug.Log("1 スコア更新完了");
            }
            //１０以上の位の計算
            else
            {
                imageNumber[imagecount].sprite = numberImage[score / count];
                Debug.Log(count + " スコア更新完了");
            }
            count *= 10;
        }
        Debug.Log(score);
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
