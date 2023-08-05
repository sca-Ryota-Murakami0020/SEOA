using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class resultoC : MonoBehaviour
{
    //今回のスコア
    private int _score;
    //ランキングのスコア
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
        //スコアを１ずつ加算していく
        while(showScore != _score)
        {
            showScore++;
            yield return new WaitForEndOfFrame();
        }
    }

}
