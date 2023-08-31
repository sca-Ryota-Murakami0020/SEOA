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
    //次のスポーン位置
    private int nextPos;
    #endregion

    #region//リスト関係
    [Header("呼び出すオブジェクト")]
    [SerializeField]
    private GameObject[] sponerObject;    
    //格納する動物達
    [SerializeField]
    private GameObject[] animals;
    //格納している動物達
    private Queue<GameObject> animalIndex;
    //プレイヤーがゲットした動物
    private Queue<GameObject> getAnimals;
    #endregion

    //プロパティ
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
        //初期化
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
        //動物を各スポナーに出現させる
        for (int count = 0; count < sponerObject.Length; count++)
        {
            //出現する位置と進行方向を与える
            GameObject sponeAnimal = animalIndex.Dequeue();
            sponeAnimal.transform.position = sponerObject[count].transform.position;
            sponeAnimal.transform.rotation = sponerObject[count].transform.rotation;
        }
    }

    //配置する動物の格納順の更新
    public void SponeAnimal(GameObject animal)
    {
        getAnimals.Enqueue(animal);
        AnimalController an = animal.gameObject.GetComponent<AnimalController>();
        an.CanGet = false;
        an.StopAnimal();
        StartCoroutine(SponeAnimalActive());
    }

    //カレー取得時の配置動物の格納順の更新
    public void PowerUpSponeAnimal()
    {
        //while()
        
    }

    //通常の動物の設置処理
    private IEnumerator SponeAnimalActive()
    {    
        //設置処理
        for (int count = 0; count < getAnimals.Count; count++)
        {
            //ここで次の場所が決まるまで処理を行う
            while(true)
            {
                int selectNumber = Random.Range(0, sponerObject.Length);

                if (oldSponerNumber != selectNumber)
                {
                    oldSponerNumber = selectNumber;
                    break;
                }
            }
            //リストの末端に獲得した動物を入れる
            GameObject setAnimal = getAnimals.Dequeue();
            animalIndex.Enqueue(setAnimal);

            //ここで次の出現する位置と進行方向を与え、スポナーに呼び出す動物を設置する
            GameObject sponeAnimal = animalIndex.Dequeue(); //ここは元々getAnimalsにしていたが、よく考えてみたらanimalsにしないと正しく参照する訳がないので修正した
            AnimalController ac = sponeAnimal.GetComponent<AnimalController>();
            sponeAnimal.transform.position = sponerObject[oldSponerNumber].transform.position;
            sponeAnimal.transform.rotation = sponerObject[oldSponerNumber].transform.rotation;           
                      
            //待機時間を設定
            int waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
            //ここで動物が動くようにする
            ac.ResetPar();
        }
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
}
