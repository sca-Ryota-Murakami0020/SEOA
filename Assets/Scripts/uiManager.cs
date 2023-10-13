using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace GameManager
{
    public class uiManager : MonoBehaviour
    {
        //GameManager
        //private GameManager gm;
        //表示するUI
        //今回はボタンの操作をuiManagerに任せるためにSetActiveによる
        //表示の操作とする。
        [SerializeField] private GameObject pauseUI;
        //TimeManager
        [SerializeField] private TimeManager tm;

        void Start()
        {
            //gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            //UI表示の初期化
            pauseUI.SetActive(false);
        }

        //ポーズUIの表示
        public void OpenPauseUI()
        {
            Debug.Log("開く");
            pauseUI.SetActive(true);
            Time.timeScale = 0f;
            tm.NoActiveDoCount();
        }

        //ゲーム再開
        public void BackGame()
        {
            Debug.Log("閉じる");
            pauseUI.SetActive(false);
            Time.timeScale = 1.0f;
            tm.ActiveDoCount();
        }

        //タイトルへ
        public void GoTitle()
        {
            //gm.PlayerScore = 0;
            SceneManager.LoadScene("TITLE");
        }

        //リトライ
        public void LoadGame()
        {
            //gm.PlayerScore = 0;
            SceneManager.LoadScene("GameScene");
        }

    }

}

