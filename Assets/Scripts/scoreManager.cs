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

    // Start is called before the first frame update
    void Start()
    {
        //�Q��
        gm = GetComponent<GameManager>();
        pp = GetComponent<PlayerPalmate>();

        //���l�̏�����
        for(int count = 0; count <= imageNumber.Length; count++)
        { 
            imageNumber[count].sprite = numberImage[0]  ;  
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore()
    {

    }
}
