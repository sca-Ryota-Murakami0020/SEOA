using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreManager : MonoBehaviour
{
    //PlayerPalmate
    private PlayerPalmate pp;
    //�����̉摜
    [SerializeField] private Sprite[] numberImage;
    //�����̔z�u�ʒu
    [SerializeField] private Image[] imageNumber;
    //�J���}�̔z�u
    [SerializeField] private Image[] commaImage;

    // Start is called before the first frame update
    void Start()
    {
        //�Q��
        pp = GameObject.Find("Player").GetComponent<PlayerPalmate>();

        //���l�̏�����
        for(int count = 0; count < imageNumber.Length; count++)
        { 
            imageNumber[count].sprite = numberImage[0];
            //�X�R�A�������\��(0,000)�ɂ���
            if(count < 3)imageNumber[count].enabled = true;
            if(count >= 4) imageNumber[count].enabled = false;
        }

        //�R���}�\���̏�����
        commaImage[1].enabled = false;
    }

    //�X�R�A�\���̍X�V
    public void UpdateScore(int score)
    {
        Debug.Log(score);
        //�e���̌v�Z�������Ȃ����߂Ɏg���ϐ�
        int count = 10;
        //�P���ڂ���v�Z�������Ȃ�
        for(int imageCount = 0;imageCount < imageNumber.Length;imageCount++)
        {
            //�P�̈ʂ̌v�Z
            if(imageCount <= 0)
            {
                //���Ɍv�Z���ʂ�9�𒴂��Ă���or0��������Ă���ꍇ0�ɂ���l
                int showScore = score / count;
                if(showScore < 0�@|| showScore > imageNumber.Length)showScore =0;
                imageNumber[imageCount].sprite = numberImage[showScore];
                Debug.Log("�P�̌��F" + showScore);
            }
            //�P�O�ȏ�̈ʂ̌v�Z
            if(imageCount >= 1)
            {
                //���Ɍv�Z���ʂ�9�𒴂��Ă���or0��������Ă���ꍇ0�ɂ���l
                int showScore = score / count;
                if (showScore < 0 || showScore > imageNumber.Length) showScore = 0;
                imageNumber[imageCount].sprite = numberImage[showScore];
                Debug.Log(count + "�̌��F" + showScore);
                count *= 10;
            }
            //�����Ōv�Z���錅����������̂�count������������
            
        }
    }

    //�\������X�R�A�̌����E�R���}�̕\�����X�V
    public void ShowScore(int score)
    {
        //�l���X�R�A�ɉ����ĕ\������X�R�A�̌������X�V����
        int showScoreNumber = score / 10;
        if(showScoreNumber >= 4)
        {
            if(showScoreNumber >= 6 && commaImage[1].enabled == false)
            {
                commaImage[1].enabled = true;
            }
            imageNumber[showScoreNumber].enabled = true;
        }
    }
}
