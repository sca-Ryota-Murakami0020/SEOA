using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManeger
{
    public class ActiveManager : MonoBehaviour
    {
        //�֐��֌W
        //FiverC
        [SerializeField] private FiverC fc;
        //
        [SerializeField] private FiverAnimalManager fam;

        //�I�u�W�F�N�g�֌W
        //�t�B�[�o�[�p��Image
        [SerializeField] private GameObject fiverObject;
        //�^�C���p��Image
        [SerializeField] private GameObject timeCountObject;
        //�A�j���[�V�����𗬂����߂̂���
        [SerializeField] private CountDownAnimationC countDowonC;

        #region//�t�B�[�o�[�֌W
        //�t�B�[�o�[�p��Image��\������
        public void ActiveFiverImage()
        {
            fiverObject.SetActive(true);
        }

        //�t�B�[�o�[�p��Image���\���ɂ���
        public void NoActiveFiverImage()
        {
            //Debug.Log("TM���ł̏�������");
            fiverObject.SetActive(false);
        }

        //�t�B�[�o�[�A�j���[�V�������Đ�����
        public void ActiveFiverTime()
        {
            //�t�B�[�o�[�I�u�W�F�N�g�̕\������
            ActiveFiverImage();
            //�t�B�[�o�[�A�j���[�V�������J�n����
            fc.OpenSelectIcon();
        }

        //���̃t�B�[�o�[���N������
        public void ActiveFiverMouse()
        {
            fam.SetMouseIndex();
            fam.MouseFiver();
        }

        //�l�Y�~�̃t�B�[�o�[���N������
        public void ActiveFiverCow()
        {
            fam.SetCowIndex();
            fam.CowFiver();
        }

        //�����������Ăяo��
        public void ActiveFiverAnimals(GameObject animal)
        {
            animal.SetActive(true);
        }

        //�����������\���ɂ���
        public void NoActiveFiverAnimals(GameObject animal)
        {

        }
        #endregion

        #region//�J�E���g�_�E���֌W
        //�J�E���g�_�E���p��Image��\��
        public void ShowCountDown()
        {
            timeCountObject.SetActive(true);
        }

        //�J�E���g�_�E���p��Image���\����
        public void CloseCountDown()
        {
            timeCountObject.SetActive(false);
        }

        //�ŏ��̃J�E���g�_�E���p�̃A�j���[�V�������Đ�
        public void StartBeginCountDown()
        {
            countDowonC.ActiveStartCountAnimation();
        }

        //���X�g5�b�O�J�E���g�_�E���p�̃A�j���[�V�������Đ�
        public void StartEndCountDown()
        {
            countDowonC.ActiveEndCountAnimation();
        }

        #endregion
    }
}
