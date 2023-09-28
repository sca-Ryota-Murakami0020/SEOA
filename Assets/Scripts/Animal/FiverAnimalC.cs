using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Spine;
using System;
using Spine.Unity;
using GameManeger;

public class FiverAnimalC : MonoBehaviour
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
    //FiverAnimalMAnager
    [SerializeField] private FiverAnimalManager fiverAM;
    //SkeletonAnimation
    [SerializeField] private SkeletonAnimation sa;
    //�^�b�`�����Ƃ��ɔ�������F
    [SerializeField] private Color changeColor;
    //���̐F
    [SerializeField] private Color normalColor;
    //�����̓����蔻��
    [SerializeField] private Collider2D animalCollider2d;
    //������Rigidbody2d
    [SerializeField] private Rigidbody2D rb2d;

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
        get { return this.canGet; }
        set { this.canGet = value; }
    }

    public bool SelectFlag
    {
        get { return this.selectFag; }
        set { this.selectFag = value; }
    }

    public DoMove Move
    {
        get { return this.canMove; }
        set { this.canMove = value; }
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
        if (this.selectFag)
        {
            //�󋵔��f�֐�
            CheckGame();
            //Debug.Log(tm.DoCount);
        }
        if(!this.selectFag)
        {
            NoActiveisTriggerAnimal();
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
        if (!pp.DoChain && this.selectFag)
        {
            this.canMove = DoMove.OK;
        }

        //�߂܂��Ă����ԂȂ�
        if (!this.canGet)
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
            ActiveisTriggerAnimal();
        }
    }

    //�����蔻��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�i�s�����ɓ���������Γ����蔻�������
        if ((collision.gameObject.CompareTag("plusAngle") || collision.gameObject.CompareTag("animal"))
            && this.selectFag && !doTurn)
        {
            NoActiveisTriggerAnimal();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //�X�e�[�W�O�Ɍ������Ă�����
        if (collision.gameObject.CompareTag("OutStage") && this.selectFag)
        {
            string animalName = LayerMask.LayerToName(this.gameObject.layer);
            //���̃I�u�W�F�N�g�̃��C���[�����Ȃ�
            if(animalName == LayerMask.LayerToName(6))
            {
                fiverAM.FiverSponeCow(this.gameObject);
            }
            //���̃I�u�W�F�N�g�̃��C���[���l�Y�~�Ȃ�
            if (animalName == LayerMask.LayerToName(7))
            {
                fiverAM.FiverSponeMouse(this.gameObject);
            }
        }

        //�������m�����蔲�������Ă�����
        if(collision.gameObject.CompareTag("animal") && this.selectFag)
        {
            ActiveisTriggerAnimal();
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

    //�����蔻��t�^
    public void ActiveisTriggerAnimal()
    {
        //��������������ϐ�������
        animalCollider2d.isTrigger = true;
        rb2d.isKinematic = true;
    }

    //�����蔻�����
    public void NoActiveisTriggerAnimal()
    {
        animalCollider2d.isTrigger = false;
        rb2d.isKinematic = false;
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
}
