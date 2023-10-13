using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManager
{
    public class CountDownAnimationC : MonoBehaviour
    {
        //TimeManager
        [SerializeField] private TimeManager tm;
        //AnimalManager
        [SerializeField] private AnimalManager am;
        //ActiveManager
        [SerializeField] private ActiveManager activeManager;
        //アニメーションを流すImage
        [SerializeField] private Animator ani;

        //動物を初期位置に設置する
        public void StartSetAnimal()
        {
            am.SetAnimals();
        }

        //ゲームをプレイ可能状態にする
        public void FinishStartCountDown()
        {
            tm.ActiveDoCount();
        }

        //ゲームを終了状態にする
        private void EndGame()
        {
            tm.NoActiveDoCount();
        }

        //カウントダウン用のImageを非表示にする
        public void CloseCount()
        {
            activeManager.CloseCountDown();
        }

        //最初のカウントダウンを起動
        public void ActiveStartCountAnimation()
        {
            ani.SetTrigger("doStart");
        }

        //最後のカウントダウンを起動
        public void ActiveEndCountAnimation()
        {
            ani.SetTrigger("doEnd");
        }
    }
}

