using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chainConterC : MonoBehaviour
{
    //使用する画像
    [SerializeField] private Sprite[] numberImage;
    //使用するImage
    [SerializeField] private Image[] image;
    //PlayerPlamate
    [SerializeField] private PlayerPalmate pp;
    //つなげている数
    private int chainCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        image[0].sprite = numberImage[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
