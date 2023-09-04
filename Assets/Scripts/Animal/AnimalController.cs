using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class AnimalController : MonoBehaviour
{
    //�����̃X�s�[�h
    [SerializeField] private float normalSpeed;
    //���������擾���Ă��鎞�̃X�s�[�h
    [SerializeField] private float powerdownSpeed;
    //�`�F�C�����̓����̓���
    [SerializeField] private float slowSpeed;
    //�X�e�[�W�Ɏc���ł��鎞��
    [SerializeField] private float stageAlive;
    //�߂܂�������
    private bool canGet = false;
    //�o���������ꂽ�������𔻒肷��t���O
    private bool selectFag = false;
    //�f���Ă��邩�̃t���O
    private bool renderFlag = false;
    //PlayerPlamate
    [SerializeField] private PlayerPalmate pp;
    //AnimalManager
    [SerializeField] private AnimalManager am;
    //SkeletonAnimation
    [SerializeField] private SkeletonAnimation sa;
    //�^�b�`�����Ƃ��ɔ�������F
    [SerializeField] private Color changeColor;
    //���̐F
    [SerializeField] private Color normalColor;
    //�����̃����_�[
    [SerializeField] private Renderer animalRenderer;

    public enum DoMove
    {
        NULL,
        OK,
        NOT,
        SLOW
    };

    DoMove canMove = DoMove.NULL;

    public bool CanGet
    {
        get { return this.canGet;}
        set { this.canGet = value;}
    }

    public bool SelectFlag
    {
        get { return this.selectFag;}
        set { this.selectFag = value;}
    }

    public bool RenderFlag
    {
        get { return this.renderFlag;}
        set { this.renderFlag = value;}
    }

    public DoMove Move
    {
        get { return this.canMove;}
        set { this.canMove = value;}
    }

    private void Update()
    {
        CheckMove();   
        
    }

    //��ԊǗ�
    private void CheckMove()
    {
        //�o����������Ă���I�u�W�F�N�g�ł���Ȃ�
        if(this.selectFag)
        {
            //�󋵔��f�֐�
            CheckGame();   
            //Debug.Log(renderFlag);
        }       
    }

    //�Q�[���̏�Ԃɉ������������s���֐�
    private void CheckGame()
    {
        //�Q�[���J�n���_�܂��̓t�B�[�o�[���o���Ȃ�
        if (pp.DontStart && pp.OpenMenu)
        {
            StopAnimal();
        }

        //�v���C���[���X���C�v���Ȃ�
        if (pp.DoChain)
        {
            this.canMove = DoMove.SLOW;
        }

        //�v���C���[���q���Ă��Ȃ���Ԃł���A���g���Q�[�����ɏo�邱�Ƃ�������Ă���̂Ȃ�
        if(!pp.DoChain && this.selectFag)
        {
            this.canMove = DoMove.OK;
        }

        //�߂܂��Ă����ԂȂ�
        if(!this.canGet)
        {
            ChangeColor();
        }

        //��L�̏�ԈȊO�̏�ԂȂ�
        if (canMove == DoMove.OK)
        {
            MoveAnimal();
        }

        if(this.animalRenderer.isVisible)
        {
            this.renderFlag = true;
        }

        if(!this.animalRenderer.isVisible)
        {
            this.renderFlag = false;
            Debug.Log("�f���ĂȂ���");
        }

        //�����߂܂炸�ɉ�ʊO�ɏo���ꍇ
        if(!renderFlag && this.selectFag)
        {
            am.ReturnAnimal(this.gameObject);
        }
    }

    //�����蔻��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("animal") && this.selectFag)
        {
            //Debug.Log("���]�J�n");
            //�ǉ�����p�x�������_���Ō��肷��
            int randomRad = Random.Range(-90, 90);
            //��������������ϐ�������
            Quaternion animalAngle = this.transform.rotation;
            //���肵���p�x�𓮕��ɕt�^����i�ڐG����͓̂����^�O�̃I�u�W�F�N�g�Ȃ̂ŁA���葤�̔�����s���K�v�͂Ȃ��j
            animalAngle.z += randomRad;
            //Debug.Log("���]����");
        }

        if (collision.gameObject.CompareTag("car") && this.selectFag)
        {
            //Debug.Log("�e���ꂽ");
            am.ReturnAnimal(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("OutStage") && this.selectFag)
        {
            am.BackAnimalList(this.gameObject);
            Debug.Log("�����Ăяo��");
        }
    }

    //����
    private void MoveAnimal()
    {
        //���[�J���ϐ�
        float localSpeed = 0.0f;
      
        //�v���C���[����������������ꍇ
        if (pp.PlayerSituation == PlayerPalmate.PlayerState.POWERDOWN)
        {
            localSpeed = this.powerdownSpeed;
            //�X���C�v���̓����̓���
            if (canMove == DoMove.SLOW)
            {
                localSpeed *= this.slowSpeed;
            }
        }
        //�j���[�g�����ȓ���
        if (this.canMove != DoMove.SLOW && pp.PlayerSituation == PlayerPalmate.PlayerState.NULL)
        {
            localSpeed = this.normalSpeed;
            //�X���C�v���̓����̓���
            if (canMove == DoMove.SLOW)
            {
                localSpeed *= this.slowSpeed;
            }
        }

        //�����ňړ��������s��
        this.transform.position += this.transform.up * localSpeed;
    }

    //�X�|�i�[�ɐݒu����Ă��铮���𓮂������߂Ɋe�p�����[�^�[������������
    public void ResetPar()
    {
        this.canMove = DoMove.OK;
        this.canGet = true;
        this.sa.skeleton.SetColor(normalColor);
    }

    //�s����~���ɂ��鏈��
    public void StopAnimal()
    {
        this.canMove = DoMove.NOT;
        this.canGet = false;
        this.selectFag = false;
    }

    //������getFlag�݂̂�false�ɂ��������Ɏg���֐�
    public void CanselGet() => this.canGet = false;

    //�m�ۂ��ꂽ�ۂɃI�u�W�F�N�g�̃J���[��ύX����
    public void ChangeColor() => sa.skeleton.SetColor(changeColor);

    //�����Ă��鎞
    private void OnBecameVisible()
    {
        
    }

    //�����Ă��Ȃ���
    private void OnBecameInvisible()
    {
        
    }

}