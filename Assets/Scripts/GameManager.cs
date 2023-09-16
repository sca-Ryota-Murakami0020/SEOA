using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameManeger;

public class GameManager : MonoBehaviour
{
    //���̃����L���O�p�X�R�A�̔z��
    private int[] highScoreIndex;
    //�v���C���[���o�����X�R�A
    private int playerScore;
    ///�������K�[�h�𔃂��Ă���ꍇ
    private bool guardRam;
    //scoreManager
    private scoreManager sm;
    
    //�v���p�e�B
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

    //�V���O���g��
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //�v�f���̒�`
        highScoreIndex = new int[5];

        //�e�v�f�̏�����
        for (int count = 0; count < 5; count++)
        {
            highScoreIndex[count] = 0;
        }

        //�Q�[���̃X�R�A�̏�����
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
            //�����L���O�Ɠ����X�R�A���o�����ꍇ�A�����̏��ʂ̉��̃X�R�A���X�V����
            if(playerScore == highScoreIndex[count])
            {
                highScoreIndex[count + 1] = playerScore;
                break;
            }
        }
    }
}
