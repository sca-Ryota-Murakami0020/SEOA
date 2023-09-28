using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//全ての関数はアニメーションで呼び出される
namespace GameManeger
{
    public class FiverC : MonoBehaviour
    {
        //アニメーション
        [SerializeField] private Animator ani;
        //TimeManager
        [SerializeField] private TimeManager timeManager;
        //CurryMoveC
        [SerializeField] private CurryMoverC curryMoveC;
        //ActiveManager
        [SerializeField] private ActiveManager activeManager;

        //決まった動物のアイコンを表示する
        public void OpenSelectIcon()
        {
            //Debug.Log("フィーバー開始");
            //ゲーム時間計測を中止する
            timeManager.NoActiveDoCount();
            //Debug.Log("計測不可状態");
            //カレーを初期位置に戻す
            curryMoveC.ResetCurryPos();
            //アニメーションを開始する
            ani.SetTrigger("StartFiver");
        }

        void Update()
        {
            
        }

        //ここで抽選する
        public void DoingSelect()
        {
            //Debug.Log("アニメーション");
            int randomRange = Random.Range(1,30);
            //牛の演出を行う
            if(randomRange % 2 == 0)
            {
                activeManager.ActiveFiverCow();
                ani.SetTrigger("SelectCow");
            }

            //ネズミの演出を行う
            if(randomRange % 2 != 0)
            {
                activeManager.ActiveFiverMouse();
                ani.SetTrigger("SelectMouse");
            }
        }

        //最後のアニメーションに移動する
        private void FinishAnimation()
        {
            ani.SetTrigger("DoShow");
            //ゲーム時間の計測を再開する
            timeManager.ActiveDoCount();
            timeManager.ActiveFiverTime();
        }

        //演習を終了する
        public void StartFiver()
        {
            //ここでフィーバー用のImageを非表示にする
            activeManager.NoActiveFiverImage();
        }
    }
}


