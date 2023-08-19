using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    [Header("呼び出すオブジェクト")][SerializeField]
    private GameObject[] sponerObject;
    //格納している動物達
    private Queue<GameObject> animals;
    //プレイヤーがゲットした動物
    private Queue<GameObject> getAnimals;
    //牛のオブジェクト
    [Header("牛")][SerializeField]
    private GameObject cow;
    [Header("ネズミ")][SerializeField]
    private GameObject mouse;
    //最大待機時間
    [Header("最大待機時間")][SerializeField] 
    private int maxWaitTime;
    //最小待機時間
    [Header("最小待機時間")][SerializeField]
    private int minWaitTime;
    //次のスポーン位置
    private int nextPos;

    //[SerializeField] private PlayerPalmate pp;

    public Queue<GameObject> Animals
    {
        get { return this.animals;}
        set { this.animals = value;}
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
        animals = new Queue<GameObject>();
        getAnimals = new Queue<GameObject>();
        nextPos = 0;
        //動物を格納
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
        //動物を各スポナーに出現させる
        for (int count = 0; count < sponerObject.Length; count++)
        {
            //出現する位置と進行方向を与える
            GameObject sponeAnimal = animals.Dequeue();
            sponeAnimal.transform.position = sponerObject[count].transform.position;
            sponeAnimal.transform.rotation = sponerObject[count].transform.rotation;
        }
    }

    //配置する動物の格納順の更新
    public void SponeAnimal(GameObject animal)
    {
        getAnimals.Enqueue(animal);
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
            //動物が格納されていない場所を全探索する
            //if (sponerObject[count] == null)          
            //出現する位置と進行方向を与える
            if (nextPos < sponerObject.Length)
            {
                nextPos++;
            }
            if (nextPos >= sponerObject.Length)
            {
                nextPos = 0;
            }
            

            GameObject sponeAnimal = getAnimals.Dequeue();           
            sponeAnimal.transform.position = sponerObject[nextPos].transform.position;
            sponeAnimal.transform.rotation = sponerObject[nextPos].transform.rotation;
                      
            animals.Enqueue(sponeAnimal);
            //待機時間を設定
            int waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
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
