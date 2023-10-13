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
        //�\������UI
        //����̓{�^���̑����uiManager�ɔC���邽�߂�SetActive�ɂ��
        //�\���̑���Ƃ���B
        [SerializeField] private GameObject pauseUI;
        //TimeManager
        [SerializeField] private TimeManager tm;

        void Start()
        {
            //gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            //UI�\���̏�����
            pauseUI.SetActive(false);
        }

        //�|�[�YUI�̕\��
        public void OpenPauseUI()
        {
            Debug.Log("�J��");
            pauseUI.SetActive(true);
            Time.timeScale = 0f;
            tm.NoActiveDoCount();
        }

        //�Q�[���ĊJ
        public void BackGame()
        {
            Debug.Log("����");
            pauseUI.SetActive(false);
            Time.timeScale = 1.0f;
            tm.ActiveDoCount();
        }

        //�^�C�g����
        public void GoTitle()
        {
            //gm.PlayerScore = 0;
            SceneManager.LoadScene("TITLE");
        }

        //���g���C
        public void LoadGame()
        {
            //gm.PlayerScore = 0;
            SceneManager.LoadScene("GameScene");
        }

    }

}

