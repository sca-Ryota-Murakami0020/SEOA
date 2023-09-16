using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManeger
{
    public class TimeManager : MonoBehaviour
    {
        //�v�������̔���
        private bool doCount = false;
        //�n�߂̃J�E���g�_�E�����I���������̔���
        private bool endStartCount = false;
        //���X�g�J�E���g�_�E�����J�n������
        private bool startEndCount = false;
        //�Q�[������(�摜�Ő����̕\���������Ȃ����߁A�����^�̕����������₷������)
        private int gameTime = 60;
        //�\������摜�̔Ԓn�����i�[����ϐ�
        private int spriteCount;
        //�ׂ������Ԍo�߂������Ȃ����߂ɗp���鏬���^�̕ϐ�
        private float countTime = 0.0f;
        //Audiosource
        private AudioSource audios = null;

        //�����̃C���[�W
        [Header("�\�����鐔����Image")]
        [SerializeField]
        private Sprite[] numberImage;
        //�����̔z�u�ʒu
        [Header("�\������Image�̔z�u�ʒu")]
        [SerializeField]
        private Image[] imageNumber;
        //�U�O�`�Q�O�b�̂a�f�l
        [SerializeField] private AudioClip beginBGM;
        //���X�g�X�p�[�gBG�l
        [SerializeField] private AudioClip lastBGM;

        //�A�j���[�V�����𗬂����߂̂���
        [SerializeField]private CountDownAnimationC cc;

        public bool DoCount
        {
            get { return this.doCount;}
            set { this.doCount = value;}
        }

        // Start is called before the first frame update
        void Start()
        {
            //�ŏ��̃J�E���g�_�E���̊J�n
            cc.ActiveStartCountAnimation();
        }

        // Update is called once per frame
        void Update()
        {
            if(doCount)
            {
                CountTimer();
                PlayBGM();
            }
        }

        //���Ԍv��
        private void CountTimer()
        {
            countTime += Time.deltaTime;
            if (countTime >= 1.0f)
            {
                --gameTime;
                countTime = 0.0f;
                //10�b�P�ʂ̌v�Z(�g�[�^���̃Q�[������/10(�����_�ȉ��͐؂�̂�))
                imageNumber[1].sprite = numberImage[gameTime / 10];
                //1�b�P�ʂ̌v�Z(�g�[�^���̃Q�[�����ԁ[10�b�P�ʂ̐��l��10)
                int secondTimer = gameTime - ((gameTime / 10) * 10);
                imageNumber[0].sprite = numberImage[secondTimer];
                //�I��5�b�O�̃A�j���[�V�������Ăяo��
                if (gameTime <= 5 && !startEndCount)
                {
                    cc.ActiveEndCountAnimation();
                }
            }
            //if(doPoworUp)
            //{

            //}
        }

        //BGM�v���C
        private void PlayBGM()
        {
            if (gameTime >= 20 && gameTime <= 60)
            {
                PlayBGM(beginBGM);
            }
            if (gameTime < 20)
            {
                PlayBGM(lastBGM);
            }
        }

        //BGM����ʉ����Ăяo���֐�
        public void PlayBGM(AudioClip clip)
        {
            if (audios != null)
                audios.PlayOneShot(clip);
        }
    }
}

