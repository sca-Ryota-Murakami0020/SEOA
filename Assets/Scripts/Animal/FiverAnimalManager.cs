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
    //フィーバー時用のスポナー
    [SerializeField] private GameObject[] fiverSponer;
    //フィーバー用のオブジェクトの初期位置
    [SerializeField] private GameObject resetPos;
    //牛のオブジェクト
    [SerializeField] private GameObject[] cowObject;
    //ネズミのオブジェクト
    [SerializeField] private GameObject[] mouseObject;
    //フィーバー用の牛のリスト
    private Queue<GameObject> fiverCowIndex = new Queue<GameObject>();
    //フィーバー用のネズミのリスト
    private Queue<GameObject> fiverMouseIndex = new Queue<GameObject>();

    //現在呼び出しているスポナーの番号
    private int sponerNumber = 0;
    //待機時間
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
        //初期化
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
            Debug.Log("参照中");
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
                Debug.Log("牛ん");
            }

            if (isMouse)
            {
                //SponeMouse(fiverMouseIndex.Dequeue());
                Debug.Log("なずみ");
            }
        }

        Debug.Log("フィーバ" + doingFiver);
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
        //Debug.Log("フィーバー入った");
        doingFiver = true;
    }

    public void NoActiveFiver()
    {
        doingFiver = false;
    }

    public void SetCowIndex()
    {
        //動物を各スポナーに出現させる
        for (int count = 0; count < fiverSponer.Length; count++)
        {
            //出現する位置と進行方向を与える
            GameObject sponeAnimal = fiverCowIndex.Dequeue();
            sponeAnimal.transform.position = fiverSponer[count].transform.position;
            sponeAnimal.transform.rotation = fiverSponer[count].transform.rotation;
            //AnimalController an = sponeAnimal.GetComponent<AnimalController>();
            FiverAnimalC fc = sponeAnimal.GetComponent<FiverAnimalC>();
            fc.ResetPar();
        }
        Debug.Log("牛設置");
    }

    public void SetMouseIndex()
    {
        //動物を各スポナーに出現させる
        for (int count = 0; count < fiverSponer.Length; count++)
        {
            //出現する位置と進行方向を与える
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
        //獲得リストの末端に獲得した動物を入れる
        StartCoroutine(SponeCowActive(returnCow));
    }

    public void SponeMouse(GameObject returnMouse)
    {
        FiverAnimalC fc = returnMouse.gameObject.GetComponent<FiverAnimalC>();
        returnMouse.transform.position = resetPos.transform.position;
        fc.StopAnimal();
        //獲得リストの末端に獲得した動物を入れる
        StartCoroutine(SponeMouseActive(returnMouse));
    }

    //牛用の配置変更
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

    //ネズミ用の配置変更
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
        //捕まえた動物をリストの末端に追加する
        fiverCowIndex.Enqueue(animal);

        //ここで次の出現する位置と進行方向を与え、スポナーに呼び出す動物を設置する
        //Debug.Log(animalIndex.Count);
        GameObject sponeAnimal = fiverCowIndex.Dequeue(); //ここは元々getAnimalsにしていたが、よく考えてみたらanimalsにしないと正しく参照する訳がないので修正した
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

        //待機時間を設定
        yield return new WaitForSeconds(waitTime);
        //ここで動物が動くようにする
        fc.SelectFlag = true;
        fc.ResetPar();
        pp.ChainCount = 0;
    }

    private IEnumerator SponeMouseActive(GameObject animal)
    {
        //捕まえた動物をリストの末端に追加する
        fiverMouseIndex.Enqueue(animal);

        //ここで次の出現する位置と進行方向を与え、スポナーに呼び出す動物を設置する
        //Debug.Log(animalIndex.Count);
        GameObject sponeAnimal = fiverMouseIndex.Dequeue(); //ここは元々getAnimalsにしていたが、よく考えてみたらanimalsにしないと正しく参照する訳がないので修正した
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

        //待機時間を設定
        yield return new WaitForSeconds(waitTime);
        //ここで動物が動くようにする
        fc.SelectFlag = true;
        fc.ResetPar();
        pp.ChainCount = 0;
    }
}
