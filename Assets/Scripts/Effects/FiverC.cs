using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�S�Ă̊֐��̓A�j���[�V�����ŌĂяo�����
namespace GameManager
{
    public class FiverC : MonoBehaviour
    {
        //�A�j���[�V����
        [SerializeField] private Animator animator;
        //CurryMoveC
        [SerializeField] private CurryMoverC curryMoveCon;
        //ActiveManager
        [SerializeField] private ActiveManager activeManager;

        //
        [SerializeField] private AnimalManager animalManager;
        //
        [SerializeField] private TimeManager tm;

        //���܂��������̃A�C�R����\������
        public void OpenSelectIcon()
        {
            //�Q�[�����Ԍv���𒆎~����
            tm.NoActiveDoCount();
            //�J���[�������ʒu�ɖ߂�
            curryMoveCon.ResetCurryPos();
            //�A�j���[�V�������J�n����
            animator.SetTrigger("StartFiver");
        }

        void Update()
        {
            
        }

        //�����Œ��I����
        public void DoingSelect()
        {
            int rNum = Random.Range(0,1);
            
            if(rNum == 0)
            {
                //activeManager.ActiveFiverCow();
                animalManager.FeverCow();
                //���̉��o���s��
                animator.SetTrigger("SelectCow");
            }
           
            else
            {
                //activeManager.ActiveFiverMouse();
                animalManager.FeverMouse();
                //�l�Y�~�̉��o���s��
                animator.SetTrigger("SelectMouse");
            }
            animalManager.SelectSponeFeverAnimal();
            animalManager.SetStartFeverAnimals();
            tm.ActiveFiverTime();
        }

        //�Ō�̃A�j���[�V�����Ɉړ�����
        private void FinishAnimation()
        {
            animator.SetTrigger("DoShow");
            //�Q�[�����Ԃ̌v�����ĊJ����
            tm.ActiveDoCount();
        }

        //���K���I������
        public void StartFiver()
        {
            //�����Ńt�B�[�o�[�p��Image���\���ɂ���
            activeManager.NoActiveFiverImage();
        }
    }
}


