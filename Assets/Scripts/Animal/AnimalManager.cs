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
    //�l�����������̃��X�g
    private Queue<GameObject> getAnimals = new Queue<GameObject>();

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
        //Debug.Log("�Ăяo��");
    }

    // Update is called once per frame
    void Update()
    { 
        //Debug.Log(animalIndex.Count);
    }

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
            an.SelectFlag = true;
            an.ResetPar();
            //getAnimals.Enqueue(sponeAnimal);
        }
    }

    //�z�u���铮���̊i�[���̍X�V
    public void SponeAnimal(GameObject animal)
    {       
        //getAnimals.Enqueue(animal);
        AnimalController an = animal.gameObject.GetComponent<AnimalController>();
        animal.transform.position = this.transform.position;
        //an.RenderFlag = false;
        an.CanGet = false;
        an.SelectFlag = false; 
        an.StopAnimal();
        //Debug.Log(animal);
        //�l�����X�g�̖��[�Ɋl����������������
        getAnimals.Enqueue(animal);
        //animalIndex.Enqueue(animal);
        StartCoroutine(SponeAnimalActive(animal));
    }

    //�J���[�擾���̔z�u�����̊i�[���̍X�V
    public void PowerUpSponeAnimal()
    {
        //while()
        
    }
 
    public void ResetEnemyPar()
    {
        for(int count = 0; count < animalIndex.Count; count++)
        {
            GameObject ani = getAnimals.Dequeue();
            AnimalController an = ani.gameObject.GetComponent<AnimalController>();
            if(an.SelectFlag)
            {
                an.ResetPar();
            }
        }
    }

    //�߂܂������ɍs�������̏���
    public void ReturnAnimal(GameObject setAnimal)
    {
        //���W�̏�����
        setAnimal.transform.position = this.transform.position;
        setAnimal.transform.rotation = this.transform.rotation;
        AnimalController an = setAnimal.gameObject.GetComponent<AnimalController>();
        //an.SelectFlag = false;
        //�����Ń��X�g�ɒǉ����铮���̏������Z�b�g����
        an.StopAnimal();
    }

    //��ʊO�ɏo�����ɓ��������X�g�ɒǉ����āA�V����������ǉ�����
    public void BackAnimalList(GameObject backAnimal)
    {
        //Debug.Log("�A�҂���");
        //���W�̏�����
        backAnimal.transform.position = this.transform.position;
        backAnimal.transform.rotation = this.transform.rotation;
        AnimalController an = backAnimal.gameObject.GetComponent<AnimalController>();
        //an.SelectFlag = false;
        an.StopAnimal();
        //Debug.Log("�Ăяo���J�n");
        StartCoroutine(NormalSponeAnimal());
    }

    //�ʏ�̓����̐ݒu����
    private IEnumerator SponeAnimalActive(GameObject animal)
    {
        Debug.Log(pp.ChainCount);
        animalIndex.Enqueue(animal);
        //�ݒu����
        for (int count = 0; count < pp.ChainCount; count++)
        {
            //�����Ŏ��̏ꏊ�����܂�܂ŏ������s��
            while (true)
            {
                int selectNumber = Random.Range(0, sponerObject.Length);

                if (oldSponerNumber != selectNumber)
                {
                    oldSponerNumber = selectNumber;
                    break;
                }
            }

            //�����Ŏ��̏o������ʒu�Ɛi�s������^���A�X�|�i�[�ɌĂяo��������ݒu����
            //Debug.Log(animalIndex.Count);
            GameObject sponeAnimal = animalIndex.Dequeue(); //�����͌��XgetAnimals�ɂ��Ă������A�悭�l���Ă݂���animals�ɂ��Ȃ��Ɛ������Q�Ƃ���󂪂Ȃ��̂ŏC������
            AnimalController ac = sponeAnimal.GetComponent<AnimalController>();
            sponeAnimal.transform.position = sponerObject[oldSponerNumber].transform.position;
            sponeAnimal.transform.rotation = sponerObject[oldSponerNumber].transform.rotation;
            ac.SelectFlag = true;

            //�ҋ@���Ԃ�ݒ�
            int waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
            //�����œ����������悤�ɂ���
            ac.ResetPar();
        }
        pp.ChainCount = 0;
    }

    //��ʊO�ɏo�����ɓ������Ăяo������
    private IEnumerator NormalSponeAnimal()
    {
        //Debug.Log("�ǂ�");
        //�ݒu����
        for (int count = 0; count < pp.ChainCount; count++)
        {
            //�����Ŏ��̏ꏊ�����܂�܂ŏ������s��
            while (true)
            {
                int selectNumber = Random.Range(0, sponerObject.Length);

                if (oldSponerNumber != selectNumber)
                {
                    oldSponerNumber = selectNumber;
                    break;
                }
            }

            //�����Ŏ��̏o������ʒu�Ɛi�s������^���A�X�|�i�[�ɌĂяo��������ݒu����
            GameObject sponeAnimal = animalIndex.Dequeue(); //�����͌��XgetAnimals�ɂ��Ă������A�悭�l���Ă݂���animals�ɂ��Ȃ��Ɛ������Q�Ƃ���󂪂Ȃ��̂ŏC������
            AnimalController ac = sponeAnimal.GetComponent<AnimalController>();
            sponeAnimal.transform.position = sponerObject[oldSponerNumber].transform.position;
            sponeAnimal.transform.rotation = sponerObject[oldSponerNumber].transform.rotation;
            ac.SelectFlag = true;

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
