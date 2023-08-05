using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    [Header("�Ăяo���I�u�W�F�N�g")][SerializeField]
    private GameObject[] sponerObject;
    //�i�[���Ă��铮���B
    private Queue<GameObject> animals;
    //���̃I�u�W�F�N�g
    [Header("��")][SerializeField]
    private GameObject cow;
    [Header("�l�Y�~")][SerializeField]
    private GameObject mouse;
    //�ő�ҋ@����
    [Header("�ő�ҋ@����")][SerializeField] 
    private int maxWaitTime;
    //�ŏ��ҋ@����
    [Header("�ŏ��ҋ@����")][SerializeField]
    private int minWaitTime;

    // Start is called before the first frame update
    void Start()
    {
        //������
        sponerObject = new GameObject[10];
        animals = new Queue<GameObject>();
        //�������i�[
        for(int count = 0; count < 30; count++)
        {
            if(count == 0 ||count % 2 == 0)
            {
                animals.Enqueue(mouse);
            }
            else
            {
                animals.Enqueue(cow);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetAnimals()
    {
        //�������e�X�|�i�[�ɏo��������
        for (int count = 0; count < sponerObject.Length; count++)
        {
            //�o������ʒu�Ɛi�s������^����
            GameObject sponeAnimal = animals.Dequeue();
            sponeAnimal.transform.position = sponerObject[count].transform.position;
            sponeAnimal.transform.rotation = sponerObject[count].transform.rotation;
        }
    }

    //�z�u���铮���̊i�[���̍X�V
    public void SponeAnimal(GameObject animal)
    {
        animals.Enqueue(animal);
        StartCoroutine(SponeAnimalActive());
    }

    //�J���[�擾���̔z�u�����̊i�[���̍X�V
    public void PowerUpSponeAnimal()
    {
        //while()
        
    }

    //�ʏ�̓����̐ݒu����
    private IEnumerator SponeAnimalActive()
    {
        //�ҋ@���Ԃ�ݒ�
        int waitTime = Random.Range(minWaitTime,maxWaitTime);
        yield return new WaitForSeconds(waitTime);
        //�ݒu����
        for (int count = 0; count < sponerObject.Length; count++)
        {
            //�������i�[����Ă��Ȃ��ꏊ��S�T������
            if (sponerObject[count] == null)
            {
                //�o������ʒu�Ɛi�s������^����
                GameObject sponeAnimal = animals.Dequeue();
                sponeAnimal.transform.position = sponerObject[count].transform.position;
                sponeAnimal.transform.rotation = sponerObject[count].transform.rotation;
            }
        }
    }

    //�J���[�擾���̓����̐ݒu����
    private IEnumerator PowerUpSponeAnimalAvtive()
    {
        //�ݒu����
        for (int count = 0; count < sponerObject.Length; count++)
        {
            //�������i�[����Ă��Ȃ��ꏊ��S�T������
            if (sponerObject[count] == null)
            {
                //�o������ʒu�Ɛi�s������^����
                GameObject sponeAnimal = animals.Dequeue();
                sponeAnimal.transform.position = sponerObject[count].transform.position;
                sponeAnimal.transform.rotation = sponerObject[count].transform.rotation;
            }
        }
        yield return null;
    }
}
