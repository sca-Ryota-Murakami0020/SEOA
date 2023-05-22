using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class uiManager : MonoBehaviour
{
    //GameManager
    private GameManager gm;
    //�\������UI
    //����̓{�^���̑����uiManager�ɔC���邽�߂�SetActive�ɂ��
    //�\���̑���Ƃ���B
    [SerializeField] private GameObject pauseUI;
    //�|�[�Y���̃t���O
    private bool doPause;

    //�v���p�e�B
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
        //UI�\���̏�����
        pauseUI.SetActive(false);
    }

    //�|�[�YUI�̕\��
    public void OpenPauseUI()
    {
        if(doPause == false)
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0F;
            doPause = true;
        }
    }

    //�Q�[���ĊJ
    public void BackGame()
    {
        doPause = false;
        pauseUI.SetActive(false);
        Time.timeScale = 1.0f;
    }

    //�^�C�g����
    public void GoTitle()
    {
        gm.PlayerScore = 0;
        SceneManager.LoadScene("TITLE");
    }

    //���g���C
    public void LoadGame()
    {
        gm.PlayerScore = 0;
        SceneManager.LoadScene("GameScene");
    }
}
