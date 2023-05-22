using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreManager : MonoBehaviour
{
    //GamaManager
    private GameManager gm;
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
        gm = GetComponent<GameManager>();
        pp = GetComponent<PlayerPalmate>();

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore()
    {
        int count = imageNumber.Length;
        for(int imageCount = 10000000;imageCount >= 1;imageCount /= 10)
        {
            imageNumber[count].sprite = numberImage[gm.PlayerScore % imageCount];
            count--;
        }
    }

    public void ShowComma()
    {
        int commaCount = 0;
        for(int checkScore = 1000000;checkScore >= 1000;checkScore /= 1000)
        {
            if (gm.PlayerScore / checkScore >= 1)
            {
                commaImage[commaCount].enabled = true;
                commaCount++;
            }
        }
    }
}
