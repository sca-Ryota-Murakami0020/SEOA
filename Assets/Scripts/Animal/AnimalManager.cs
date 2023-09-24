using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    #region//変数関係
    //前回のスポナーの番号
    private int oldSponerNumber;
    //最大待機時間
    [Header("最大待機時間")][SerializeField] 
    private int maxWaitTime;
    //最小待機時間
    [Header("最小待機時間")][SerializeField]
    private int minWaitTime;
    //PlayerPalmeta
    [SerializeField] private PlayerPalmate pp;
    #endregion

    #region//リスト関係
    [Header("呼び出すオブジェクト")]
    [SerializeField]
    private GameObject[] sponerObject;    
    //格納する動物達
    [SerializeField]
    private GameObject[] animals;
    //格納している動物達
    private Queue<GameObject> animalIndex = new Queue<GameObject>();
    //獲得した動物のリスト
    private Queue<GameObject> getAnimals = new Queue<GameObject>();

    #endregion

    //プロパティ
    public Queue<GameObject> Animals
    {
        get { return this.animalIndex;}
        set { this.animalIndex = value;}
    }

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        animalIndex = new Queue<GameObject>();
        oldSponerNumber = 0;
        for(int count = 0; count < animals.Length; count++)
        {
            animals[count].transform.position = this.transform.position;
            animals[count].transform.rotation = this.transform.rotation;
            animalIndex.Enqueue(animals[count]);
        }
    }

    // Update is called once per frame
    void Update()
    { 
    
    }

    #region//関数
    //動物の初期配置
    public void SetAnimals()
    {
        //動物を各スポナーに出現させる
        for (int count = 0; count < sponerObject.Length; count++)
        {
            //出現する位置と進行方向を与える
            GameObject sponeAnimal = animalIndex.Dequeue();
            sponeAnimal.transform.position = sponerObject[count].transform.position;
            sponeAnimal.transform.rotation = sponerObject[count].transform.rotation;
            AnimalController an = sponeAnimal.GetComponent<AnimalController>();
            an.ResetPar();
        }
    }

    //配置する動物の格納順の更新
    public void SponeAnimal(GameObject animal)
    {       
        AnimalController an = animal.gameObject.GetComponent<AnimalController>();
        animal.transform.position = this.transform.position;
        an.StopAnimal();
        //Debug.Log(animal);
        //獲得リストの末端に獲得した動物を入れる
        getAnimals.Enqueue(animal);
        //animalIndex.Enqueue(animal);
        StartCoroutine(SponeAnimalActive(animal));
    }

    /*
    public void ResetAnimalPar()
    {
        for(int count = 0; count < animalIndex.Count; count++)
        {
            GameObject ani = getAnimals.Dequeue();
            AnimalController an = ani.gameObject.GetComponent<AnimalController>();
            if(an.SelectFlag)
            {
                an.ResetPar();
                Debug.Log("関数呼び出し");
            }
        }
    }*/

    //捕まえた時に行う動物の処理
    public void ReturnAnimal(GameObject setAnimal)
    {
        //座標の初期化
        setAnimal.transform.position = this.transform.position;
        setAnimal.transform.rotation = this.transform.rotation;
        AnimalController an = setAnimal.gameObject.GetComponent<AnimalController>();
        //ここでリストに追加する動物の情報をリセットする
        an.StopAnimal();
    }

    //画面外に出た時に動物をリストに追加して、新しい動物を追加する
    public void BackAnimalList(GameObject backAnimal)
    {
        //座標の初期化
        backAnimal.transform.position = this.transform.position;
        backAnimal.transform.rotation = this.transform.rotation;
        AnimalController an = backAnimal.gameObject.GetComponent<AnimalController>();
        //戻した動物を行動不可状態にする
        an.StopAnimal();
        //配置コルーチンを呼び出す
        StartCoroutine(NormalSponeAnimal());
    }

    //カレー取得時の配置動物の格納順の更新
    public void PowerUpSponeAnimal()
    {
        //while()

    }
    #endregion

    #region//コルーチン処理
    //通常の動物の設置処理
    private IEnumerator SponeAnimalActive(GameObject animal)
    {
        //捕まえた動物をリストの末端に追加する
        animalIndex.Enqueue(animal);
        //ここで次の場所が決まるまで処理を行う
        while (true)
        {
            int selectNumber = Random.Range(0, sponerObject.Length);

            if (oldSponerNumber != selectNumber)
            {
                oldSponerNumber = selectNumber;
                break;
            }
        }

        //ここで次の出現する位置と進行方向を与え、スポナーに呼び出す動物を設置する
        //Debug.Log(animalIndex.Count);
        GameObject sponeAnimal = animalIndex.Dequeue(); //ここは元々getAnimalsにしていたが、よく考えてみたらanimalsにしないと正しく参照する訳がないので修正した
        AnimalController ac = sponeAnimal.GetComponent<AnimalController>();
        sponeAnimal.transform.position = sponerObject[oldSponerNumber].transform.position;
        sponeAnimal.transform.rotation = sponerObject[oldSponerNumber].transform.rotation;

        //待機時間を設定
        int waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);
        //ここで動物が動くようにする
        ac.SelectFlag = true;
        ac.ResetPar();
        pp.ChainCount = 0;
    }

    //画面外に出た時に動物を呼び出す処理
    private IEnumerator NormalSponeAnimal()
    {
        //設置処理
        //ここで次の場所が決まるまで処理を行う
        while (true)
        {
            int selectNumber = Random.Range(0, sponerObject.Length);

            if (oldSponerNumber != selectNumber)
            {
                oldSponerNumber = selectNumber;
                break;
            }
        }

        //ここで次の出現する位置と進行方向を与え、スポナーに呼び出す動物を設置する
        GameObject sponeAnimal = animalIndex.Dequeue(); //ここは元々getAnimalsにしていたが、よく考えてみたらanimalsにしないと正しく参照する訳がないので修正した
        AnimalController ac = sponeAnimal.GetComponent<AnimalController>();
        sponeAnimal.transform.position = sponerObject[oldSponerNumber].transform.position;
        sponeAnimal.transform.rotation = sponerObject[oldSponerNumber].transform.rotation;

        //待機時間を設定
        int waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);
        //ここで動物が動くようにする
        ac.SelectFlag = true;
        ac.ResetPar();
    }

    //カレー取得時の動物の設置処理
    /*
    private IEnumerator PowerUpSponeAnimalAvtive()
    {
        //設置処理
        for (int count = 0; count < sponerObject.Length; count++)
        {
            //動物が格納されていない場所を全探索する
            if (sponerObject[count] == null)
            {
                //出現する位置と進行方向を与える
                GameObject sponeAnimal = animals.Dequeue();
                sponeAnimal.transform.position = sponerObject[count].transform.position;
                sponeAnimal.transform.rotation = sponerObject[count].transform.rotation;
            }
        }
        yield return null;
    }*/
    #endregion
}
