using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�S�Ă̊֐��̓A�j���[�V�����ŌĂяo�����
namespace GameManeger
{
    public class FiverC : MonoBehaviour
    {
        //�A�j���[�V����
        [SerializeField] private Animator ani;
        //TimeManager
        [SerializeField] private TimeManager timeManager;
        //CurryMoveC
        [SerializeField] private CurryMoverC curryMoveC;
        //ActiveManager
        [SerializeField] private ActiveManager activeManager;

        //���܂��������̃A�C�R����\������
        public void OpenSelectIcon()
        {
            //Debug.Log("�t�B�[�o�[�J�n");
            //�Q�[�����Ԍv���𒆎~����
            timeManager.NoActiveDoCount();
            //Debug.Log("�v���s���");
            //�J���[�������ʒu�ɖ߂�
            curryMoveC.ResetCurryPos();
            //�A�j���[�V�������J�n����
            ani.SetTrigger("StartFiver");
        }

        void Update()
        {
            
        }

        //�����Œ��I����
        public void DoingSelect()
        {
            //Debug.Log("�A�j���[�V����");
            int randomRange = Random.Range(1,30);
            //���̉��o���s��
            if(randomRange % 2 == 0)
            {
                activeManager.ActiveFiverCow();
                ani.SetTrigger("SelectCow");
            }

            //�l�Y�~�̉��o���s��
            if(randomRange % 2 != 0)
            {
                activeManager.ActiveFiverMouse();
                ani.SetTrigger("SelectMouse");
            }
        }

        //�Ō�̃A�j���[�V�����Ɉړ�����
        private void FinishAnimation()
        {
            ani.SetTrigger("DoShow");
            //�Q�[�����Ԃ̌v�����ĊJ����
            timeManager.ActiveDoCount();
            timeManager.ActiveFiverTime();
        }

        //���K���I������
        public void StartFiver()
        {
            //�����Ńt�B�[�o�[�p��Image���\���ɂ���
            activeManager.NoActiveFiverImage();
        }
    }
}


