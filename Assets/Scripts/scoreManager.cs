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
    //数字の画像
    [SerializeField] private Sprite[] numberImage;
    //数字の配置位置
    [SerializeField] private Image[] imageNumber;

    // Start is called before the first frame update
    void Start()
    {
        //参照
        gm = GetComponent<GameManager>();
        pp = GetComponent<PlayerPalmate>();

        //数値の初期化
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
