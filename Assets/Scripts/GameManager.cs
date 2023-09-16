using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameManeger;

public class GameManager : MonoBehaviour
{
    //牛のランキング用スコアの配列
    private int[] highScoreIndex;
    //プレイヤーが出したスコア
    private int playerScore;
    ///ラム酒ガードを買っている場合
    private bool guardRam;
    //scoreManager
    private scoreManager sm;
    
    //プロパティ
    public int PlayerScore
    {
        get { return this.playerScore;}
        set { this.playerScore = value;}
    }
    public bool GuardRam
    {
        get { return this.guardRam;}
        set { this.guardRam = value;}
    }

    //シングルトン
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //要素数の定義
        highScoreIndex = new int[5];

        //各要素の初期化
        for (int count = 0; count < 5; count++)
        {
            highScoreIndex[count] = 0;
        }

        //ゲームのスコアの初期化
        playerScore = 0;
    }
    
    public void UpdateRanking(int score)
    {
        playerScore = score;
        for(int count = 4; count >= 0; count--)
        {
            if(playerScore > highScoreIndex[count])
            {
                int temp = highScoreIndex[count + 1];
                highScoreIndex[count + 1] = highScoreIndex[count];
                highScoreIndex[count] = playerScore;
            }
            //ランキングと同じスコアを出した場合、既存の順位の下のスコアを更新する
            if(playerScore == highScoreIndex[count])
            {
                highScoreIndex[count + 1] = playerScore;
                break;
            }
        }
    }
}
