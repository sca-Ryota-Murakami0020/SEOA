using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //牛のランキング用スコアの配列
    private int[] cowHighScoreIndex;
    //鼠用のランキング用スコアの配列
    private int[] ratHighScoreIndex;
    //プレイヤーが出したスコア
    private int playerScore;
    //プレイするのがネズミモードかの判定
    private bool playMouse;
    //scoreManager
    private scoreManager sm;
    //PlayerPalmate
    private PlayerPalmate pp;
    
    //プロパティ
    public int PlayerScore
    {
        get { return this.playerScore;}
        set { this.playerScore = value;}
    }
    public bool Mouse
    {
        get { return this.playMouse;}
        set { this.playMouse = value;}
    }
    //シングルトン
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //参照
        pp = GameObject.Find("Player").GetComponent<PlayerPalmate>();

        //要素数の定義
        cowHighScoreIndex = new int[5];
        ratHighScoreIndex = new int[5];

        //各要素の初期化
        for (int count = 0; count < 5; count++)
        {
            cowHighScoreIndex[count] = 0;
            ratHighScoreIndex[count] = 0;
        }

        //ゲームのスコアの初期化
        playerScore = 0;
    }
    
    public void UpdateRanking()
    {
        playerScore = pp.Score;
        for(int count = 4; count >= 0; count--)
        {
            if(playerScore > cowHighScoreIndex[count])
            {
                int temp = cowHighScoreIndex[count + 1];
                cowHighScoreIndex[count + 1] = cowHighScoreIndex[count];
                cowHighScoreIndex[count] = playerScore;
            }
            //ランキングと同じスコアを出した場合、既存の順位の下のスコアを更新する
            if(playerScore == cowHighScoreIndex[count])
            {
                cowHighScoreIndex[count + 1] = playerScore;
                break;
            }
        }
    }
}
