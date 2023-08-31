using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    //�����̃X�s�[�h
    [SerializeField] private float speed;
    //�`�F�C�����̓����̓���
    [SerializeField] private float slowSpeed;
    //�߂܂�������
    private bool canGet = true;

    public enum DoMove
    {
        NULL,
        OK,
        NOT,
        SLOW
    };

    DoMove canMove = DoMove.OK;

    public bool CanGet
    {
        get { return this.canGet;}
        set { this.canGet = value;}
    }

    public DoMove Move
    {
        get { return this.canMove;}
        set { this.canMove = value;}
    }

    private void Update()
    {
        if(canMove == DoMove.OK)
        {
            //�X���C�v���̓����̓���
            if(canMove == DoMove.SLOW)
            {
                this.transform.position += this.transform.up * slowSpeed;
            }
            //���̑��̎��̓���
            if(canMove != DoMove.SLOW)
            {
                this.transform.position += this.transform.up * speed;
            }
            
        }
    }

    public void ResetPositionAnimal(int count)
    {

    }

    //�X�|�i�[�ɐݒu����Ă��铮���𓮂������߂Ɋe�p�����[�^�[������������
    public void ResetPar()
    {
        canMove = DoMove.OK;
        canGet = true;
    }

    //�s����~���ɂ��鏈��
    public void StopAnimal()
    {
        canMove = DoMove.NOT;
    }
}
