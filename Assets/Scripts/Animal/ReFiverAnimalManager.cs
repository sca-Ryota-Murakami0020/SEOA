using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManeger;

public class ReFiverAnimalManager : MonoBehaviour
{
    #region//�I�u�W�F�N�g�֌W
    //�X�|�i�[
    [SerializeField] private GameObject[] feverAnimalSponer;
    //���̃I�u�W�F�N�g
    [SerializeField] private GameObject feverCowObject;
    //�l�Y�~�̃I�u�W�F�N�g
    [SerializeField] private GameObject feverMouseObject;
    #endregion

    #region//�ϐ��֌W
    //�O��̃X�|�i�[�̔ԍ�
    private int oldSponerNumber = 0;
    //�ő�ҋ@����
    [Header("�ő�ҋ@����")]
    [SerializeField]
    private int maxWaitTime;
    //�ŏ��ҋ@����
    [Header("�ŏ��ҋ@����")]
    [SerializeField]
    private int minWaitTime;
    //PlayerPalmeta
    [SerializeField] private PlayerPalmate pp;
    #endregion

    public static AnimalManager instance;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetAnimals()
    {
        //�ŏ��̔z�u
        for (int count = 0; count < feverAnimalSponer.Length; count++)
        {
            //�Ăяo��������ݒ肷��
            GameObject selectAnimal = SelectAnimal();
            //�z�u��̃X�|�i�[����I�΂ꂽ�������Ăяo��
            Instantiate(SelectAnimal(),
                feverAnimalSponer[count].transform.position,
                feverAnimalSponer[count].transform.rotation);
        }
    }

    public GameObject SelectAnimal()
    {
        GameObject selectObject = null;
        int randNum = Random.Range(0, 2);
        int num = randNum % 2;
        //���̏o��
        if (num != 0) selectObject = feverCowObject;
        //
        if (num == 0) selectObject = feverMouseObject;
        Debug.Log("�Ăяo���I�u�W�F�N�g" + selectObject);
        return selectObject;
    }

    //�������X�|�i�[�ɌĂяo��
    public void SponeAnimal()
    {
        //���[�J���ϐ������
        int randNum = 0;
        //�z�u��̐ݒ�
        while (true)
        {
            randNum = Random.Range(0, feverAnimalSponer.Length - 1);
            //�O��o�͂����X�|�i�[�ƈقȂ�Ȃ�
            //�V�����Ăяo���X�|�i�[�����肷��
            if (randNum != oldSponerNumber)
            {
                break;
            }
        }

        //�X�|�i�[��݂��A�I�΂ꂽ�������Ăяo��
        //���̍ۂɃX�|�i�[�������Ă����]�Ɠ����l�𓮕��ɗ^����
        Instantiate(SelectAnimal(),
            feverAnimalSponer[randNum].transform.position,
            feverAnimalSponer[randNum].transform.rotation);
    }
}
