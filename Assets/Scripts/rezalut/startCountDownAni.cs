using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startCountDownAni : MonoBehaviour
{
    [SerializeField] private PlayerPalmate pp;

    [SerializeField] private AnimalManager am;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
