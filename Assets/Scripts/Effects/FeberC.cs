using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManeger
{
    public class FeberC : MonoBehaviour
    {
        //呼び出すImage
        [SerializeField] private Image fiverImage;
        //牛のアイコン
        [SerializeField] private Sprite cowImage;
        //ネズミのアイコン
        [SerializeField] private Sprite mouseImage;
        //アニメーション
        [SerializeField] private Animator ani;
        //TimeManager
        [SerializeField] private TimeManager tm;

        //決まった動物のアイコンを表示する
        public void OpenSelectIcon()
        {
            tm.NoActiveDoCount();
            ani.SetTrigger("StartFiver");
        }

        //ここで抽選する
        public void DoingSelect()
        {
            int randomRange = Random.Range(1,30);
            //牛の演出を行う
            if(randomRange % 2 == 0)
            {
                fiverImage.sprite = cowImage;
            }

            //ネズミの演出を行う
            if(randomRange % 2 != 0)
            {
                fiverImage.sprite = mouseImage;
            }

            ani.SetTrigger("Selected");
        }

        //演習を終了する
        public void StartFiver()
        {
            tm.ActiveDoCount();
        }
    }
}


