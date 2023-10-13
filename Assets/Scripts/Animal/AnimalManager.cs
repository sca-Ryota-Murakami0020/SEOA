using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManager;

public class AnimalManager : MonoBehaviour
{
    /*
    #region//�ϐ��֌W
    //�O��̃X�|�i�[�̔ԍ�
    private int oldSponerNumber;
    //�ő�ҋ@����
    [Header("�ő�ҋ@����")][SerializeField] 
    private int maxWaitTime;
    //�ŏ��ҋ@����
    [Header("�ŏ��ҋ@����")][SerializeField]
    private int minWaitTime;
    //PlayerPalmeta
    [SerializeField] private PlayerPalmate pp;
    #endregion

    #region//���X�g�֌W
    [Header("�Ăяo���I�u�W�F�N�g")]
    [SerializeField]
    private GameObject[] sponerObject;    
    //�i�[���铮���B
    [SerializeField]
    private GameObject[] animals;
    //�i�[���Ă��铮���B
    private Queue<GameObject> animalIndex = new Queue<GameObject>();
    #endregion

    //�v���p�e�B
    public Queue<GameObject> Animals
    {
        get { return this.animalIndex;}
        set { this.animalIndex = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        //������
        animalIndex = new Queue<GameObject>();
        oldSponerNumber = 0;
        for(int count = 0; count < animals.Length; count++)
        {
            animals[count].transform.position = this.transform.position;
            animals[count].transform.rotation = this.transform.rotation;
            animalIndex.Enqueue(animals[count]);
        }
    }

    #region//�֐�
    //�����̏����z�u
    public void SetAnimals()
    {
        //�������e�X�|�i�[�ɏo��������
        for (int count = 0; count < sponerObject.Length; count++)
        {
            //�o������ʒu�Ɛi�s������^����
            GameObject sponeAnimal = animalIndex.Dequeue();
            sponeAnimal.transform.position = sponerObject[count].transform.position;
            sponeAnimal.transform.rotation = sponerObject[count].transform.rotation;
            AnimalController an = sponeAnimal.GetComponent<AnimalController>();
            an.ResetPar();
        }
    }

    //�X�|�i�[�̂Ƃ���ɓ�����z�u����
    public void SetSponerAnimal(GameObject animal)
    {
        //�����ŔԒn�̐ݒu���s��
        while (true)
        {
            int selectNumber = Random.Range(0, sponerObject.Length);

            if (oldSponerNumber != selectNumber)
            {
                oldSponerNumber = selectNumber;
                break;
            }
        }
        //�ʒu�̐ݒ�
        animal.transform.position = sponerObject[oldSponerNumber].transform.position;
        //��]�̐ݒ�
        animal.transform.rotation = sponerObject[oldSponerNumber].transform.rotation;
        Debug.Log("�Ăяo����");
    }

    //�����̈ʒu�������ʒu�ɖ߂�
    public void ResetPositionAnimals(GameObject animal)
    {
        //�ʒu�̕ύX
        animal.transform.position = this.transform.position;
        //�����̕ύX
        animal.transform.rotation = this.transform.rotation;
    }

    //�z�u���铮���̊i�[���̍X�V
    public void SponeAnimal(GameObject animal)
    {       
        //�֐����Q��
        AnimalController an = animal.gameObject.GetComponent<AnimalController>();
        //�ʒu�Ɖ�]��������
        ResetPositionAnimals(animal);
        //�����̃X�e�[�^�X��������
        an.StopAnimal();
        //animalIndex.Enqueue(animal);
        StartCoroutine(SponeAnimalActive(animal));
    }


    //�߂܂������ɍs�������̏���
    //�Ăяo�����[��PlayerPalamate,AnimalController
    public void ReturnAnimal(GameObject setAnimal)
    {
        //���W�̏�����
        ResetPositionAnimals(setAnimal);
        AnimalController an = setAnimal.gameObject.GetComponent<AnimalController>();
        //�����Ń��X�g�ɒǉ����铮���̏������Z�b�g����
        an.StopAnimal();
    }

    //��ʊO�ɏo�����ɓ��������X�g�ɒǉ����āA�V����������ǉ�����
    //�Ăяo�����[��AnimalController
    public void BackAnimalList(GameObject backAnimal)
    {
        //���W�̏�����
        ResetPositionAnimals(backAnimal);
        AnimalController an = backAnimal.gameObject.GetComponent<AnimalController>();
        //�߂����������s���s��Ԃɂ���
        an.StopAnimal();
        //�����Ń��X�g�ɍē�������
        animalIndex.Enqueue(backAnimal);
        //�z�u�R���[�`�����Ăяo��
        StartCoroutine(NormalSponeAnimal());
    }
    #endregion

    #region//�R���[�`������
    //�ʏ�̓����̐ݒu����
    private IEnumerator SponeAnimalActive(GameObject animal)
    {
        //�߂܂������������X�g�̖��[�ɒǉ�����
        animalIndex.Enqueue(animal);
        //���X�g�̓���ϐ���
        GameObject sponeAnimal = animalIndex.Dequeue(); 
        //�֐��̎擾
        AnimalController ac = sponeAnimal.GetComponent<AnimalController>();
        //�����Ŏ��̏ꏊ�����܂�܂ŏ������s��
        SetSponerAnimal(sponeAnimal);
        //�ҋ@���Ԃ�ݒ�
        int waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);
        //�����œ����������悤�ɂ���
        ac.ResetPar();
    }

    //��ʊO�ɏo�����ɓ������Ăяo������
    private IEnumerator NormalSponeAnimal()
    {
        //���X�g�̓���ϐ���
        GameObject sponeAnimal = animalIndex.Dequeue();
        //�֐��̎擾
        AnimalController ac = sponeAnimal.GetComponent<AnimalController>();
        //�ݒu����
        SetSponerAnimal(sponeAnimal);
        //�ҋ@���Ԃ�ݒ�
        int waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);
        //�����œ����������悤�ɂ���
        ac.ResetPar();
    }
    #endregion
    */

    #region//�I�u�W�F�N�g�֌W
    //�X�|�i�[
    [SerializeField] private GameObject[] animalSponer;
    //���̃I�u�W�F�N�g
    [SerializeField] private GameObject cowObject;
    //�l�Y�~�̃I�u�W�F�N�g
    [SerializeField] private GameObject mouseObject;
    //�t�B�[�o�[�p�̋��̃I�u�W�F�N�g
    [SerializeField] private GameObject feverCowObject;
    //�t�B�[�o�[�p�̃l�Y�~�̃I�u�W�F�N�g
    [SerializeField] private  GameObject feverMouseObject;
    #endregion

    #region//�ϐ��֌W
    //�O��̃X�|�i�[�̔ԍ�
    private int oldSponerNumber = 0;
    //�t�B�[�o�[���ɌĂяo���������������ɐ�������t���O
    private bool sponeAnimalCow = false;
    //�t�B�[�o�[���ɌĂяo���������l�Y�~�����ɐ�������t���O
    private bool sponeAnimalMouse = false;
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
    //TimeManager
    [SerializeField] private TimeManager tm;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //�ŏ��̔z�u
    public void SetAnimals()
    {
        //�ŏ��̔z�u
        for (int count = 0; count < animalSponer.Length; count++)
        {
            //�Ăяo��������ݒ肷��
            GameObject selectAnimal = SelectAnimal();
            //�z�u��̃X�|�i�[����I�΂ꂽ�������Ăяo��
            Instantiate(SelectAnimal(),
                animalSponer[count].transform.position,
                animalSponer[count].transform.rotation);
        }
    }

    //�o�͂��铮����ݒ肷��
    public GameObject SelectAnimal()
    {
        GameObject selectObject = null;
        int randNum = Random.Range(0, 2);
        int num = randNum % 2;
        //���̏o��
        if(num != 0) selectObject = cowObject;
        //
        if(num == 0) selectObject = mouseObject;
        //Debug.Log("�Ăяo���I�u�W�F�N�g" + selectObject);
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
            randNum = Random.Range(0, animalSponer.Length);
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
            animalSponer[randNum].transform.position,
            animalSponer[randNum].transform.rotation);
    }

    //�l�Y�~�p�̃t�B�[�o�[�^�C�����s��
    public void FeverMouse()
    {
        sponeAnimalMouse = true;
    }
    //�t�B�[�o�[�t���O��false�ɂ���B�i�l�Y�~�j
    public void FinishFeverMouse()
    {
        sponeAnimalMouse = false;
    }
    //���p�̃t�B�[�o�[�^�C�����s��
    public void FeverCow()
    {
        sponeAnimalCow = true;
    }
    //�t�B�[�o�[�t���O��false�ɂ���B�i���j
    public void FinishFeverCow()
    {
        sponeAnimalCow = false;
    }

    //
    public void SetStartFeverAnimals()
    {
        for(int count = 0; count < animalSponer.Length; count++)
        {
            //�z�u��̃X�|�i�[����I�΂ꂽ�������Ăяo��(�l�Y�~�p)
            if (sponeAnimalMouse)
            {

                Instantiate(feverMouseObject,
                    animalSponer[count].transform.position,
                    animalSponer[count].transform.rotation);
                //Debug.Log("�l�Y�~�̔z�u");
            }

            //�z�u��̃X�|�i�[����I�΂ꂽ�������Ăяo��(���p)
            if (sponeAnimalCow)
            {
                Instantiate(feverCowObject,
                    animalSponer[count].transform.position,
                    animalSponer[count].transform.rotation);
                //Debug.Log("���̔z�u");
            }
        }
    }

    //�t�B�[�o�[�p�̓����Ăяo��
    public void SelectSponeFeverAnimal()
    {
        //���p�̏o��
        if(sponeAnimalCow)
        {
            FeverSponeAnimal(feverCowObject);
            //Debug.Log("cow");
        }

        //�l�Y�~�p�̏o��
        if(sponeAnimalMouse)
        {
            FeverSponeAnimal(feverMouseObject);
            //Debug.Log("mouse");
        }
    }

    //�t�B�[�o�[�p�̓����̏o��
    private void FeverSponeAnimal(GameObject animal)
    {
        //���[�J���ϐ������
        int randNum = 0;
        //�z�u��̐ݒ�
        while (true)
        {
            randNum = Random.Range(0, animalSponer.Length);
            //�O��o�͂����X�|�i�[�ƈقȂ�Ȃ�
            //�V�����Ăяo���X�|�i�[�����肷��
            if (randNum != oldSponerNumber)
            {
                break;
            }
        }

        //�X�|�i�[��݂��A�I�΂ꂽ�������Ăяo��
        //���̍ۂɃX�|�i�[�������Ă����]�Ɠ����l�𓮕��ɗ^����
        Instantiate(animal,
            animalSponer[randNum].transform.position,
            animalSponer[randNum].transform.rotation);
    }
}
