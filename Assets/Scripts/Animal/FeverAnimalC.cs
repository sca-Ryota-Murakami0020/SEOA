using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Spine;
using System;
using Spine.Unity;
using GameManeger;

public class FeverAnimalC : MonoBehaviour
{
    //TimeManager
    private TimeManager tm;
    //PlayerPlamate
    private PlayerPalmate pp;
    //AnimalManager
    private AnimalManager am;

    //�߂܂���ꂽ�t���O
    private bool alreadyGet = false;
    //���x
    [SerializeField] private float normalSpeed;
    //�X���C�v���̃X�s�[�h
    [SerializeField] private float swaipSpeed;

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

    //�X�e�[�^�X�i�v���C���[�̏�Ԃɂ��j
    public enum DoMove
    {
        NOT,
        MOVE,
        SLOW
    };

    DoMove canMove = DoMove.NOT;

    #region//�v���p�e�B
    public bool AlreadyGet
    {
        get { return this.alreadyGet; }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        tm = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        pp = GameObject.Find("Player").GetComponent<PlayerPalmate>();
        am = GameObject.Find("AnimalManagerObject").GetComponent<AnimalManager>();
    }

    private void Update()
    {
        //�Q�[�����i�s���Ȃ�
        if (tm.DoCount)
        {
            SelectState();
            SelectMove();
        }

        //Debug.Log("debag" + TimeManager.instance.DoCount);

        //�Q�[���i�s�����f���ꂽ��
        if (!tm.DoCount && tm.DoingFiver)
        {
            this.canMove = DoMove.NOT;
        }

        //�t�B�[�o�[���I�������炱�̃I�u�W�F�N�g��j�󂷂�
        if (!tm.DoingFiver)
        {
            Debug.Log("haitta");
            Destroy(this.gameObject);
        }

        //Debug.Log("DoCount:" + tm.DoingFiver);
    }

    //���݂̃Q�[����Ԃ𒲂ׂ�i�|�[�Y���A�v���C���Ȃǁj
    private void SelectState()
    {
         //�v���C���[���X���C�v���Ȃ�
         if (pp.DoChain)
         {
             this.canMove = DoMove.SLOW;
         }

         //�v���C���[���q���Ă��Ȃ���ԂȂ�
         if (!pp.DoChain)
         {
             this.canMove = DoMove.MOVE;
         }
    }

    //�s���̏���������s��
    private void SelectMove()
    {
        //�s�����@�̑I��
        switch (canMove)
        {
            //�Q�[����~��
            case DoMove.NOT:
                StopAnimal();
                break;
            //�ʏ��̓���
            case DoMove.MOVE:
                Move();
                break;
            //�q���Ă�����
            case DoMove.SLOW:
                 SlowMove();
                break;
        }

        //�߂܂��Ă����ԂȂ�
        if (this.alreadyGet)
        {
            ChangeColor();
        }
    }

    //���j���[�J�����̓���
    private void StopAnimal() => this.transform.position += this.transform.up * 0.0f;

    //�ʏ�̓���
    private void Move() => this.transform.position += this.transform.up * normalSpeed;

    //�q���Ă��鎞�̓���
    private void SlowMove() => this.transform.position += this.transform.up * normalSpeed * swaipSpeed;

    //�߂܂��Ă����Ԃ̎��ɐF��ς���
    //�m�ۂ��ꂽ�ۂɃI�u�W�F�N�g�̃J���[��ύX����
    public void ChangeColor() => sa.skeleton.SetColor(changeColor);

    //�m�ۍς݂ɂ���
    //public void GetAnimal() => this.alreadyGet = true;

    //�����蔻��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("animal"))
        {
            ActiveisTriggerAnimal();
        }

        if (collision.gameObject.CompareTag("car"))
        {
            //���̃X�|�i�[�ɓ����𐶐�������
            am.SelectSponeFeverAnimal();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�i�s�����ɓ����������
        if ((collision.gameObject.CompareTag("plusAngle") || collision.gameObject.CompareTag("animal")))
        {
            NoActiveisTriggerAnimal();
        }
    }

    //�폜����
    private void OnTriggerExit2D(Collider2D collision)
    {
        //��ʊO�ɏo���ꍇ�A���X�g�̖��[�ɖ߂�
        if (collision.gameObject.CompareTag("OutStage"))
        {
            am.SelectSponeFeverAnimal();
            Destroy(this.gameObject);
        }
        //�������m�����蔲�������Ă�����
        if (collision.gameObject.CompareTag("animal"))
        {
            ActiveisTriggerAnimal();
        }
    }

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
        rb2d.isKinematic = true;
        //Debug.Log("�����蔻�����");
    }
    public void NotGet() => this.alreadyGet = true;
}
