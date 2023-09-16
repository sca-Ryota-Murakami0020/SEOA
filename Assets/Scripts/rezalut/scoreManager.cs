using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManeger
{
    public class scoreManager : MonoBehaviour
    {
        //PlayerPalmate
        [SerializeField] private PlayerPalmate pp;
        //数字の画像
        [SerializeField] private Sprite[] numberImage;
        //数字の配置位置
        [SerializeField] private Image[] imageNumber;
        //カンマの配置
        [SerializeField] private Image[] commaImage;

        //更新前のスコア
        private int oldScore = 0;
        //現在のスコア
        private int _score = 0;

        // Start is called before the first frame update
        void Start()
        {
            //参照
            //pp = GameObject.Find("Player").GetComponent<PlayerPalmate>();

            //数値の初期化
            for (int count = 0; count < imageNumber.Length; count++)
            {
                imageNumber[count].sprite = numberImage[0];
                //スコアを初期表示(0,000)にする
                if (count < 3) imageNumber[count].enabled = true;
                if (count >= 4) imageNumber[count].enabled = false;
            }

            //コンマ表示の初期化
            commaImage[1].enabled = false;
        }

        private void Update()
        {
            UpdateScore();
        }

        /*
        //スコア表示の更新
        public void UpdateScore(int score)
        {
            //各桁の計算をおこなうために使う変数
            int count = 10;
            //１桁目から計算をおこなう
            for (int imageCount = 0; imageCount < imageNumber.Length; imageCount++)
            {
                //１の位の計算
                if (imageCount <= 0)
                {
                    //仮に計算結果が9を超えているor0を下回っている場合0にする値
                    int showScore = score % count;
                    //if(showScore < 0　|| showScore > imageNumber.Length)showScore = 0;
                    imageNumber[imageCount].sprite = numberImage[showScore + 1];
                }

                //１０以上の位の計算
                if (imageCount >= 1)
                {
                    //仮に計算結果が9を超えているor0を下回っている場合0にする値
                    int showScore = (score / count) % 10;
                    //if (showScore < 0 || showScore > imageNumber.Length) showScore = 0;
                    imageNumber[imageCount].sprite = numberImage[showScore];
                    if (Mathf.Log10(pp.Score) >= 5)
                    {
                        imageNumber[imageCount].enabled = true;
                    }
                    count *= 10;
                }
            }
        }
        */

        //表示スコアの更新
        public void UpdateScore()
        {
            //１ずつ加算していくようにするための加算方法
            if(oldScore < _score)
            {
                oldScore++;
                int localOldScore = oldScore;
                for(int count = 0; count < imageNumber.Length; count++)
                {
                    int showNum = localOldScore % 10;
                    imageNumber[count].sprite = numberImage[showNum];
                    localOldScore %= 10;
                }
            }
        }

        //表示するスコアの桁数・コンマの表示を更新
        public void ShowScore(int score)
        {
            //獲得スコアに応じて表示するスコアの桁数を更新する
            int showScoreNumber = score / 10;
            if (showScoreNumber >= 4)
            {
                if (showScoreNumber >= 6 && commaImage[1].enabled == false)
                {
                    commaImage[1].enabled = true;
                }
                imageNumber[showScoreNumber].enabled = true;
            }
        }

        public void AddScore(int score, int count)
        {
            oldScore = _score;
            //通常のスコア加算
            //タップで終わらせている時
            if (count == 1)
            {
                _score += score;
            }
            //つなげている状態
            if (count >= 2)
            {
                _score += score * count + (int)((score * count) * 0.1);
            }
        }
    }

}
