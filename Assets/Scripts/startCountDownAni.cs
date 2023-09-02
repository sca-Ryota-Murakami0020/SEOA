using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startCountDownAni : MonoBehaviour
{
    [Header("カウント用Image")][SerializeField]
    private Image countImage;

    [SerializeField] private Sprite[] countSprite;

    [SerializeField] private PlayerPalmate pp;

    [SerializeField] private AnimalManager am;

    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartContDown()
    {

    }

    public void CheckContDown()
    {
        countImage.sprite = countSprite[count];
        count++;
    }

    public void StartSetAnimal()
    {
        am.SetAnimals();
    }

    public void EndCountDownAnimation()
    {
        pp.DontStart = false;

    }
}
