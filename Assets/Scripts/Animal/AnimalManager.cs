using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
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
}
