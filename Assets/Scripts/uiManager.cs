using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class uiManager : MonoBehaviour
{
    //GameManager
    private GameManager gm;
    //表示するUI
    //今回はボタンの操作をuiManagerに任せるためにSetActiveによる
    //表示の操作とする。
    [SerializeField] private GameObject pauseUI;
    //ポーズ中のフラグ
    private bool doPause;

    //プロパティ
    public bool DoPause
    {
        get { return this.doPause;}
        set { this.doPause = value;}
    }

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //
        doPause = false;
        //UI表示の初期化
        pauseUI.SetActive(false);
    }

    //ポーズUIの表示
    public void OpenPauseUI()
    {
        if(doPause == false)
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0F;
            doPause = true;
        }
    }

    //ゲーム再開
    public void BackGame()
    {
        doPause = false;
        pauseUI.SetActive(false);
        Time.timeScale = 1.0f;
    }

    //タイトルへ
    public void GoTitle()
    {
        gm.PlayerScore = 0;
        SceneManager.LoadScene("TITLE");
    }

    //リトライ
    public void LoadGame()
    {
        gm.PlayerScore = 0;
        SceneManager.LoadScene("GameScene");
    }
}
