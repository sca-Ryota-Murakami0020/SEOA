using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class resultoC : MonoBehaviour
{
    //����̃X�R�A
    private int _score;
    //�����L���O�̃X�R�A
    private int[] _rankingScore;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ShowwithScore()
    {
        int showScore = 0;
        //�X�R�A���P�����Z���Ă���
        while(showScore != _score)
        {
            showScore++;
            yield return new WaitForEndOfFrame();
        }
    }

}
