using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManeger;

public class RumC : MonoBehaviour
{
    [SerializeField] private TimeManager tm;

    [SerializeField] private GameObject resetPos; 

    [SerializeField] private GameObject[] rumSponer;

    [SerializeField] private float _speed;

    private bool canGet = false;

    private bool moveLeft = false;

    public bool CanGet
    {
        get { return this.canGet;}
        set { this.canGet = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetRumPos();
    }

    // Update is called once per frame
    void Update()
    {
        if(canGet && tm.DoCount)
        {
            MoveRum();
        }
    }

    public void SetRum()
    {
        //�X�|�i�[�̈ʒu�����߂�
        int sponerPos = Random.Range(0, rumSponer.Length);
        //�X�|�i�[�ֈړ�
        this.transform.position = rumSponer[sponerPos].transform.position;
        //�I�΂ꂽ�X�|�i�[���E���̏ꍇ
        if (sponerPos == 2 || sponerPos == 3)
        {
            //�J���[�̐i�s�������������Ɍ��肷��
            MoveLeft();
        }
        //�J���[�̐i�s��������
        ActiveCanFlag();
    }

    private void MoveLeft()
    {
        moveLeft = true;
    }

    private void MoveRum()
    {
        //������
        if (moveLeft) this.transform.position += this.transform.right * _speed * -1;
        //�E����
        if (!moveLeft) this.transform.position += this.transform.right * _speed;
    }

    private void ActiveCanFlag()
    {
        canGet = true;
    }

    private void ResetRumPos()
    {
        this.transform.position = resetPos.transform.position;
    }

    //�Փ˔���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�Ԃɓ���������J���[�������ʒu�ɖ߂�
        if (collision.gameObject.CompareTag("car"))
        {
            ResetRumPos();
        }
    }

    //�폜����
    private void OnTriggerExit2D(Collider2D collision)
    {
        //�����ʒu�ɖ߂�
        if (collision.gameObject.CompareTag("OutStage"))
        {
            ResetRumPos();
        }
    }
}
