using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManeger
{
    public class FeberC : MonoBehaviour
    {
        //�Ăяo��Image
        [SerializeField] private Image fiverImage;
        //���̃A�C�R��
        [SerializeField] private Sprite cowImage;
        //�l�Y�~�̃A�C�R��
        [SerializeField] private Sprite mouseImage;
        //�A�j���[�V����
        [SerializeField] private Animator ani;
        //TimeManager
        [SerializeField] private TimeManager tm;

        //���܂��������̃A�C�R����\������
        public void OpenSelectIcon()
        {
            tm.NoActiveDoCount();
            ani.SetTrigger("StartFiver");
        }

        //�����Œ��I����
        public void DoingSelect()
        {
            int randomRange = Random.Range(1,30);
            //���̉��o���s��
            if(randomRange % 2 == 0)
            {
                fiverImage.sprite = cowImage;
            }

            //�l�Y�~�̉��o���s��
            if(randomRange % 2 != 0)
            {
                fiverImage.sprite = mouseImage;
            }

            ani.SetTrigger("Selected");
        }

        //���K���I������
        public void StartFiver()
        {
            tm.ActiveDoCount();
        }
    }
}


