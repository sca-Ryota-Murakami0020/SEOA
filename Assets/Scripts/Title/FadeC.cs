using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeC : MonoBehaviour
{
    [SerializeField] private TitleManager tm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAnimationIn()
    {
        tm.StartFadeIn();
    }

    public void StartAnimationOut()
    {
        tm.StartFadeOut();
    }
}
