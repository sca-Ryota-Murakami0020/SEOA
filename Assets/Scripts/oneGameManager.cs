using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class oneGameManager : MonoBehaviour
{
    //1�Q�[�����Ɋl�������X�R�A
    private int score;
    //�Q�[������
    private int gameTime;
    //�ׂ������Ԍo�߂������Ȃ����߂ɗp���鏬���^�̕ϐ�
    private float countTime;
    //GameManager
    private GameManager gm;
    //Audiosource
    private AudioSource audios = null;
    //�U�O�`�Q�O�b�̂a�f�l
    [SerializeField] private AudioClip beginBGM;
    //���X�g�X�p�[�gBG�l
    [SerializeField] private AudioClip lastBGM;
    //�����̃C���[�W
    [SerializeField] private Sprite[] numberImage;
    //�����̔z�u�ʒu
    [SerializeField] private Image[] imageNumber;

    //�v���p�e�B
    public int Score
    {
        get { return this.score;}
        set { this.score = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        //������
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
            //10�b
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
