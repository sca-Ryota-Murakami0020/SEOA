using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using GameManeger;

public class PlayerPalmate : MonoBehaviour
{
    #region//�Q�Ƃ���X�N���v�g
    //scoreManager
    [SerializeField] private scoreManager sm;
    //TimeManager
    [SerializeField] private TimeManager tm;
    //GameManager
    [SerializeField] private GameManager gm;
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
    //�J���[���擾�����ꍇ
    private bool getCurry = false;
    //���������擾�����ꍇ
    private bool getRum = false;
    //�l�����������̖��O
    private string animalName = null;
    //�G�t�F�N�g���Ăяo������
    private int effectCount = 0;
    //�Ȃ�����
    private int chainCount = 0;
    //�l������
    private Queue<GameObject> animalInfo = new Queue<GameObject>();
    #endregion

    #region//�C���X�y�N�^�[�ŕύX����l
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
    private PlayerState playerState = PlayerState.NULL;
    private GetAnimals getAnimalName = GetAnimals.NULL;
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

    #endregion

    #region//�v���p�e�B
    public int EffectCount
    {
        get { return this.effectCount;}
        set { this.effectCount = value;}
    }

    public int ChainCount
    {
        get { return this.chainCount;}
        set { this.chainCount = value;}
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
        audios = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(tm.DoCount)
        {
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
                    animalController.NotGet();
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

    private IEnumerator ActiveEffect()
    {
        doSwaip = false;
        //�X�R�A���Z
        //���̃J�E���g
        if(getAnimalName == GetAnimals.Cow)
        {
            sm.AddScore(cowScore, chainCount);
        }
        //�l�Y�~�̃J�E���g
        if(getAnimalName == GetAnimals.Mouse)
        {
            sm.AddScore(mouseScore, chainCount);
        }
        //�q��������������
        chainCount = 0;
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
