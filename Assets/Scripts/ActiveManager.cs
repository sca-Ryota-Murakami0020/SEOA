using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManeger
{
    public class ActiveManager : MonoBehaviour
    {
        //関数関係
        //FiverC
        [SerializeField] private FiverC fc;
        //
        [SerializeField] private AnimalManager am;
        //
        [SerializeField] private TimeManager tm;
        
        //オブジェクト関係
        //フィーバー用のImage
        [SerializeField] private GameObject fiverObject;
        //タイム用のImage
        [SerializeField] private GameObject timeCountObject;
        //アニメーションを流すためのもの
        [SerializeField] private CountDownAnimationC countDowonC;

        #region//フィーバー関係
        //フィーバー用のImageを表示する
        public void ActiveFiverImage()
        {
            fiverObject.SetActive(true);
        }

        //フィーバー用のImageを非表示にする
        public void NoActiveFiverImage()
        {
            //Debug.Log("TM側での消去完了");
            fiverObject.SetActive(false);
        }

        //フィーバーアニメーションを再生する
        public void ActiveFiverTime()
        {
            //フィーバーオブジェクトの表示する
            ActiveFiverImage();
            //フィーバーアニメーションを開始する
            fc.OpenSelectIcon();
        }

        //牛のフィーバーを起動する
        public void ActiveFiverMouse()
        {
            tm.ActiveFiverTime();
            am.FeverMouse();
            am.SelectSponeFeverAnimal();
            am.SetStartFeverAnimals();
        }

        //ネズミのフィーバーを起動する
        public void ActiveFiverCow()
        {
            tm.ActiveFiverTime();
            am.FeverCow();
            am.SelectSponeFeverAnimal();
            am.SetStartFeverAnimals();
        }

        //フィーバーを終了する
        public void FinishFever()
        {
            am.FinishFeverCow();
            am.FinishFeverMouse();
        }
        #endregion

        #region//カウントダウン関係
        //カウントダウン用のImageを表示
        public void ShowCountDown()
        {
            timeCountObject.SetActive(true);
        }

        //カウントダウン用のImageを非表示に
        public void CloseCountDown()
        {
            timeCountObject.SetActive(false);
        }

        //最初のカウントダウン用のアニメーションを再生
        public void StartBeginCountDown()
        {
            countDowonC.ActiveStartCountAnimation();
        }

        //ラスト5秒前カウントダウン用のアニメーションを再生
        public void StartEndCountDown()
        {
            countDowonC.ActiveEndCountAnimation();
        }

        #endregion
    }
}

