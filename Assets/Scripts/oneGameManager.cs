using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class oneGameManager : MonoBehaviour
{
    //1ゲーム中に獲得したスコア
    private int score;
    //ゲーム時間
    private int gameTime;
    //細かい時間経過をおこなうために用いる小数型の変数
    private float countTime;
    //GameManager
    private GameManager gm;
    //Audiosource
    private AudioSource audios = null;
    //６０～２０秒のＢＧＭ
    [SerializeField] private AudioClip beginBGM;
    //ラストスパートBGＭ
    [SerializeField] private AudioClip lastBGM;
    //数字のイメージ
    [SerializeField] private Sprite[] numberImage;
    //数字の配置位置
    [SerializeField] private Image[] imageNumber;

    //プロパティ
    public int Score
    {
        get { return this.score;}
        set { this.score = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        score = 0;
        gameTime = 60;
        countTime = 0.0f;

        audios = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CountTime();
        PlayBGM();
    }

    private void CountTime()
    {
        countTime += Time.deltaTime;
        if(countTime >= 1.0f)
        {
            --gameTime;
            countTime = 0.0f;
            //10秒
            imageNumber[0].sprite =numberImage[ gameTime / 10];
        }
    }

    private void PlayBGM()
    {
        if(gameTime >= 20 && gameTime <= 60)
        {
            PlayBGM(beginBGM);
        }
        if(gameTime < 20)
        {
            PlayBGM(lastBGM);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        if(audios != null)
        audios.PlayOneShot(clip);
    }
}
