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

    //プロパティ
    public int PlayerScore
    {
        get { return this.playerScore;}
        set { this.playerScore = value;}
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

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateRanking()
    {
        for(int count = 4; count >= 0; count--)
        {
            if(playerScore >= cowHighScoreIndex[count])
            {
                int temp = cowHighScoreIndex[count + 1];
                cowHighScoreIndex[count + 1] = cowHighScoreIndex[count];
                cowHighScoreIndex[count] = playerScore;
            }
        }
    }

    public void BackGame()
    {

    }

    public void GoTitle()
    {
        playerScore = 0;
        SceneManager.LoadScene("TitleScene");
    }

    public void LoadGame()
    {
        playerScore = 0;
        SceneManager.LoadScene("GameManager");
    }
}
