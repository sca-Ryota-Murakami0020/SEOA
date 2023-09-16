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

        //�����������ʒu�ɐݒu����
        public void StartSetAnimal()
        {
            am.SetAnimals();
        }

        //�Q�[�����v���C�\��Ԃɂ���
        public void FinishStartCountDown()
        {
            tm.DoCount = true;
            
        }

        //�Q�[�����I����Ԃɂ���
        private void EndGame()
        {
            //Debug.Log("aaaaaaaaaa");
            tm.DoCount = false;
        }

        //�ŏ��̃J�E���g�_�E�����N��
        public void ActiveStartCountAnimation()
        {
            ani.SetTrigger("doStart");
        }

        //�Ō�̃J�E���g�_�E�����N��
        public void ActiveEndCountAnimation()
        {
            ani.SetTrigger("doEnd");
        }
    }
}

