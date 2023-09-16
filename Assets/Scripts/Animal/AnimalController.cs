using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManeger;
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
    //�߂܂�������
    private bool canGet = false;
    //�o���������ꂽ�������𔻒肷��t���O
    private bool selectFag;
    //�ҋ@����
    private float waitTime = 0.0f;
    //��]��
    private bool doTurn = false;
    //PlayerPlamate
    [SerializeField] private PlayerPalmate pp;
    //TimeManager
    [SerializeField] private TimeManager tm;
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

    #region//�v���p�e�B
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
    #endregion

    //��ԊǗ�
    private void CheckMove()
    {
        //�o����������Ă���I�u�W�F�N�g�ł���Ȃ�
        if(this.selectFag)
        {
            //�󋵔��f�֐�
            CheckGame();
            //Debug.Log(tm.DoCount);
        }       
    }

    //�Q�[���̏�Ԃɉ������������s���֐�
    private void CheckGame()
    {     
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
        if (canMove != DoMove.NOT && tm.DoCount)
        {
            MoveAnimal();

            //�t�B�[�o�[���o��or�|�[�Y���Ȃ�
            if (!tm.DoCount)
            {
                StopAnimal();
            }
        }
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

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�i�s�����ɓ����������
        if((collision.gameObject.CompareTag("plusAngle")||collision.gameObject.CompareTag("animal"))
            && this.selectFag && !doTurn)
        {
            ChangeAngleAnimal();
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

    //���񏈗�
    public void ChangeAngleAnimal()
    {
        //��������������ϐ�������
        //StartCoroutine(TurnAnimalAction());
    }

    //�X�|�i�[�ɐݒu����Ă��铮���𓮂������߂Ɋe�p�����[�^�[������������
    public void ResetPar()
    {
        this.canMove = DoMove.OK;
        this.canGet = true;
        this.selectFag = true;
        this.sa.skeleton.SetColor(normalColor);
        //Debug.Log("�ύX�͌�������");
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

    /*
    //����s��
    private IEnumerator TurnAnimalAction()
    {
        //�X�s�[�h��ύX����i�����j
        this.canMove = DoMove.SLOW;
        doTurn = true;
        //�ǉ�����p�x�������_���Ō��肷��
        float randomNum = Random.Range(1, 2);
        bool pulsRad = false;
        if (randomNum % 2 == 0) pulsRad = true;
        if (randomNum % 2 != 0) pulsRad = false;
        //��]��ϐ���
        Quaternion animalAngle = this.transform.rotation;
        //���肵���p�x�𓮕��ɕt�^����i�ڐG����͓̂����^�O�̃I�u�W�F�N�g�Ȃ̂ŁA���葤�̔�����s���K�v�͂Ȃ��j
        int countRad = 0;
        float addRad = 1.0f;
        
        while(countRad < 120)
        {
            if(pulsRad)
            {
                //animalAngle.z += addRad;
                //this.transform.rotation = Quaternion.AngleAxis(addRad, this.transform.forward);Space.Self)
                transform.Rotate(0, 0, addRad);
            }
            if(!pulsRad)
            {
                //animalAngle.z -= addRad;
                //this.transform.rotation = Quaternion.AngleAxis(-addRad, this.transform.forward);
                transform.Rotate(0, 0, -addRad);
            }
            //this.transform.rotation = animalAngle;
            countRad ++;
            yield return new WaitForEndOfFrame();
        }

        //�X�s�[�h�����̐��l�ɒ���
        this.canMove = DoMove.OK;
        doTurn = false;
        yield break;
    }*/
}