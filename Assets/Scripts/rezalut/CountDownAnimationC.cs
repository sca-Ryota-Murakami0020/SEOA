using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManeger
{
    public class CountDownAnimationC : MonoBehaviour
    {
        [SerializeField] private TimeManager tm;

        [SerializeField] private AnimalManager am;

        [SerializeField] private Animator ani;

        //動物を初期位置に設置する
        public void StartSetAnimal()
        {
            am.SetAnimals();
        }

        //ゲームをプレイ可能状態にする
        public void FinishStartCountDown()
        {
            tm.DoCount = true;
        }

        //ゲームを終了状態にする
        private void EndGame()
        {
            //Debug.Log("aaaaaaaaaa");
            tm.DoCount = false;
        }

        //カウントダウン用のImageを非表示にする
        public void CloseCount()
        {
            tm.CloseCountDown();
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

