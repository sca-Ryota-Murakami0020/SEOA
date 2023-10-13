using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManager
{
    public class TimeManager : MonoBehaviour
    {
        //ActiveManager
        [SerializeField] private ActiveManager activeManager;

        //計測中かの判定
        private bool doCount = false;
        //ラストカウントダウンを開始したか
        private bool startEndCount = false;
        //フィーバーしているか
        private bool doingFiver = false;
        //ゲーム時間(画像で数字の表示をおこなうため、整数型の方が実装しやすいから)
        private int gameTime = 60;
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

        #region//カレー関係
        //CurryMoveC
        [SerializeField] private CurryMoverC cm;
        //]
        [SerializeField] private AnimalManager am;

        //カレーの待機時間
        private int cuuryWaitTime = 0;
        //カレーによる強化時間
        [SerializeField] private int powerUpTime;
        //実施に時間を計測する変数
        private int countBufTime;
        //最短時間間隔
        [SerializeField] private int minCurrySponeTime;
        //最長時間間隔
        [SerializeField] private int maxCurrySponeTime;
        #endregion

        #region//ラム酒関係
        //
        [SerializeField] private RumC rc;
        //デバフ影響時間
        [SerializeField] private int powerDownTime;
        //実施に時間を計測する変数
        private int countDebufTime = 0;
        //
        private int rumWaitTime = 0;
        //
        private bool doingDebuf = false;
        //最短時間間隔
        [SerializeField] private int minRumSponeTime;
        //最長時間間隔
        [SerializeField] private int maxRumSponeTime;
        #endregion

        public static TimeManager instance;

        public bool DoCount
        {
            get { return this.doCount;}
            set { this.doCount = value;}
        }

        public bool DoingFiver
        {
            get { return this.doingFiver;}
            set { this.doingFiver = value;}
        }

        // Start is called before the first frame update
        void Start()
        {
            //初期化
            countBufTime = powerUpTime;
            countDebufTime = powerDownTime;

            //最初のカウントダウンの開始
            activeManager.StartBeginCountDown();
            //カレーの出現時間を設定する
            ResetCurryTime();
            //
            ResetRumTime();
            //フィーバー用のImageを非表示にする
            activeManager.NoActiveFiverImage();
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

        #region//計測関係（フラグ込み）
        //時間計測
        private void CountTimer()
        {
            countTime += Time.deltaTime;
            if (countTime >= 1.0f)
            {
                //ゲーム時間の減少
                --gameTime;
                //カレーが初期位置にいたなら
                if (!cm.CanMove)
                {
                    CountCurryWaitTime();
                }
                if(!rc.CanGet)
                {
                    CountRumWaitTime();
                }
                //計測用の変数を更新
                countTime = 0.0f;               
                //フィーバー中なら
                if (doingFiver)
                {
                    Fiver();
                }
                //
                if(doingDebuf)
                {
                    Debuf();
                }

                //10秒単位の計算(トータルのゲーム時間/10(小数点以下は切り捨て))
                imageNumber[1].sprite = numberImage[gameTime / 10];
                //1秒単位の計算(トータルのゲーム時間ー10秒単位の数値＊10)
                int secondTimer = gameTime - ((gameTime / 10) * 10);
                imageNumber[0].sprite = numberImage[secondTimer];
                //終了5秒前のアニメーションを呼び出す
                if (gameTime <= 5 && !startEndCount)
                {
                    //終了アニメーションが流れたことにする
                    startEndCount = true;
                    //アニメーション用のImageを呼び出す
                    activeManager.ShowCountDown();
                    //対象のアニメーションを表示する
                    activeManager.StartEndCountDown();
                }
            }
        }

        //計測開始
        public void ActiveDoCount()
        {
            doCount = true;
        }

        //計測中止
        public void NoActiveDoCount()
        {
            doCount = false;
        }

        //カレーの待機時間を計測
        private void CountCurryWaitTime()
        {
            //カレーの待機時間を減少する
            --cuuryWaitTime;
            //カレーの待機時間が0になったら
            if (cuuryWaitTime <= 0)
            {
                //カレーの配置
                cm.SetCurry();
                //カレーの待機時間をリセット
                ResetCurryTime();
            }
        }

        private void CountRumWaitTime()
        {
            --countDebufTime;
            if(countDebufTime <= 0)
            {
                rc.SetRum();
                ResetRumTime();
            }
        }

        #endregion

        #region//音響
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
        private void PlayBGM(AudioClip clip)
        {
            if (audios != null)
                audios.PlayOneShot(clip);
        }
        #endregion

        #region//フィーバー関係
        //フィーバー時間の計測
        private void Fiver()
        {
            --countBufTime;
            //フィーバー時間が終了したら
            if (countBufTime <= 0)
            {
                //フィーバー中を解除
                doingFiver = false;
                //フィーバー状態の初期化
                activeManager.FinishFever();
                //初期化
                countBufTime = powerUpTime;
            }
        }

        //フィーバー中にする
        public void ActiveFiverTime()
        {
            //Debug.Log("doingFiverの切り替え");
            //フィーバー中にする
            doingFiver = true;
        }

        //カレーの時間間隔を設定する
        public void ResetCurryTime()
        {
            cuuryWaitTime = Random.Range(minCurrySponeTime,maxCurrySponeTime);
            //Debug.Log(cuuryWaitTime);
        }
        #endregion

        private void ResetRumTime()
        {
            rumWaitTime = Random.Range(minRumSponeTime,maxRumSponeTime);
        }

        public void ActiveDebuf()
        {
            doingDebuf = true;
        }

        public void Debuf()
        {
            --countDebufTime;
            //デバフ時間が終了したら
            if (countBufTime <= 0)
            {
                //デバフ中を解除
                doingFiver = false;
                //初期化
                countBufTime = powerUpTime;
            }
        }
    }
}