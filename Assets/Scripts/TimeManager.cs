using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManeger
{
    public class TimeManager : MonoBehaviour
    {
        //計測中かの判定
        private bool doCount = false;
        //始めのカウントダウンを終了したかの判定
        private bool endStartCount = false;
        //ラストカウントダウンを開始したか
        private bool startEndCount = false;
        //ゲーム時間(画像で数字の表示をおこなうため、整数型の方が実装しやすいから)
        private int gameTime = 60;
        //表示する画像の番地数を格納する変数
        private int spriteCount;
        //細かい時間経過をおこなうために用いる小数型の変数
        private float countTime = 0.0f;
        //Audiosource
        private AudioSource audios = null;

        //数字のイメージ
        [Header("表示する数字のImage")]
        [SerializeField]
        private Sprite[] numberImage;
        //数字の配置位置
        [Header("表示するImageの配置位置")]
        [SerializeField]
        private Image[] imageNumber;
        //６０～２０秒のＢＧＭ
        [SerializeField] private AudioClip beginBGM;
        //ラストスパートBGＭ
        [SerializeField] private AudioClip lastBGM;

        //アニメーションを流すためのもの
        [SerializeField]private CountDownAnimationC cc;

        public bool DoCount
        {
            get { return this.doCount;}
            set { this.doCount = value;}
        }

        // Start is called before the first frame update
        void Start()
        {
            //最初のカウントダウンの開始
            cc.ActiveStartCountAnimation();
        }

        // Update is called once per frame
        void Update()
        {
            if(doCount)
            {
                CountTimer();
                PlayBGM();
            }
        }

        //時間計測
        private void CountTimer()
        {
            countTime += Time.deltaTime;
            if (countTime >= 1.0f)
            {
                --gameTime;
                countTime = 0.0f;
                //10秒単位の計算(トータルのゲーム時間/10(小数点以下は切り捨て))
                imageNumber[1].sprite = numberImage[gameTime / 10];
                //1秒単位の計算(トータルのゲーム時間ー10秒単位の数値＊10)
                int secondTimer = gameTime - ((gameTime / 10) * 10);
                imageNumber[0].sprite = numberImage[secondTimer];
                //終了5秒前のアニメーションを呼び出す
                if (gameTime <= 5 && !startEndCount)
                {
                    cc.ActiveEndCountAnimation();
                }
            }
            //if(doPoworUp)
            //{

            //}
        }

        //BGMプレイ
        private void PlayBGM()
        {
            if (gameTime >= 20 && gameTime <= 60)
            {
                PlayBGM(beginBGM);
            }
            if (gameTime < 20)
            {
                PlayBGM(lastBGM);
            }
        }

        //BGMや効果音を呼び出す関数
        public void PlayBGM(AudioClip clip)
        {
            if (audios != null)
                audios.PlayOneShot(clip);
        }
    }
}

