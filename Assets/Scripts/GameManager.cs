using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //���̃����L���O�p�X�R�A�̔z��
    private int[] cowHighScoreIndex;
    //�l�p�̃����L���O�p�X�R�A�̔z��
    private int[] ratHighScoreIndex;
    //�v���C���[���o�����X�R�A
    private int playerScore;
    //�v���C����̂��l�Y�~���[�h���̔���
    private bool playMouse;
    //scoreManager
    private scoreManager sm;
    //PlayerPalmate
    private PlayerPalmate pp;
    
    //�v���p�e�B
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
    //�V���O���g��
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //�Q��
        pp = GameObject.Find("Player").GetComponent<PlayerPalmate>();

        //�v�f���̒�`
        cowHighScoreIndex = new int[5];
        ratHighScoreIndex = new int[5];

        //�e�v�f�̏�����
        for (int count = 0; count < 5; count++)
        {
            cowHighScoreIndex[count] = 0;
            ratHighScoreIndex[count] = 0;
        }

        //�Q�[���̃X�R�A�̏�����
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
            //�����L���O�Ɠ����X�R�A���o�����ꍇ�A�����̏��ʂ̉��̃X�R�A���X�V����
            if(playerScore == cowHighScoreIndex[count])
            {
                cowHighScoreIndex[count + 1] = playerScore;
                break;
            }
        }
    }
}
