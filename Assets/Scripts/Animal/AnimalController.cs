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
    //
    [SerializeField] private float angleRange;
    //�߂܂�������
    private bool canGet = false;
    //�o���������ꂽ�������𔻒肷��t���O
    private bool selectFag = false;
    //�ҋ@����
    private float waitTime = 0.0f;
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

        //�i�s�����ɓ��������邱�Ƃ����m����֐�
        CheckForwardAnimal();

    }

    //�����蔻��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("animal") && this.selectFag)
        {
            ChangeAngleAnimal();
        }

        if (collision.gameObject.CompareTag("car") && this.selectFag)
        {
            //Debug.Log("�e���ꂽ");
            am.ReturnAnimal(this.gameObject);
        }
    }

    //�폜����
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("OutStage") && this.selectFag)
        {
            am.BackAnimalList(this.gameObject);
        }
    }

    //�ړ�
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

    //�m�ۂ��ꂽ�ۂɃI�u�W�F�N�g�̃J���[��ύX����
    public void ChangeColor() => sa.skeleton.SetColor(changeColor);

    //�i�s�����ɓ��������邩���f����֐�
    public void CheckForwardAnimal()
    {
        //ray���΂��I�u�W�F�N�g���`
        Vector2 rayOriginPos = this.transform.position;
        Vector2 rayAngle = new Vector2(this.transform.rotation.x,this.transform.rotation.y);
        RaycastHit2D rayHitObject = Physics2D.Raycast(rayOriginPos, rayAngle, angleRange);
        Debug.DrawRay(rayOriginPos, rayAngle * angleRange, Color.red, 5f);

        //ray���΂��I�u�W�F�N�g���s����������Ă���I�u�W�F�N�g�����������I�u�W�F�N�g�̃^�O��animal�Ȃ�
        if(rayHitObject.collider.gameObject.CompareTag("animal") && this.selectFag)
        {
            ChangeAngleAnimal();
        }
    }

    //���񏈗�
    public void ChangeAngleAnimal()
    {
        //�ǉ�����p�x�������_���Ō��肷��
        float randomNum = Random.Range(1, 2);
        float randomRad = 0;
        if (randomNum % 2 == 0) randomRad = 120.0f;
        if (randomRad % 2 != 0) randomRad = -120.0f;
        //��������������ϐ�������
        Quaternion animalAngle = this.transform.rotation;
        //���肵���p�x�𓮕��ɕt�^����i�ڐG����͓̂����^�O�̃I�u�W�F�N�g�Ȃ̂ŁA���葤�̔�����s���K�v�͂Ȃ��j
        animalAngle.z += randomRad;
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
    public void NotGet() => this.canGet = false;
}