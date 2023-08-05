using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEffect : MonoBehaviour
{
    //�G�t�F�N�g
    [Header("�Ăяo���G�t�F�N�g")][SerializeField] private PlayEffect[] effects;
    //���݂̃G�t�F�N�g
    private int effectCurrent = 0;
    //�G�t�F�N�g�Đ�����ꏊ
    private Queue<Vector2> animalInfo = new Queue<Vector2>();

    //�G�t�F�N�g�Đ�����
    private bool isEffect = false;


    private void Update()
    {
        if (isEffect) return; // �G�t�F�N�g�Đ����͏������Ȃ�

        if (Input.GetKeyDown(KeyCode.A))
        {
            //���W�̋L��
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            animalInfo.Enqueue(pos);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //���o�����J�n
            Debug.Log("StartEffect");
            StartCoroutine(ActiveEffect());
        }
    }
    private void Effect(Vector2 pos)
    {
        // �G�t�F�N�g�Đ�����(���W)
        effects[effectCurrent].Play(pos);
        // ���̃G�t�F�N�g�Đ����鏀��
        effectCurrent++;
        //�G�t�F�N�g�̌Ăяo������ʂ�I�������J�E���g��0�ɂ���
        if (effectCurrent == effects.Length) effectCurrent = 0;
    }

    private IEnumerator ActiveEffect()
    {
        //�G�t�F�N�g�Đ����̔���ɂ���
        isEffect = true;
        //Queue�̒��g�̗v�f����0�ł͂Ȃ����菈�����s��
        while (animalInfo.Count != 0)
        {
            //�G�t�F�N�g�Đ�
            Effect(animalInfo.Dequeue());
            yield return new WaitForSeconds(0.1f);
        }
        //�������I��������G�t�F�N�g�Đ����I������
        isEffect = false;
    }
}
