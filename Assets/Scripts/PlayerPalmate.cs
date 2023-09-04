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
    //�Q�[���J�n�R�J�E���g�O�̑���ł��Ȃ����
    private bool dontStart;
    //���j���[���J���Ă�����
    private bool openMenu;
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
    //�Ȃ�����
    private int chainCount;
    //�Q�[������(�摜�Ő����̕\���������Ȃ����߁A�����^�̕����������₷������)
    private int gameTime;
    //�\������摜�̔Ԓn�����i�[����ϐ�
    private int spriteCount;
    //�ׂ������Ԍo�߂������Ȃ����߂ɗp���鏬���^�̕ϐ�
    private float countTime;
    /*
    //1�t���[���O�̃X���C�v�̈ʒu
    private Vector2 oldFlameTouchPos;
    //���݂̃X���C�v�ʒu
    private Vector2 nowTouchPos;
    //���݂Ȃ���n�_�ɂȂ��Ă��铮���̈ʒu
    private Vector2 oldTouchPos;
    */
    //�߂܂��������̏ꏊ�i�����͏�ɓ����̂ŃI�u�W�F�N�g�Ŋi�[����������m�Ȉʒu�ɃG�t�F�N�g���Ăяo����j
    private Queue<GameObject> animalInfo;
    
    //�^�b�`�֐�
    private Touch touch;
    #endregion

    #region//�C���X�y�N�^�[�ŕύX����l
    //�m�ۊԊu����
    [SerializeField] private float waitTime;
    //���̃X�R�A
    [SerializeField] private int cowScore;
    //�l�Y�~�̃X�R�A
    [SerializeField] private int mouseScore;
    //�G�t�F�N�g�o���Ԋu����
    [SerializeField] private float waitEffectTime;
    //�p���[�A�b�v����
    [SerializeField] private float powerupTime;
    //�p���[�_�E������
    [SerializeField] private float powerdownTime;
    //�����̃C���[�W
    [Header("�\�����鐔����Image")]
    [SerializeField]
    private Sprite[] numberImage;
    //�����̔z�u�ʒu
    [Header("�\������Image�̔z�u�ʒu")]
    [SerializeField]
    private Image[] imageNumber;
    #endregion

    #region//�N���X
    //�v���C���[�̏��
    public enum PlayerState
    {
        NULL,
        POWERUP,
        POWERDOWN
    };

    private enum GetAnimals
    {
        NULL,
        Cow,
        Mouse
    };

    //�ϐ�
    private PlayerState playerState;
    private GetAnimals getAnimalName;
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

    #region//�v���p�e�B
    public int Score {
        get { return this._score; }
        set { this._score = value; }
    }

    public int ChainCount
    {
        get { return this.effectCount;}
        set { this.effectCount = value;}
    }

    public bool DontStart
    {
        get { return this.dontStart;}
        set { this.dontStart = value;}
    }

    public bool OpenMenu
    {
        get { return this.openMenu;}
        set { this.openMenu = value;}
    }

    public bool DoChain
    {
        get { return this.doSwaip;}
    }

    public PlayerState PlayerSituation
    {
        get { return this.playerState;}
    }

    public Queue<GameObject> Animals
    {
        get { return this.animalInfo;}
        set { this.animalInfo = value;}
    }
    #endregion

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

        dontStart = true;
        doPoworUp = false;
        doPoworDwon = false;
        canAnimal = true;
        openMenu = false;

        animalName = null;
        playerState = PlayerState.NULL;
        getAnimalName = GetAnimals.NULL;
        animalInfo = new Queue<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if((!dontStart) && (!openMenu))
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
                //if(getCurry) DoPowerUp();
                //if(getRum) DoPowerDown();
                StartCoroutine(ActiveEffect());
            }
        }
    }

    //�X���C�v����
    private void TryCatchingAnimals()
    {       
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 100 );
        //�����Ȃ��Ƃ�����^�b�vor�X���C�v���Ȃ�
        if (hit2d.collider == null)
        {
            return;
        }
        
        if (hit2d.collider.tag == "animal")
        {
            //Debug.Log("�q���Ă�");
            doSwaip = true;
            //���C���[�}�X�N���l������
            string getAnimalName = LayerMask.LayerToName(hit2d.collider.gameObject.layer);
            //�����ێ����Ă��閼�O������������
            if(this.getAnimalName == GetAnimals.NULL)
            {
                //�ŏ��̃f�[�^�����ɕύX
                if (getAnimalName == LayerMask.LayerToName(6))
                {
                    this.getAnimalName = GetAnimals.Cow;
                }
                //�ŏ��̃f�[�^���l�Y�~�ɕύX
                if(getAnimalName == LayerMask.LayerToName(7))
                {
                    this.getAnimalName = GetAnimals.Mouse;
                }
            }

            if (this.getAnimalName != GetAnimals.NULL && this.getAnimalName.ToString() == getAnimalName)
            {
                //�߂܂��������̊֐����擾����
                AnimalController animalController = hit2d.collider.gameObject.GetComponent<AnimalController>();

                //���������ɓ������Ă��܂��Ă�����ȍ~�̏����͂��Ȃ�
                if (!animalController.CanGet) return;

                //�����Ȃ��悤�Ƃ��Ă��铮������ԍŏ��̓����Ɠ����ꍇ
                if (animalController.CanGet)
                {
                    PlayBGM(touchSE);
                    //Debug.Log("�F�̕ύX�J�n");
                    animalController.ChangeColor();
                    animalController.CanselGet();
                    //�߂܂��������̕߂܂����t���O��false�ɂ���
                    //animalController.CanGet = false;
                    //Debug.Log("canGet:" + animalController.CanGet);
                    //�v�f�̖��[�ɒǉ�����
                    animalInfo.Enqueue(hit2d.collider.gameObject);                    
                    //�Ȃ��Ă��鐔���X�V
                    chainCount += 1;
                }
            }
        }

        //�G�ꂽ�I�u�W�F�N�g���A�C�e���Ȃ�
        if(hit2d.collider.tag == "Item")
        {
            if(!doSwaip) doSwaip = true;
            //���C���[�}�X�N���l������
            string getItemName = LayerMask.LayerToName(hit2d.collider.gameObject.layer);
            //���������擾�����ꍇ
            if (getItemName == LayerMask.LayerToName(8))
            {
                PlayBGM(touchSE);
                getRum = true;
            }
            //�J���[���擾�����ꍇ
            if (getItemName == LayerMask.LayerToName(9))
            {
                PlayBGM(touchSE);
                getCurry = true;
            }           
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

    //���̃X�R�A����
    void GetCow(int count)
    {
        //�t�B�[�o�[���̃X�R�A�v�Z
        //if(doPoworUp) _score += (int)(score + score * 0.5);
        //�f�o�t�t�^���̃X�R�A�v�Z
        //if(doPoworDwon) _score += (int)(score - score * 0.3);

        //�ʏ�̃X�R�A���Z
        //�^�b�v�ŏI��点�Ă��鎞
        if(count == 1)
        {
            _score += cowScore;
        }
        //�Ȃ��Ă�����
        if(count >= 2)
        {
            _score += cowScore * count + (int)((cowScore * count) * 0.1);
        }
        
        sm.UpdateScore(_score);
        //�`�F�C������������
        chainCount = 0;
    }

    //�l�Y�~�̃X�R�A���Z
    void GetMouse(int count)
    {
        //�ʏ�̃X�R�A���Z
        //�^�b�v�ŏI��点�Ă��鎞
        if (count == 1)
        {
            _score += mouseScore;
        }
        //�Ȃ��Ă�����
        if (count >= 2)
        {
            _score += mouseScore * count + (int)((mouseScore * count) * 0.1);
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
        //�v�f�̍폜
        am.SponeAnimal(animalInfo.Dequeue());
        //Debug.Log("��������");
        //�����ŏo�͂�������z��ȏ�ɂȂ�����0�ɖ߂��I��������
        if (effect.Length == effectCount) effectCount = 0;
    }

    //�p���[�A�b�v����
    private void DoPowerUp()
    {
        StartCoroutine(PowerUpTime());
    }

    //�p���[�_�E������
    private void DoPowerDown()
    {
        StartCoroutine(PowerDownTime());
    }

    //���j���[��
    public void PlayOpenMenu()
    {
        openMenu = true;
    }

    //���j���[����Q�[���ɖ߂�
    public void BackGameFromMenu()
    {
        openMenu = false;
    }

    private IEnumerator ActiveEffect()
    {
        doSwaip = false;
        //�X�R�A���Z
        //Debug.Log("chainCount" + chainCount);
        if(getAnimalName == GetAnimals.Cow)
        {
            GetCow(chainCount);
        }
        if(getAnimalName == GetAnimals.Mouse)
        {
            GetMouse(chainCount);
        }
        
        //�G�t�F�N�g���Ăяo���I�u�W�F�N�g�̐���0�ɂȂ�܂ōs��
        while (animalInfo.Count != 0)
        {
            //�G�t�F�N�g�Đ�
            //Debug.Log(animalInfo.Count);
            Effect(animalInfo.Peek());
            //�����ɉ����ČĂяo��������ύX����
            if (getAnimalName == GetAnimals.Cow) PlayBGM(cowSE);
            if (getAnimalName == GetAnimals.Mouse) PlayBGM(mouseSE);
            yield return new WaitForSeconds(waitEffectTime);
        }
        //�L�����閼�O�̏�����
        animalName = null;
        getAnimalName = GetAnimals.NULL;
        yield return null;
    }

    //�p���[�A�b�v�����i�w��b���ԍs�������j
    private IEnumerator PowerUpTime()
    {
        playerState = PlayerState.POWERUP;
        yield return new WaitForSeconds(powerupTime);
        getCurry = false;
        playerState = PlayerState.NULL;
    }

    //�p���[�_�E�������i�w��b���ԍs�������j
    private IEnumerator PowerDownTime()
    {
        playerState = PlayerState.POWERDOWN;
        yield return new WaitForSeconds(powerdownTime);
        getRum = false;
        playerState = PlayerState.NULL;
    }
}
