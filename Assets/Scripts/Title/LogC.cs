using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogC : MonoBehaviour
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

    public void StartingLogAni()
    {
        tm.StartLogAnimation();
    }

    public void LoopingAni()
    {
        tm.StartLoopAnimation();
    }
}
