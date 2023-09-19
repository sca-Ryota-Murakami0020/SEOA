using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManeger
{
    public class CountDownAnimationC : MonoBehaviour
    {
        //TimeManager
        [SerializeField] private TimeManager tm;
        //AnimalManager
        [SerializeField] private AnimalManager am;
        //�A�j���[�V�����𗬂�Image
        [SerializeField] private Animator ani;

        //�����������ʒu�ɐݒu����
        public void StartSetAnimal()
        {
            am.SetAnimals();
        }

        //�Q�[�����v���C�\��Ԃɂ���
        public void FinishStartCountDown()
        {
            tm.ActiveDoCount();
        }

        //�Q�[�����I����Ԃɂ���
        private void EndGame()
        {
            tm.NoActiveDoCount();
        }

        //�J�E���g�_�E���p��Image���\���ɂ���
        public void CloseCount()
        {
            tm.CloseCountDown();
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

