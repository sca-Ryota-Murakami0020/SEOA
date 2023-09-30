using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManeger
{
    public class scoreManager : MonoBehaviour
    {
        //PlayerPalmate
        [SerializeField] private PlayerPalmate pp;
        //�����̉摜
        [SerializeField] private Sprite[] numberImage;
        //�����̔z�u�ʒu
        [SerializeField] private Image[] imageNumber;
        //�J���}�̔z�u
        [SerializeField] private Image[] commaImage;

        //�X�V�O�̃X�R�A
        private int oldScore = 0;
        //���݂̃X�R�A
        private int _score = 0;

        private float _currentDisplayScore = 0f;

        // Start is called before the first frame update
        void Start()
        {
            //���l�̏�����
            for (int count = 0; count < imageNumber.Length; count++)
            {
                imageNumber[count].sprite = numberImage[0];
                //�X�R�A�������\��(0,000)�ɂ���
                foreach(Image imageNum in imageNumber)
                {
                    imageNum.enabled = false;
                }
                //if (count < 3) imageNumber[count].enabled = true;
                //if (count >= 4) imageNumber[count].enabled = false;
            }

            //�R���}�\���̏�����
            commaImage[1].enabled = false;
        }

        private void Update()
        {
            UpdateScore();
            ShowScore((int)_currentDisplayScore);
        }

        private void FixedUpdate()
        {
            
            //Debug.Log("�X�R�A�F" + _score);
            //Debug.Log("OldScore�F" + oldScore);
        }

        /*
        //�X�R�A�\���̍X�V
        public void UpdateScore(int score)
        {
            //�e���̌v�Z�������Ȃ����߂Ɏg���ϐ�
            int count = 10;
            //�P���ڂ���v�Z�������Ȃ�
            for (int imageCount = 0; imageCount < imageNumber.Length; imageCount++)
            {
                //�P�̈ʂ̌v�Z
                if (imageCount <= 0)
                {
                    //���Ɍv�Z���ʂ�9�𒴂��Ă���or0��������Ă���ꍇ0�ɂ���l
                    int showScore = score % count;
                    //if(showScore < 0�@|| showScore > imageNumber.Length)showScore = 0;
                    imageNumber[imageCount].sprite = numberImage[showScore + 1];
                }

                //�P�O�ȏ�̈ʂ̌v�Z
                if (imageCount >= 1)
                {
                    //���Ɍv�Z���ʂ�9�𒴂��Ă���or0��������Ă���ꍇ0�ɂ���l
                    int showScore = (score / count) % 10;
                    //if (showScore < 0 || showScore > imageNumber.Length) showScore = 0;
                    imageNumber[imageCount].sprite = numberImage[showScore];
                    if (Mathf.Log10(pp.Score) >= 5)
                    {
                        imageNumber[imageCount].enabled = true;
                    }
                    count *= 10;
                }
            }
        }
        */

        //�\���X�R�A�̍X�V
        public void UpdateScore()
        {
            
            if (_currentDisplayScore == _score) return;
            float localOldScore = _currentDisplayScore;
            if (_score - _currentDisplayScore < 0) _currentDisplayScore += (float)_score - _currentDisplayScore;
            else
            {
                float _scoreMaxSec = 0.3333f;
                //�P�����Z���Ă����悤�ɂ��邽�߂̉��Z���@
                localOldScore += (_score - oldScore) / (_scoreMaxSec / Time.deltaTime);
                _currentDisplayScore = (int)localOldScore;
            }
            localOldScore = _currentDisplayScore;
           
            
            for (int count = 0; count < imageNumber.Length; count++)
            {
                int showNum = (int)localOldScore % 10;
                //Debug.Log(showNum);

                imageNumber[count].sprite = numberImage[(int)showNum];
                localOldScore /= 10;
            }
           // Debug.Log(imageNumber[0]);
        }

        //�\������X�R�A�̌����E�R���}�̕\�����X�V
        public void ShowScore(int score)
        {
            int calcScore = score;
            int nums = 0;

            while(calcScore > 0)
            {
                imageNumber[nums].enabled = true;
                calcScore /= 10;
                if (nums >= 6)
                {
                    Debug.Log(score);
                    commaImage[1].enabled = true;
                }
                nums++;
            }
            if(score > 1000) return;
            for (int count = 0; count < 4; count++)
            {
                //imageNumber[count].sprite = numberImage[0];
                imageNumber[count].enabled = true;
            }
            commaImage[0].enabled = true;

            //�l���X�R�A�ɉ����ĕ\������X�R�A�̌������X�V����
            //int showScoreNumber = score / 10;
            //if (showScoreNumber >= 4)
            //{
            //    if (showScoreNumber >= 5 && commaImage[1].enabled == false)
            //    {
            //        commaImage[1].enabled = true;
            //    }
            //    imageNumber[showScoreNumber].enabled = true;
            //}
        }

        //�ʏ�̃X�R�A���Z
        //�Ăяo�����[��PlayerPlamate
        public void AddScore(int score, int count)
        {
            oldScore = _score;
            //�ʏ�̃X�R�A���Z
            //�^�b�v�ŏI��点�Ă��鎞
            if (count == 1)
            {
                _score += score;
            }
            //�Ȃ��Ă�����
            if (count >= 2)
            {
                _score += score * count + (int)((score * count) * 0.1);
            }
        }

        //�t�B�[�o�[�p�̃X�R�A���Z
        //�Ăяo�����[��PlayerPlamate
        public void FiverAddScore(int getScore, int count)
        {
            //��r���邽�߂̃X�R�A���L�^
            oldScore = _score;
            //�X�R�A���Z
            if(count == 1)
            {
                _score += getScore + (int)(getScore * 0.2); 
            }
            //�Ȃ��Ă�����
            if(count >= 2)
            {
                _score += getScore * count + (int)((getScore * count) * 0.2);
            }
        }
    }

}
