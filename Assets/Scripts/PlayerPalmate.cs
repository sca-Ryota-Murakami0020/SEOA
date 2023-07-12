using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class PlayerPalmate : MonoBehaviour
{
    #region//�Q�Ƃ���X�N���v�g
    //scoreManager
    private scoreManager sm;
    //GameManager
    private GameManager gm;
    //Audiosource
    private AudioSource audios = null;
    #endregion

    #region//�ϐ�
    //�X���C�v�����̔���
    private bool doSwaip;
    //�J���[���擾���A�o�t���󂯂Ă���l��
    private bool doPoworUp;
    //���������擾���A�f�o�t���󂯂Ă�����
    private bool doPoworDwon;
    //�|�[�Y��or�Q�[���J�n�R�J�E���g�O�̑���ł��Ȃ����
    private bool doStop;
    //�l�����������̖��O
    private string animalName;
    //1�Q�[�����̊l���X�R�A
    private int _score;
    //�Q�[������(�摜�Ő����̕\���������Ȃ����߁A�����^�̕����������₷������)
    private int gameTime;
    //�\������摜�̔Ԓn�����i�[����ϐ�
    private int spriteCount;
    //�ׂ������Ԍo�߂������Ȃ����߂ɗp���鏬���^�̕ϐ�
    private float countTime;
    //�X���C�v�ł���c��̋���
    private float swaipRange = 5.0f;
    //1�t���[���O�̃X���C�v�̈ʒu
    private Vector2 oldFlameTouchPos;
    //���݂̃X���C�v�ʒu
    private Vector2 nowTouchPos;
    //���݂Ȃ���n�_�ɂȂ��Ă��铮���̈ʒu
    private Vector2 oldTouchPos;
    #endregion

    #region//���ʉ��֌W
    //���̖���
    [SerializeField] private AudioClip cowSE;
    //�l�Y�~�̖���
    [SerializeField] private AudioClip mouseSE;
    //�^�b�`��
    [SerializeField] private AudioClip touchSE;
    //�X���C�v�����Ƃ��ɃQ�[�W��������ʉ�
    [SerializeField] private AudioClip gazeSE;
    //�U�O�`�Q�O�b�̂a�f�l
    [SerializeField] private AudioClip beginBGM;
    //���X�g�X�p�[�gBG�l
    [SerializeField] private AudioClip lastBGM;
    //�����̃C���[�W
    [Header("�\�����鐔����Image")][SerializeField] private Sprite[] numberImage;
    //�����̔z�u�ʒu
    [Header("�\������Image�̔z�u�ʒu")][SerializeField] private Image[] imageNumber;
    #endregion

    //�v���p�e�B
    public int Score {
        get { return this._score; }
        set { this._score = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        sm = GameObject.Find("Scorecounter").GetComponent<scoreManager>();
        audios = GetComponent<AudioSource>();

        //������
        _score = 0;
        gameTime = 60;
        countTime = 0.0f;
        doStop = true;
        doPoworUp = false;
        doPoworDwon = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(doStop == true)
        {
            //���Ԍv��
            CountTime();
            //BGM�𗬂��֐�
            PlayBGM();
            //�X���C�v����
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 100);
                //if(Physics2D.Raycast(ray,out hit2d,))
                //�����Ȃ��Ƃ�����^�b�v������
                if (hit2d.collider == null)
                {
                    return;
                }
                //�^�b�`�����I�u�W�F�N�g�������Ȃ�
                if (hit2d.collider && hit2d.collider.tag == "animal")
                {
                    oldTouchPos = hit2d.collider.transform.position;
                    doSwaip = false;
                }
            }

            if(Input.GetMouseButton(0) && doSwaip == true)
            {
                //1�t���[���O�̃}�E�X�̈ʒu��ێ�
                oldFlameTouchPos = nowTouchPos;
                //���݂̃X���C�v�ʒu�̍X�V
                nowTouchPos = Input.mousePosition;
                //�����ňړ������̌v�Z���s��
                NowVectorPosition(nowTouchPos);
                float maveCount = nowTouchPos - oldFlameTouchPos;
                swaipRange -= 

                //�n�_�̍X�V
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 100);
                //�����Ȃ��Ƃ�����^�b�v������
                if (hit2d.collider == null)
                {
                    return;
                }
                if (hit2d.collider && hit2d.collider.tag == "animal")
                {
                    //�����ŃX���C�v��ɓ����������Ƃ��ɁA�G�ꂽ���������̎n�_�ɕύX����
                    oldTouchPos = hit2d.collider.transform.position;
                }
            }

            //�^�b�`����
            if (Input.GetMouseButton(0))//Input.touchCount == 1 && touch.phase == TouchPhase.Began
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 100);
                //if(Physics2D.Raycast(ray,out hit2d,))
                //�����Ȃ��Ƃ�����^�b�v������
                if (hit2d.collider == null)
                {
                    return;
                }
                //�^�b�`�����I�u�W�F�N�g�������Ȃ�
                if (hit2d.collider && hit2d.collider.tag == "animal")
                {
                    if(Input.GetMouseButtonUp(0) && doSwaip == false)
                    {
                        AnimalController animalController = GameObject.FindWithTag("animal").GetComponent<AnimalController>();
                        int score = animalController.Score;
                        animalName = hit2d.collider.name;
                        GetAnimal(score, animalName);
                        Destroy(hit2d.collider.gameObject);
                    }
                }
            }
        }
    }

    private float NowVectorPosition(Vector2 vec)
    {
        
    }

    private void CountTime()
    {
        countTime += Time.deltaTime;
        if (countTime >= 1.0f)
        {
            --gameTime;
            countTime = 0.0f;
            //10�b�P�ʂ̌v�Z(�g�[�^���̃Q�[������/10(�����_�ȉ��͐؂�̂�))
            imageNumber[1].sprite = numberImage[gameTime / 10];
            //1�b�P�ʂ̌v�Z(�g�[�^���̃Q�[�����ԁ[10�b�P�ʂ̐��l��10)
            int secondTimer = gameTime - ((gameTime / 10) * 10);
            imageNumber[0].sprite = numberImage[secondTimer];
        }
    }

    private void PlayBGM()
    {
        if (gameTime >= 20 && gameTime <= 60)
        {
            PlayBGM(beginBGM);
        }
        if (gameTime < 20)
        {
            PlayBGM(lastBGM);
        }
    }

    void GetAnimal(int score,string name)
    {
        _score += score;
        PlayBGM(touchSE);
        //�����ɉ����ČĂяo��������ύX����
        if(name == "Cow") PlayBGM(cowSE);
        if(name == "Mouse") PlayBGM(mouseSE);
        sm.UpdateScore(_score);
    }

    //BGM����ʉ����Ăяo���֐�
    public void PlayBGM(AudioClip clip)
    {
        if (audios != null)
            audios.PlayOneShot(clip);
    }
}
