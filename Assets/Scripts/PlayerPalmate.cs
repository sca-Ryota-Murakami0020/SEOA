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
    //AnimalManager
    [SerializeField]private AnimalManager am;
    //�G�t�F�N�g
    [Header("�Ăяo���G�t�F�N�g")] [SerializeField]
    private effectsC[] effect;
    //�J�E���g�_�E�����o�p�A�j���[�V����
    //[Header("�J�E���g�_�E���A�j���[�V����")][SerializeField]
    //private Animation countDwonAnim;
    //feberC
    //[Header("�t�B�[�o�[���o�֌W���Ǘ�����X�N���v�g")][SerializeField]
    //private feberC fc;
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
    //�J���[���擾�����ꍇ
    private bool getCurry;
    //���������擾�����ꍇ
    private bool getRum;
    //�m�ۊԊu��݂���
    private bool canAnimal;
    //�l�����������̖��O
    private string animalName;
    //�G�t�F�N�g���Ăяo������
    private int effectCount;
    //1�Q�[�����̊l���X�R�A
    private int _score;
    //�l���\��̃X�R�A
    private int getScore;
    //�Ȃ�����
    private int chainCount;
    //�Q�[������(�摜�Ő����̕\���������Ȃ����߁A�����^�̕����������₷������)
    private int gameTime;
    //�\������摜�̔Ԓn�����i�[����ϐ�
    private int spriteCount;
    //�m�ۊԊu����
    [SerializeField] private float waitTime;
    //�ׂ������Ԍo�߂������Ȃ����߂ɗp���鏬���^�̕ϐ�
    private float countTime;
    //�G�t�F�N�g�o���Ԋu����
    [SerializeField] private float waitEffectTime;
    //1�t���[���O�̃X���C�v�̈ʒu
    private Vector2 oldFlameTouchPos;
    //���݂̃X���C�v�ʒu
    private Vector2 nowTouchPos;
    //���݂Ȃ���n�_�ɂȂ��Ă��铮���̈ʒu
    private Vector2 oldTouchPos;
    //�߂܂��������̏ꏊ�i�����͏�ɓ����̂ŃI�u�W�F�N�g�Ŋi�[����������m�Ȉʒu�ɃG�t�F�N�g���Ăяo����j
    private Queue<GameObject> animalInfo;
    //�����̃C���[�W
    [Header("�\�����鐔����Image")] [SerializeField] 
    private Sprite[] numberImage;
    //�����̔z�u�ʒu
    [Header("�\������Image�̔z�u�ʒu")] [SerializeField] 
    private Image[] imageNumber;
    //�^�b�`�֐�
    private Touch touch;
    #endregion

    #region//��ԃN���X
    //�v���C���[�̏��
    private enum PlayerState
    {
        NULL,
        NORMAL,
        POWERUP,
        POWERDOWM
    };
    private PlayerState playerState;
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
    #endregion

    //�v���p�e�B
    public int Score {
        get { return this._score; }
        set { this._score = value; }
    }

    public int ChainCount
    {
        get { return this.effectCount;}
        set { this.effectCount = value;}
    }

    public bool DoStop
    {
        get { return this.doStop;}
        set { this.doStop = value;}
    }

    public Queue<GameObject> Animals
    {
        get { return this.animalInfo;}
        set { this.animalInfo = value;}
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
        effectCount = 0;
        chainCount = 0;
        doStop = true;
        doPoworUp = false;
        doPoworDwon = false;
        animalName = null;
        canAnimal = true;
        animalInfo = new Queue<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(doStop)
        {
            StartCoroutine(StartCountDown());
        }

        if(!doStop)
        {
            //���Ԍv��
            CountTime();
            //BGM�𗬂��֐�
            PlayBGM();
            //����
            if (Input.GetMouseButton(0))
            {
                TryCatchingAnimals();
            }
            //�}�E�X�𗣂���or�w�𗣂����ꍇ
            if (Input.GetMouseButtonUp(0))
            {
                //�X�R�A���i�[�����X�N���v�g�������ŎQ�Ƃ���
                //AnimalController animalController = GameObject.FindWithTag("animal").GetComponent<AnimalController>();
                //int score = animalController.Score;
                //animalName = hit2d.collider.name;
                //if(getCurry) DoPowerUp();
                //if(getRum) DoPowerDown();
                StartCoroutine(ActiveEffect());
            }
        }
        //Debug.Log($"connectCount:{effectCount}");
    }

    private void TryCatchingAnimals()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 100);
        //�����Ȃ��Ƃ�����^�b�vor�X���C�v���Ȃ�
        if (hit2d.collider == null)
        {
            return;
        }
        if (hit2d.collider.tag == "animal")
        {
            AnimalController animalController = 
            //�L�����閼�O�̍X�V
            string localName = animalController.Name;
            //animalName�Ɋi�[����Ă��閼�O�������ꍇ�܂���animalName�̖��O��localName���قȂ�ꍇ
            if (animalName == null)// || animalName != localName)
            {
                animalName = localName;
            }

            //���������ɓ������Ă��܂��Ă�����ȍ~�̏����͂��Ȃ�
            if(!animalController.CanGet) return;

            //�����Ȃ��悤�Ƃ��Ă��铮������ԍŏ��̓����Ɠ����ꍇ
            if (hit2d.collider.name == animalName && canAnimal)
            {
                PlayBGM(touchSE);

                getScore = animalController.Score;
                //�v�f�̖��[�ɒǉ�����
                animalInfo.Enqueue(hit2d.collider.gameObject);
                //am.GetAnimals.Enqueue(hit2d.collider.gameObject);
                //getScore = localScore;
                //�Ȃ��Ă��鐔���L��
                chainCount += 1;
                //Debug.Log(chainCount);
                StartCoroutine(DerayTime());
            }

        }
        //�J���[���擾�����ꍇ
        if (hit2d.collider.tag == "Curry")
        {
            PlayBGM(touchSE);
            getCurry = true;
        }
        //���������擾�����ꍇ
        if (hit2d.collider.tag == "Rum")
        {
            PlayBGM(touchSE);
            getRum = true;
        }
    }

    //���Ԍv��
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
            if(gameTime <= 0)
            {
                gm.UpdateRanking(_score);
            }
        }
        //if(doPoworUp)
        //{

        //}
    }

    //BGM�v���C
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

    //�X�R�A����
    void GetAnimal(int score, int count)
    {
        //�t�B�[�o�[���̃X�R�A�v�Z
        //if(doPoworUp) _score += (int)(score + score * 0.5);
        //�f�o�t�t�^���̃X�R�A�v�Z
        //if(doPoworDwon) _score += (int)(score - score * 0.3);

        //�ʏ�̃X�R�A���Z
        //�^�b�v�ŏI��点�Ă��鎞
        if(count == 1)
        {
            _score += score;
        }
        //�Ȃ��Ă�����
        if(count >= 2)
        {
            _score += score * count + (int)((score * count) * 0.1);
        }
        
        sm.UpdateScore(_score);
        //�`�F�C������������
        chainCount = 0;
    }

    //BGM����ʉ����Ăяo���֐�
    public void PlayBGM(AudioClip clip)
    {
        if (audios != null)
            audios.PlayOneShot(clip);
    }

    private void Effect(GameObject animal)
    {
        //�����̈ʒu�ɃG�t�F�N�g���Ăяo��
        effect[effectCount].PlayEffect(animal);
        //�Ȃ������𑝂₷
        effectCount++;
        //Debug.Log(animalInfo.Count);
        //�v�f�̍폜
        am.SponeAnimal(animalInfo.Dequeue());
        //Debug.Log("��������");
        //�����ŏo�͂�������z��ȏ�ɂȂ�����0�ɖ߂��I��������
        if (effect.Length == effectCount) effectCount = 0;
    }

    private void DoPowerUp()
    {
        doPoworUp = true;
        //fc.StartFeber();
    }

    private void DoPowerDown()
    {
        doPoworDwon = false;
    }

    private IEnumerator StartCountDown()
    {
        for(int count = 3; count >= 0; count--)
        {
            Debug.Log(count);
            yield return new WaitForSeconds(1);
        }
        doStop = false;
    }

    public void StartCountDownAnim()
    {

    }

    public void ReturnGame()
    {
        doStop = false;
    }

    private IEnumerator ActiveEffect()
    {
        //�X�R�A���Z
        Debug.Log("chainCount" + chainCount);
        GetAnimal(getScore, chainCount);
        
        //�G�t�F�N�g���Ăяo���I�u�W�F�N�g�̐���0�ɂȂ�܂ōs��
        while (animalInfo.Count != 0)
        {
            //�G�t�F�N�g�Đ�
            Effect(animalInfo.Peek());
            //�����ɉ����ČĂяo��������ύX����
            if (animalName == "Cow") PlayBGM(cowSE);
            if (animalName == "Mouse") PlayBGM(mouseSE);
            yield return new WaitForSeconds(waitEffectTime);
        }
        //�L�����閼�O�̏�����
        animalName = null;
        yield return null;
    }

    private IEnumerator DerayTime()
    {
        canAnimal = false;
        yield return new WaitForSeconds(waitTime);
        canAnimal = true;
        yield return null;
    }
}
