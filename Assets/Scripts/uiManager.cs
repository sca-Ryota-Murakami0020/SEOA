using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class uiManager : MonoBehaviour
{
    //
    private GameManager gm;
    //
    [SerializeField] private GameObject pauseUI;
    //
    private GameObject pauseUIInstance;
    //
    private bool doPause;

    //
    public bool DoPause
    {
        get { return this.doPause;}
        set { this.doPause = value;}
    }
    // Start is called before the first frame update
    void Start()
    {
        gm = GetComponent<GameManager>();
        //
        doPause = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPauseUI()
    {
        if(pauseUIInstance == null && doPause == false)
        {
            pauseUIInstance = GameObject.Instantiate(pauseUI) as GameObject;
            Time.timeScale = 0F;
            doPause = true;
        }
    }

    public void BackGame()
    {
        doPause = false;
        Destroy(pauseUI);
        Time.timeScale = 1.0f;
    }

    public void GoTitle()
    {
        gm.PlayerScore = 0;
        SceneManager.LoadScene("TitleScene");
    }

    public void LoadGame()
    {
        gm.PlayerScore = 0;
        SceneManager.LoadScene("GameManager");
    }
}
