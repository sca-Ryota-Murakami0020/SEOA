using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManeger;

public class CurryMoverC : MonoBehaviour
{
    //�J���[�̋����̋���
    private bool canMove = false;
    //���֓������̔���
    private bool moveLeft = false;
    //�J���[�̃X�s�[�h
    [SerializeField] private float _speed;
    //�J���[�̏o����
    private int sponeCurryCount = 0;
    //TimeManager
    [SerializeField] private TimeManager tm;
    //�����ʒu
    [SerializeField] private GameObject resetPosObject;
    //�X�|�i�[
    [SerializeField] private GameObject[] currySponer;

    //�v���p�e�B
    public bool CanMove
    {
        get { return this.canMove;}
    }

    // Start is called before the first frame update
    void Start()
    {
        //�����ʒu�ɔz�u
        ResetCurryPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && tm.DoCount)
        {
            MoveCurry();
        }
    }

    //�Փ˔���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�Ԃɓ���������J���[�������ʒu�ɖ߂�
        if(collision.gameObject.CompareTag("car"))
        {
            ResetCurryPos();
        }
    }

    //�폜����
    private void OnTriggerExit2D(Collider2D collision)
    {
        //�����ʒu�ɖ߂�
        if (collision.gameObject.CompareTag("OutStage"))
        {
            ResetCurryPos();
        }
    }

    //�s����������
    public void ActiveCanFlag()
    {
        canMove = true;
    }

    //�s���𐧌�����
    public void NoActiveCanFlag()
    {
        canMove = false;
    }

    //�i�s���������ɂ���
    public void MoveLeft()
    {
        moveLeft = true;
    }

    //�J���[�̋���
    public void MoveCurry()
    {
        //������
        if(moveLeft) this.transform.position += this.transform.right * _speed * -1;
        //�E����
        if(!moveLeft) this.transform.position += this.transform.right * _speed ;
    }

    //�����ʒu�ɖ߂�
    public void ResetCurryPos()
    {
        Vector2 resetPos = resetPosObject.transform.position;
        this.transform.position = resetPos;
        //�s���𐧌�����
        NoActiveCanFlag();
    }

    //�X�|�i�[�̈ʒu�ɔz�u����
    public void SetCurry()
    {
        if(sponeCurryCount <= 2)
        {
            //�X�|�i�[�̈ʒu�����߂�
            int sponerPos = Random.Range(0, currySponer.Length);
            //�X�|�i�[�ֈړ�
            this.transform.position = currySponer[sponerPos].transform.position;
            //�I�΂ꂽ�X�|�i�[���E���̏ꍇ
            if (sponerPos == 2 || sponerPos == 3)
            {
                //�J���[�̐i�s�������������Ɍ��肷��
                MoveLeft();
            }
            //�J���[�̐i�s��������
            ActiveCanFlag();
            //�o���񐔂𑝂₷
            sponeCurryCount++;
        }     
    }
}
