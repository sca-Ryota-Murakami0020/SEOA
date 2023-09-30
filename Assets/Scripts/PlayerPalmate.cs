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
    [SerializeField] private scoreManager scoreManager;
    //TimeManager
    [SerializeField] private TimeManager timeManager;
    //GameManager
    [SerializeField] private GameManager gameManager;
    //Audiosource
    private AudioSource audios = null;
    //AnimalManager
    [SerializeField] private AnimalManager animalManager;
    //ActiveManager
    [SerializeField] private ActiveManager activeManager;
    //�G�t�F�N�g
    [Header("�Ăяo���G�t�F�N�g")] [SerializeField]
    private effectsC[] effect;
    #endregion

    #region//�ϐ�
    //�X���C�v�����̔���
    private bool doSwaip = false;
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
    //�l����������
    private Queue<GameObject> animalInfo = new Queue<GameObject>();
    //�t�B�[�o�[���Ɋl����������
    private Queue<GameObject> fiverAnimalInfo = new Queue<GameObject>();
    #endregion

    #region//�C���X�y�N�^�[�ŕύX����l
    //���̃X�R�A
    [SerializeField] private int cowScore;
    //�l�Y�~�̃X�R�A
    [SerializeField] private int mouseScore;
    //�G�t�F�N�g�o���Ԋu����
    [SerializeField] private float waitEffectTime;
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
        set { this.doSwaip = value;}
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

    public static PlayerPalmate instance;

    // Start is called before the first frame update
    void Start()
    {
        audios = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeManager.DoCount)
        {
            //����
            if (Input.GetMouseButton(0))
            {
                TryCatchingAnimals();
            }
            //�}�E�X�𗣂���or�w�𗣂����ꍇ
            if (Input.GetMouseButtonUp(0))
            {
                if(timeManager.DoingFiver)
                {
                    StartCoroutine(FiverActiveEffect());
                }

                else
                {
                    //�X�R�A���i�[�����X�N���v�g�������ŎQ�Ƃ���
                    StartCoroutine(ActiveEffect());
                }
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
        
        //�ʏ�̓�������
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
                //FiverAnimalC fiverAnimalC = hit2d.collider.gameObject.GetComponent<FiverAnimalC>();
                AnimalController ac = hit2d.collider.gameObject.GetComponent<AnimalController>();
                //���������ɓ������Ă��܂��Ă�����ȍ~�̏����͂��Ȃ�
                if (ac.AlreadyGet) return;

                //�����Ȃ��悤�Ƃ��Ă��铮������ԍŏ��̓����Ɠ����ꍇ
                if (!ac.AlreadyGet)
                {
                    PlayBGM(touchSE);
                    ac.ChangeColor();
                    ac.GetAnimal();
                    //�v�f�̖��[�ɒǉ�����
                    animalInfo.Enqueue(hit2d.collider.gameObject);                    
                    //�Ȃ��Ă��鐔���X�V
                    chainCount += 1;
                }
            }
        }

        //�t�B�[�o�[�p�̓�����߂܂�����
        if(hit2d.collider.tag == "fiverAnimal")
        {
            doSwaip = true;
            //���C���[�}�X�N���l������
            string getAnimalName = LayerMask.LayerToName(hit2d.collider.gameObject.layer);
            //�����ێ����Ă��閼�O������������
            if (this.getAnimalName == GetAnimals.NULL)
            {
                //�ŏ��̃f�[�^�����ɕύX
                if (getAnimalName == LayerMask.LayerToName(6))
                {
                    this.getAnimalName = GetAnimals.Cow;
                }
                //�ŏ��̃f�[�^���l�Y�~�ɕύX
                if (getAnimalName == LayerMask.LayerToName(7))
                {
                    this.getAnimalName = GetAnimals.Mouse;
                }
            }

            if (this.getAnimalName != GetAnimals.NULL && this.getAnimalName.ToString() == getAnimalName)
            {
                //�߂܂��������̊֐����擾����
                //AnimalController animalController = hit2d.collider.gameObject.GetComponent<AnimalController>();
                FeverAnimalC fiverAnimalC = hit2d.collider.gameObject.GetComponent<FeverAnimalC>();

                //���������ɓ������Ă��܂��Ă�����ȍ~�̏����͂��Ȃ�
                if (fiverAnimalC.AlreadyGet) return;

                //�����Ȃ��悤�Ƃ��Ă��铮������ԍŏ��̓����Ɠ����ꍇ
                if (!fiverAnimalC.AlreadyGet)
                {
                    PlayBGM(touchSE);
                    //Debug.Log("�F�̕ύX�J�n");
                    fiverAnimalC.ChangeColor();
                    fiverAnimalC.NotGet();
                    //�v�f�̖��[�ɒǉ�����
                    fiverAnimalInfo.Enqueue(hit2d.collider.gameObject);
                    //�Ȃ��Ă��鐔���X�V
                    chainCount += 1;
                }
            }
        }

        //�G�ꂽ�I�u�W�F�N�g���A�C�e���Ȃ�
        if(hit2d.collider.tag == "item" && !timeManager.DoingFiver)
        {
            //if(!doSwaip) doSwaip = true;
            //Debug.Log("�Ȃ����Ă���");
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

    //�ʏ�̃G�t�F�N�g����
    private void Effect(GameObject animal)
    {
        //�����̈ʒu�ɃG�t�F�N�g���Ăяo��
        effect[effectCount].PlayEffect(animal);
        //�Ȃ������𑝂₷
        effectCount++;
        //���̏o�͂��s��
        animalManager.SponeAnimal();
        //�����ŏo�͂�������z��ȏ�ɂȂ�����0�ɖ߂��I��������
        if (effect.Length == effectCount) effectCount = 0;
    }

    //�t�B�[�o�[�p�̃G�t�F�N�g�\���E�X�R�A���Z
    private void FiverEffect(GameObject getAnimal)
    {
        //�l�����������̃��C���[�����l������
        string animalName = LayerMask.LayerToName(getAnimal.gameObject.layer);
        //�߂܂��������̏ꏊ�ɃG�t�F�N�g���Ăяo��
        effect[effectCount].PlayEffect(getAnimal);
        //�Ȃ������𑝂₷
        effectCount++;
        //�v�f�̍폜�B���ꂼ��̃��X�g�ɖ߂��R���[�`�����Ăяo���֐����Ăяo��
        animalManager.SelectSponeFeverAnimal();

        //�����ŏo�͂�������z��ȏ�ɂȂ�����0�ɖ߂��I��������
        if (effect.Length == effectCount) effectCount = 0;
    }

    //�ʏ�̓����̊l��
    private IEnumerator ActiveEffect()
    {
        doSwaip = false;
        //�X�R�A���Z
        //���̃J�E���g
        if(getAnimalName == GetAnimals.Cow)
        {
            scoreManager.AddScore(cowScore, chainCount);
        }
        //�l�Y�~�̃J�E���g
        if(getAnimalName == GetAnimals.Mouse)
        {
            scoreManager.AddScore(mouseScore, chainCount);
        }
        //�q��������������
        chainCount = 0;
        //�G�t�F�N�g���Ăяo���I�u�W�F�N�g�̐���0�ɂȂ�܂ōs��
        while (animalInfo.Count != 0)
        {
            //�G�t�F�N�g�Đ�
            //Debug.Log(animalInfo.Count);
            Effect(animalInfo.Dequeue());
            //�����ɉ����ČĂяo��������ύX����
            if (getAnimalName == GetAnimals.Cow) PlayBGM(cowSE);
            if (getAnimalName == GetAnimals.Mouse) PlayBGM(mouseSE);
            yield return new WaitForSeconds(waitEffectTime);
        }
        //�L�����閼�O�̏�����
        animalName = null;
        getAnimalName = GetAnimals.NULL;
        //�J���[���擾���Ă���ꍇ
        if(getCurry)
        {
            //�t�B�[�o�[�^�C���̃A�j���[�V�����𓮂���
            activeManager.ActiveFiverTime();
            getCurry = false;
        }
        yield return null;
    }

    //�t�B�[�o�[�p�̃G�t�F�N�g�E�X�R�A����
    private IEnumerator FiverActiveEffect()
    {
        doSwaip = false;
        //�X�R�A���Z
        //���̃J�E���g
        if (getAnimalName == GetAnimals.Cow)
        {
            scoreManager.FiverAddScore(cowScore, chainCount);
        }
        //�l�Y�~�̃J�E���g
        if (getAnimalName == GetAnimals.Mouse)
        {
            scoreManager.FiverAddScore(mouseScore, chainCount);
        }
        //�q��������������
        chainCount = 0;
        //�G�t�F�N�g���Ăяo���I�u�W�F�N�g�̐���0�ɂȂ�܂ōs��
        while (fiverAnimalInfo.Count != 0)
        {
            //�G�t�F�N�g�Đ�
            //Debug.Log(animalInfo.Count);
            FiverEffect(fiverAnimalInfo.Dequeue());
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
}
