using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManager
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

        private float _currentDisplayScore = 0f;

        // Start is called before the first frame update
        void Start()
        {
            //数値の初期化
            for (int count = 0; count < imageNumber.Length; count++)
            {
                imageNumber[count].sprite = numberImage[0];
                //スコアを初期表示(0,000)にする
                foreach(Image imageNum in imageNumber)
                {
                    imageNum.enabled = false;
                }
                //if (count < 3) imageNumber[count].enabled = true;
                //if (count >= 4) imageNumber[count].enabled = false;
            }

            //コンマ表示の初期化
            commaImage[1].enabled = false;
        }

        private void Update()
        {
            UpdateScore();
            ShowScore((int)_currentDisplayScore);
        }

        private void FixedUpdate()
        {
            
            //Debug.Log("スコア：" + _score);
            //Debug.Log("OldScore：" + oldScore);
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
            
            if (_currentDisplayScore == _score) return;
            float localOldScore = _currentDisplayScore;
            if (_score - _currentDisplayScore < 0) _currentDisplayScore += (float)_score - _currentDisplayScore;
            else
            {
                //0.3秒でスコア加算の演出を終わらせるための変数  
                float _scoreMaxSec = 0.3333f;
                //１ずつ加算していくようにするための加算方法
                localOldScore += (_score - oldScore) / (_scoreMaxSec / Time.deltaTime);
                _currentDisplayScore = (int)localOldScore;
            }
            localOldScore = _currentDisplayScore;
           
            
            for (int count = 0; count < imageNumber.Length; count++)
            {
                int showNum = (int)localOldScore % 10;
                //Debug.Log(showNum);

                imageNumber[count].sprite = numberImage[(int)showNum];
                localOldScore /= 10;
            }
        }

        //表示するスコアの桁数・コンマの表示を更新
        public void ShowScore(int score)
        {
            int calcScore = score;
            int nums = 0;

            while(calcScore > 0)
            {
                imageNumber[nums].enabled = true;
                calcScore /= 10;
                if (nums >= 6)
                {
                    Debug.Log(score);
                    commaImage[1].enabled = true;
                }
                nums++;
            }
            if(score > 1000) return;
            for (int count = 0; count < 4; count++)
            {

                imageNumber[count].enabled = true;
            }
            commaImage[0].enabled = true;
        }

        //通常のスコア加算
        public void AddScore(int score, int count)
        {
            //比較するためのスコアを記録
            oldScore = _score;

            if (count == 1)
            {
                _score += score;
            }
            
            else
            {
                _score += (int)(score * count * 1.1);
            }
        }

        //フィーバー用のスコア加算
        //呼び出し元ー＞PlayerPlamate
        public void FiverAddScore(int getScore, int count)
        {
            //比較するためのスコアを記録
            oldScore = _score;
            
            if(count == 1)
            {
                _score += (int)(getScore * 1.2); 
            }
            
            else
            {
                _score += (int)((getScore * count) * 1.2);
            }
        }
    }

}
