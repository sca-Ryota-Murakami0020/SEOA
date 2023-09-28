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
        //������
        fiverCowIndex = new Queue<GameObject>();
        fiverMouseIndex = new Queue<GameObject>();

        for (int count = 0; count < cowObject.Length; count++)
        {
            //�I�u�W�F�N�g�������ʒu�Ɉړ�������
            cowObject[count].transform.position = resetPos.transform.position;
            //��\���ɂ���
            cowObject[count].SetActive(false);
            //���X�g�ɓ�������
            fiverCowIndex.Enqueue(cowObject[count]);
        }

        for (int count = 0; count < mouseObject.Length; count++)
        {
            //�I�u�W�F�N�g�������ʒu�Ɉړ�������
            mouseObject[count].transform.position = resetPos.transform.position;
            //��\���ɂ���
            mouseObject[count].SetActive(false);
            //���X�g�ɓ�������
            fiverMouseIndex.Enqueue(mouseObject[count]);
            //Debug.Log("�Q�ƒ�");
        }
    }

    // Update is called once per frame
    void Update()
    {       
        Debug.Log("�t�B�[�o�[�t���O�F" + doingFiver);
    }

    //�t�B�[�o�[��Ԃɂ���
    public void ActiveFiver()
    {
        //Debug.Log("�t�B�[�o�[������");
        doingFiver = true;
    }

    //�t�B�[�o�[��Ԃ��I������
    public void NoActiveFiver()
    {
        doingFiver = false;
    }

    //�����B���X�|�i�[�Ɉړ�������֐�
    private void SetFiverAimals(GameObject setAnimal)
    {
        //���W�ύX
        setAnimal.transform.position = fiverSponer[sponerNumber].transform.position;
        //�p�x�̕ύX
        setAnimal.transform.rotation = fiverSponer[sponerNumber].transform.rotation;
        //���ɌĂяo�����X�|�i�[�̈ʒu��ݒ肷��
        //�����X�|�i�[�̐��Ɠ������l�Ȃ�0�ɖ߂��B�����łȂ����1�����Z����
        sponerNumber = fiverSponer.Length >= sponerNumber ? sponerNumber = 0 : sponerNumber++;
    }

    //�����̈ʒu�������ʒu�ɖ߂�
    private void ResetPositionFiverAnimals(GameObject setAnimal)
    {
        //�����ʒu�Ɉړ�����
        setAnimal.transform.position = resetPos.transform.position;
    }

    #region//���p�̏���
    //�����̃t�B�[�o�[�t���O�𗧂�����
    public void CowFiver()
    {
        isCow = true;
    }

    //�t�B�[�o�[����ɋ���ݒu����
    public void SetCowIndex()
    {
        //�������e�X�|�i�[�ɏo��������
        for (int count = 0; count < fiverSponer.Length; count++)
        {
            //���X�g�̓���ϐ���
            GameObject sponeAnimal = fiverCowIndex.Dequeue();
            //��\����ԂȂ̂ŕ\������
            sponeAnimal.SetActive(true);
            //�o������ʒu�Ɛi�s������^����
            sponeAnimal.transform.position = fiverSponer[count].transform.position;
            sponeAnimal.transform.rotation = fiverSponer[count].transform.rotation;
            //�Ăяo�����I�u�W�F�N�g�̃X�N���v�g���Q�Ƃ���
            FiverAnimalC fc = sponeAnimal.GetComponent<FiverAnimalC>();
            //�X�e�[�^�X�̃��Z�b�g
            fc.ResetPar();
        }
        Debug.Log("���ݒu");
    }

    //�v���C���[�ƋA���Ă�����������Ăяo�����R���[�`���N���p�̊֐�
    public void FiverSponeCow(GameObject returnCow)
    {
        StartCoroutine(SponeCowActive(returnCow));
    }

    //���̔z�u���s��
    private IEnumerator SponeCowActive(GameObject animal)
    {
        //�߂܂��������̈ʒu��ύX����
        ResetPositionFiverAnimals(animal);
        //�֐����Q��
        FiverAnimalC cowC = animal.gameObject.GetComponent<FiverAnimalC>();
        //�X�e�[�^�X�̃��Z�b�g
        cowC.ResetPar();
        //�߂܂������������X�g�̖��[�ɒǉ�����
        fiverCowIndex.Enqueue(animal);
        //�����Ŏ��̏o������ʒu�Ɛi�s������^���A�X�|�i�[�ɌĂяo��������ݒu����
        GameObject sponeAnimal = fiverCowIndex.Dequeue();
        //�X�|�[���̈ʒu�܂ňړ��E��]
        SetFiverAimals(sponeAnimal);
        //�֐��̎擾
        FiverAnimalC fc = sponeAnimal.GetComponent<FiverAnimalC>();
        //�ҋ@���Ԃ�ݒ�
        yield return new WaitForSeconds(waitTime);
        //�����œ����������悤�ɂ���
        fc.ResetPar();
    }

    //�A���Ă����������������̃��X�g�ɖ߂��A���̓������Ăяo��
    private IEnumerator ReturnSponeCow(GameObject returnCow)
    {
        //�A���Ă����������̃��X�g�ɖ߂�
        fiverCowIndex.Enqueue(returnCow);
        yield return null;
    }
    #endregion

    #region//�l�Y�~�p�̏���
    //�l�Y�~�t�B�[�o�[�p�̃t���O�𗧂�����
    public void MouseFiver()
    {
        isMouse = true;
    }

    //�t�B�[�o�[�J�n�̏����ݒu
    public void SetMouseIndex()
    {
        //�������e�X�|�i�[�ɏo��������
        for (int count = 0; count < fiverSponer.Length; count++)
        {
            //���X�g�̓���ϐ���
            GameObject sponeAnimal = fiverMouseIndex.Dequeue();
            //��\����ԂȂ̂ŕ\���ɂ���
            sponeAnimal.SetActive(true);
            //�o������ʒu�Ɛi�s������^����
            sponeAnimal.transform.position = fiverSponer[count].transform.position;
            sponeAnimal.transform.rotation = fiverSponer[count].transform.rotation;
            //�Ăяo�����I�u�W�F�N�g�̃X�N���v�g���Q�Ƃ���
            FiverAnimalC fc = sponeAnimal.GetComponent<FiverAnimalC>();
            //�X�e�[�^�X�̃��Z�b�g
            fc.ResetPar();
        }
        Debug.Log("nezumisetti");
    }

    //
    public void FiverSponeMouse(GameObject returnMouse)
    {
        FiverAnimalC fc = returnMouse.gameObject.GetComponent<FiverAnimalC>();

        //�l�����X�g�̖��[�Ɋl����������������
        StartCoroutine(SponeMouseActive(returnMouse));
    }

    private IEnumerator SponeMouseActive(GameObject animal)
    {
        //���W��������
        ResetPositionFiverAnimals(animal);
        //�֐����Q��
        FiverAnimalC mouseC = animal.gameObject.GetComponent<FiverAnimalC>();
        //�X�e�[�^�X�̃��Z�b�g
        mouseC.ResetPar();
        //�߂܂������������X�g�̖��[�ɒǉ�����
        fiverMouseIndex.Enqueue(animal);
        //�����Ŏ��̏o������ʒu�Ɛi�s������^���A�X�|�i�[�ɌĂяo��������ݒu����
        GameObject sponeAnimal = fiverMouseIndex.Dequeue(); //�����͌��XgetAnimals�ɂ��Ă������A�悭�l���Ă݂���animals�ɂ��Ȃ��Ɛ������Q�Ƃ���󂪂Ȃ��̂ŏC������
        //�֐��̎Q��
        FiverAnimalC fc = sponeAnimal.GetComponent<FiverAnimalC>();
        //�����̐ݒu
        SetFiverAimals(sponeAnimal);
        //�ҋ@���Ԃ�ݒ�
        yield return new WaitForSeconds(waitTime);
        //�����œ����������悤�ɂ���
        fc.ResetPar();
    }
    #endregion
}
