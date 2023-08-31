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
    //���̃X�|�[���ʒu
    private int nextPos;
    #endregion

    #region//���X�g�֌W
    [Header("�Ăяo���I�u�W�F�N�g")]
    [SerializeField]
    private GameObject[] sponerObject;    
    //�i�[���铮���B
    [SerializeField]
    private GameObject[] animals;
    //�i�[���Ă��铮���B
    private Queue<GameObject> animalIndex;
    //�v���C���[���Q�b�g��������
    private Queue<GameObject> getAnimals;
    #endregion

    //�v���p�e�B
    public Queue<GameObject> Animals
    {
        get { return this.animalIndex;}
        set { this.animalIndex = value;}
    }

    public Queue<GameObject> GetAnimals
    {
        get { return this.getAnimals;}
        set { this.getAnimals = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        //������
        //sponerObject = new GameObject[10];
        animalIndex = new Queue<GameObject>();
        getAnimals = new Queue<GameObject>();
        nextPos = 0;
        oldSponerNumber = 0;
        for(int count = 0; count <= animals.Length; count++)
        {
            animalIndex.Enqueue(animals[count]);
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
            GameObject sponeAnimal = animalIndex.Dequeue();
            sponeAnimal.transform.position = sponerObject[count].transform.position;
            sponeAnimal.transform.rotation = sponerObject[count].transform.rotation;
        }
    }

    //�z�u���铮���̊i�[���̍X�V
    public void SponeAnimal(GameObject animal)
    {
        getAnimals.Enqueue(animal);
        AnimalController an = animal.gameObject.GetComponent<AnimalController>();
        an.CanGet = false;
        an.StopAnimal();
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
        //�ݒu����
        for (int count = 0; count < getAnimals.Count; count++)
        {
            //�����Ŏ��̏ꏊ�����܂�܂ŏ������s��
            while(true)
            {
                int selectNumber = Random.Range(0, sponerObject.Length);

                if (oldSponerNumber != selectNumber)
                {
                    oldSponerNumber = selectNumber;
                    break;
                }
            }
            //���X�g�̖��[�Ɋl����������������
            GameObject setAnimal = getAnimals.Dequeue();
            animalIndex.Enqueue(setAnimal);

            //�����Ŏ��̏o������ʒu�Ɛi�s������^���A�X�|�i�[�ɌĂяo��������ݒu����
            GameObject sponeAnimal = animalIndex.Dequeue(); //�����͌��XgetAnimals�ɂ��Ă������A�悭�l���Ă݂���animals�ɂ��Ȃ��Ɛ������Q�Ƃ���󂪂Ȃ��̂ŏC������
            AnimalController ac = sponeAnimal.GetComponent<AnimalController>();
            sponeAnimal.transform.position = sponerObject[oldSponerNumber].transform.position;
            sponeAnimal.transform.rotation = sponerObject[oldSponerNumber].transform.rotation;           
                      
            //�ҋ@���Ԃ�ݒ�
            int waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
            //�����œ����������悤�ɂ���
            ac.ResetPar();
        }
    }

    //�J���[�擾���̓����̐ݒu����
    /*
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
    }*/
}
