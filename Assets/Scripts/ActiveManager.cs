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
        [SerializeField] private FiverAnimalManager fam;

        //オブジェクト関係
        //フィーバー用のImage
        [SerializeField] private GameObject fiverObject;
        //タイム用のImage
        [SerializeField] private GameObject timeCountObject;
        //アニメーションを流すためのもの
        [SerializeField] private CountDownAnimationC countDowonC;
        //ネズミのオブジェクトをまとめたオブジェクト
        [SerializeField] private GameObject fiverMouseIndexObject;
        //牛のオブジェクトをまとめたオブジェクト
        [SerializeField] private GameObject fiverCowIndexObject;

        public void ActiveFiverMouse()
        {
            fiverMouseIndexObject.SetActive(true);
            fam.SetMouseIndex();
            fam.MouseFiver();
        }

        public void ActiveFiverCow()
        {
            fiverCowIndexObject.SetActive(true);
            fam.SetCowIndex();
            fam.CowFiver();
        }

        public void NoActiveFiverMouse() => fiverMouseIndexObject.SetActive(false);

        public void NoActiveFiverCow() => fiverCowIndexObject.SetActive(false);

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

