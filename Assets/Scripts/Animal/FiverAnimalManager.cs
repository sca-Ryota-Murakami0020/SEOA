using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManeger;

public class FiverAnimalManager : MonoBehaviour
{
    //ActiveManager
    [SerializeField] private ActiveManager activeManager;
    //
    [SerializeField] private PlayerPalmate pp;
    //
    [SerializeField] private TimeManager tm;
    //�t�B�[�o�[���p�̃X�|�i�[
    [SerializeField] private GameObject[] fiverSponer;
    //�t�B�[�o�[�p�̃I�u�W�F�N�g�̏����ʒu
    [SerializeField] private GameObject resetPos;
    //���̃I�u�W�F�N�g
    [SerializeField] private GameObject[] cowObject;
    //�l�Y�~�̃I�u�W�F�N�g
    [SerializeField] private GameObject[] mouseObject;
    //�t�B�[�o�[�p�̋��̃��X�g
    private Queue<GameObject> fiverCowIndex = new Queue<GameObject>();
    //�t�B�[�o�[�p�̃l�Y�~�̃��X�g
    private Queue<GameObject> fiverMouseIndex = new Queue<GameObject>();

    //���݌Ăяo���Ă���X�|�i�[�̔ԍ�
    private int sponerNumber = 0;
    //�ҋ@����
    [SerializeField] private float waitTime;
    //
    private bool isCow = false;
    //
    private bool isMouse = false;
    //
    private bool doingFiver = false;

    // Start is called before the first frame update
    void Start()
    {
        activeManager.NoActiveFiverCow();
        activeManager.NoActiveFiverMouse();
        //������
        fiverCowIndex = new Queue<GameObject>();
        fiverMouseIndex = new Queue<GameObject>();

        for (int count = 0; count < cowObject.Length; count++)
        {
            cowObject[count].transform.position = resetPos.transform.position;
            fiverCowIndex.Enqueue(cowObject[count]);
        }

        for (int count = 0; count < mouseObject.Length; count++)
        {
            mouseObject[count].transform.position = resetPos.transform.position;
            fiverMouseIndex.Enqueue(mouseObject[count]);
            Debug.Log("�Q�ƒ�");
        }
    }

    // Update is called once per frame
    void Update()
    {       
        if(doingFiver)
        {
            if (isCow)
            {
                //SponeCow(fiverCowIndex.Dequeue());
                Debug.Log("����");
            }

            if (isMouse)
            {
                //SponeMouse(fiverMouseIndex.Dequeue());
                Debug.Log("�Ȃ���");
            }
        }

        Debug.Log("�t�B�[�o" + doingFiver);
    }

    public void CowFiver()
    {
        //
        isCow = true;
        Debug.Log("uuuuu" + isCow);
    }

    public void MouseFiver()
    {
        //Debug.Log("usiyou");
        isMouse = true;
        Debug.Log("iiiiiii" + isMouse);
    }

    public void ActiveFiver()
    {
        //Debug.Log("�t�B�[�o�[������");
        doingFiver = true;
    }

    public void NoActiveFiver()
    {
        doingFiver = false;
    }

    public void SetCowIndex()
    {
        //�������e�X�|�i�[�ɏo��������
        for (int count = 0; count < fiverSponer.Length; count++)
        {
            //�o������ʒu�Ɛi�s������^����
            GameObject sponeAnimal = fiverCowIndex.Dequeue();
            sponeAnimal.transform.position = fiverSponer[count].transform.position;
            sponeAnimal.transform.rotation = fiverSponer[count].transform.rotation;
            //AnimalController an = sponeAnimal.GetComponent<AnimalController>();
            FiverAnimalC fc = sponeAnimal.GetComponent<FiverAnimalC>();
            fc.ResetPar();
        }
        Debug.Log("���ݒu");
    }

    public void SetMouseIndex()
    {
        //�������e�X�|�i�[�ɏo��������
        for (int count = 0; count < fiverSponer.Length; count++)
        {
            //�o������ʒu�Ɛi�s������^����
            GameObject sponeAnimal = fiverMouseIndex.Dequeue();
            sponeAnimal.transform.position = fiverSponer[count].transform.position;
            sponeAnimal.transform.rotation = fiverSponer[count].transform.rotation;
            //AnimalController an = sponeAnimal.GetComponent<AnimalController>();
            FiverAnimalC fc = sponeAnimal.GetComponent<FiverAnimalC>();
            fc.ResetPar();
        }
        Debug.Log("nezumisetti");
    }

    public void SponeCow(GameObject returnCow)
    {
        //getAnimals.Enqueue(animal);
        FiverAnimalC fc = returnCow.gameObject.GetComponent<FiverAnimalC>();
        returnCow.transform.position = resetPos.transform.position;
        fc.StopAnimal();
        //�l�����X�g�̖��[�Ɋl����������������
        StartCoroutine(SponeCowActive(returnCow));
    }

    public void SponeMouse(GameObject returnMouse)
    {
        FiverAnimalC fc = returnMouse.gameObject.GetComponent<FiverAnimalC>();
        returnMouse.transform.position = resetPos.transform.position;
        fc.StopAnimal();
        //�l�����X�g�̖��[�Ɋl����������������
        StartCoroutine(SponeMouseActive(returnMouse));
    }

    //���p�̔z�u�ύX
    public void RuteruListCow(GameObject backCow)
    {
        GameObject sponerCow = fiverCowIndex.Dequeue();
        //
        sponerCow.transform.position = fiverSponer[sponerNumber].transform.position;
        if(sponerNumber >= fiverSponer.Length)
        {
            sponerNumber = 0;
        }
        else
        {
            sponerNumber++;
        }
        fiverCowIndex.Enqueue(backCow);
    }

    //�l�Y�~�p�̔z�u�ύX
    public void ReturnListMouse(GameObject backMouse)
    {
        GameObject sponerMouse = fiverMouseIndex.Dequeue();
        //
        sponerMouse.transform.position = fiverSponer[sponerNumber].transform.position;
        if (sponerNumber >= fiverSponer.Length)
        {
            sponerNumber = 0;
        }
        else
        {
            sponerNumber++;
        }
        fiverMouseIndex.Enqueue(backMouse);
    }

    private IEnumerator SponeCowActive(GameObject animal)
    {
        //�߂܂������������X�g�̖��[�ɒǉ�����
        fiverCowIndex.Enqueue(animal);

        //�����Ŏ��̏o������ʒu�Ɛi�s������^���A�X�|�i�[�ɌĂяo��������ݒu����
        //Debug.Log(animalIndex.Count);
        GameObject sponeAnimal = fiverCowIndex.Dequeue(); //�����͌��XgetAnimals�ɂ��Ă������A�悭�l���Ă݂���animals�ɂ��Ȃ��Ɛ������Q�Ƃ���󂪂Ȃ��̂ŏC������
        //AnimalController ac = sponeAnimal.GetComponent<AnimalController>();
        FiverAnimalC fc = sponeAnimal.GetComponent<FiverAnimalC>();
        sponeAnimal.transform.position = fiverSponer[sponerNumber].transform.position;
        sponeAnimal.transform.rotation = fiverSponer[sponerNumber].transform.rotation;
        if (sponerNumber >= fiverSponer.Length)
        {
            sponerNumber = 0;
        }
        else
        {
            sponerNumber++;
        }

        //�ҋ@���Ԃ�ݒ�
        yield return new WaitForSeconds(waitTime);
        //�����œ����������悤�ɂ���
        fc.SelectFlag = true;
        fc.ResetPar();
        pp.ChainCount = 0;
    }

    private IEnumerator SponeMouseActive(GameObject animal)
    {
        //�߂܂������������X�g�̖��[�ɒǉ�����
        fiverMouseIndex.Enqueue(animal);

        //�����Ŏ��̏o������ʒu�Ɛi�s������^���A�X�|�i�[�ɌĂяo��������ݒu����
        //Debug.Log(animalIndex.Count);
        GameObject sponeAnimal = fiverMouseIndex.Dequeue(); //�����͌��XgetAnimals�ɂ��Ă������A�悭�l���Ă݂���animals�ɂ��Ȃ��Ɛ������Q�Ƃ���󂪂Ȃ��̂ŏC������
        //AnimalController ac = sponeAnimal.GetComponent<AnimalController>();
        FiverAnimalC fc = sponeAnimal.GetComponent<FiverAnimalC>();
        sponeAnimal.transform.position = fiverSponer[sponerNumber].transform.position;
        sponeAnimal.transform.rotation = fiverSponer[sponerNumber].transform.rotation;

        if (sponerNumber >= fiverSponer.Length)
        {
            sponerNumber = 0;
        }
        else
        {
            sponerNumber++;
        }

        //�ҋ@���Ԃ�ݒ�
        yield return new WaitForSeconds(waitTime);
        //�����œ����������悤�ɂ���
        fc.SelectFlag = true;
        fc.ResetPar();
        pp.ChainCount = 0;
    }
}
