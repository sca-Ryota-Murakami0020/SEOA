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
        }

        //�R���}�\���̏�����
        for(int count = 0;count < commaImage.Length;count++)
        {
            commaImage[count].enabled = false;
        }
    }

    //�X�R�A�\���̍X�V
    public void UpdateScore(int score)
    {
        int count = 10;
        for(int imageCount = 0;imageCount < imageNumber.Length;imageCount++)
        {
            //�P�̈ʂ̌v�Z
            if(imageCount == 0)
            {
                imageNumber[imageCount].sprite = numberImage[score % count];
            }
            //�P�O�ȏ�̈ʂ̌v�Z
            else
            {
                imageNumber[imageCount].sprite = numberImage[score / count];
                Debug.Log(imageCount);
            }
            count *= 10;
        }
    }

    //�R���}�̕\��
    public void ShowComma()
    {
        int commaCount = 0;
        for(int checkScore = 1000000;checkScore >= 1000;checkScore /= 1000)
        {
            if (pp.Score / checkScore >= 1)
            {
                commaImage[commaCount].enabled = true;
                commaCount++;
            }
        }
    }
}
