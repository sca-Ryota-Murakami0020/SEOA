using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//全ての関数はアニメーションで呼び出される
namespace GameManager
{
    public class FiverC : MonoBehaviour
    {
        //アニメーション
        [SerializeField] private Animator animator;
        //CurryMoveC
        [SerializeField] private CurryMoverC curryMoveCon;
        //ActiveManager
        [SerializeField] private ActiveManager activeManager;

        //
        [SerializeField] private AnimalManager animalManager;
        //
        [SerializeField] private TimeManager tm;

        //決まった動物のアイコンを表示する
        public void OpenSelectIcon()
        {
            //ゲーム時間計測を中止する
            tm.NoActiveDoCount();
            //カレーを初期位置に戻す
            curryMoveCon.ResetCurryPos();
            //アニメーションを開始する
            animator.SetTrigger("StartFiver");
        }

        void Update()
        {
            
        }

        //ここで抽選する
        public void DoingSelect()
        {
            int rNum = Random.Range(0,1);
            
            if(rNum == 0)
            {
                //activeManager.ActiveFiverCow();
                animalManager.FeverCow();
                //牛の演出を行う
                animator.SetTrigger("SelectCow");
            }
           
            else
            {
                //activeManager.ActiveFiverMouse();
                animalManager.FeverMouse();
                //ネズミの演出を行う
                animator.SetTrigger("SelectMouse");
            }
            animalManager.SelectSponeFeverAnimal();
            animalManager.SetStartFeverAnimals();
            tm.ActiveFiverTime();
        }

        //最後のアニメーションに移動する
        private void FinishAnimation()
        {
            animator.SetTrigger("DoShow");
            //ゲーム時間の計測を再開する
            tm.ActiveDoCount();
        }

        //演習を終了する
        public void StartFiver()
        {
            //ここでフィーバー用のImageを非表示にする
            activeManager.NoActiveFiverImage();
        }
    }
}


