using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManager
{
    public class TimeManager : MonoBehaviour
    {
        //ActiveManager
        [SerializeField] private ActiveManager activeManager;

        //�v�������̔���
        private bool doCount = false;
        //���X�g�J�E���g�_�E�����J�n������
        private bool startEndCount = false;
        //�t�B�[�o�[���Ă��邩
        private bool doingFiver = false;
        //�Q�[������(�摜�Ő����̕\���������Ȃ����߁A�����^�̕����������₷������)
        private int gameTime = 60;
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

        #region//�J���[�֌W
        //CurryMoveC
        [SerializeField] private CurryMoverC cm;
        //]
        [SerializeField] private AnimalManager am;

        //�J���[�̑ҋ@����
        private int cuuryWaitTime = 0;
        //�J���[�ɂ�鋭������
        [SerializeField] private int powerUpTime;
        //���{�Ɏ��Ԃ��v������ϐ�
        private int countBufTime;
        //�ŒZ���ԊԊu
        [SerializeField] private int minCurrySponeTime;
        //�Œ����ԊԊu
        [SerializeField] private int maxCurrySponeTime;
        #endregion

        #region//�������֌W
        //
        [SerializeField] private RumC rc;
        //�f�o�t�e������
        [SerializeField] private int powerDownTime;
        //���{�Ɏ��Ԃ��v������ϐ�
        private int countDebufTime = 0;
        //
        private int rumWaitTime = 0;
        //
        private bool doingDebuf = false;
        //�ŒZ���ԊԊu
        [SerializeField] private int minRumSponeTime;
        //�Œ����ԊԊu
        [SerializeField] private int maxRumSponeTime;
        #endregion

        public static TimeManager instance;

        public bool DoCount
        {
            get { return this.doCount;}
            set { this.doCount = value;}
        }

        public bool DoingFiver
        {
            get { return this.doingFiver;}
            set { this.doingFiver = value;}
        }

        // Start is called before the first frame update
        void Start()
        {
            //������
            countBufTime = powerUpTime;
            countDebufTime = powerDownTime;

            //�ŏ��̃J�E���g�_�E���̊J�n
            activeManager.StartBeginCountDown();
            //�J���[�̏o�����Ԃ�ݒ肷��
            ResetCurryTime();
            //
            ResetRumTime();
            //�t�B�[�o�[�p��Image���\���ɂ���
            activeManager.NoActiveFiverImage();
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

        #region//�v���֌W�i�t���O���݁j
        //���Ԍv��
        private void CountTimer()
        {
            countTime += Time.deltaTime;
            if (countTime >= 1.0f)
            {
                //�Q�[�����Ԃ̌���
                --gameTime;
                //�J���[�������ʒu�ɂ����Ȃ�
                if (!cm.CanMove)
                {
                    CountCurryWaitTime();
                }
                if(!rc.CanGet)
                {
                    CountRumWaitTime();
                }
                //�v���p�̕ϐ����X�V
                countTime = 0.0f;               
                //�t�B�[�o�[���Ȃ�
                if (doingFiver)
                {
                    Fiver();
                }
                //
                if(doingDebuf)
                {
                    Debuf();
                }

                //10�b�P�ʂ̌v�Z(�g�[�^���̃Q�[������/10(�����_�ȉ��͐؂�̂�))
                imageNumber[1].sprite = numberImage[gameTime / 10];
                //1�b�P�ʂ̌v�Z(�g�[�^���̃Q�[�����ԁ[10�b�P�ʂ̐��l��10)
                int secondTimer = gameTime - ((gameTime / 10) * 10);
                imageNumber[0].sprite = numberImage[secondTimer];
                //�I��5�b�O�̃A�j���[�V�������Ăяo��
                if (gameTime <= 5 && !startEndCount)
                {
                    //�I���A�j���[�V���������ꂽ���Ƃɂ���
                    startEndCount = true;
                    //�A�j���[�V�����p��Image���Ăяo��
                    activeManager.ShowCountDown();
                    //�Ώۂ̃A�j���[�V������\������
                    activeManager.StartEndCountDown();
                }
            }
        }

        //�v���J�n
        public void ActiveDoCount()
        {
            doCount = true;
        }

        //�v�����~
        public void NoActiveDoCount()
        {
            doCount = false;
        }

        //�J���[�̑ҋ@���Ԃ��v��
        private void CountCurryWaitTime()
        {
            //�J���[�̑ҋ@���Ԃ���������
            --cuuryWaitTime;
            //�J���[�̑ҋ@���Ԃ�0�ɂȂ�����
            if (cuuryWaitTime <= 0)
            {
                //�J���[�̔z�u
                cm.SetCurry();
                //�J���[�̑ҋ@���Ԃ����Z�b�g
                ResetCurryTime();
            }
        }

        private void CountRumWaitTime()
        {
            --countDebufTime;
            if(countDebufTime <= 0)
            {
                rc.SetRum();
                ResetRumTime();
            }
        }

        #endregion

        #region//����
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
        private void PlayBGM(AudioClip clip)
        {
            if (audios != null)
                audios.PlayOneShot(clip);
        }
        #endregion

        #region//�t�B�[�o�[�֌W
        //�t�B�[�o�[���Ԃ̌v��
        private void Fiver()
        {
            --countBufTime;
            //�t�B�[�o�[���Ԃ��I��������
            if (countBufTime <= 0)
            {
                //�t�B�[�o�[��������
                doingFiver = false;
                //�t�B�[�o�[��Ԃ̏�����
                activeManager.FinishFever();
                //������
                countBufTime = powerUpTime;
            }
        }

        //�t�B�[�o�[���ɂ���
        public void ActiveFiverTime()
        {
            //Debug.Log("doingFiver�̐؂�ւ�");
            //�t�B�[�o�[���ɂ���
            doingFiver = true;
        }

        //�J���[�̎��ԊԊu��ݒ肷��
        public void ResetCurryTime()
        {
            cuuryWaitTime = Random.Range(minCurrySponeTime,maxCurrySponeTime);
            //Debug.Log(cuuryWaitTime);
        }
        #endregion

        private void ResetRumTime()
        {
            rumWaitTime = Random.Range(minRumSponeTime,maxRumSponeTime);
        }

        public void ActiveDebuf()
        {
            doingDebuf = true;
        }

        public void Debuf()
        {
            --countDebufTime;
            //�f�o�t���Ԃ��I��������
            if (countBufTime <= 0)
            {
                //�f�o�t��������
                doingFiver = false;
                //������
                countBufTime = powerUpTime;
            }
        }
    }
}