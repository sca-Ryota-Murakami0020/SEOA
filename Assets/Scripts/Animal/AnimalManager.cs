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

    //スポナーのところに動物を配置する
    public void SetSponerAnimal(GameObject animal)
    {
        //ここで番地の設置を行う
        while (true)
        {
            int selectNumber = Random.Range(0, sponerObject.Length);

            if (oldSponerNumber != selectNumber)
            {
                oldSponerNumber = selectNumber;
                break;
            }
        }
        //位置の設定
        animal.transform.position = sponerObject[oldSponerNumber].transform.position;
        //回転の設定
        animal.transform.rotation = sponerObject[oldSponerNumber].transform.rotation;
        Debug.Log("呼び出し中");
    }

    //動物の位置を初期位置に戻す
    public void ResetPositionAnimals(GameObject animal)
    {
        //位置の変更
        animal.transform.position = this.transform.position;
        //向きの変更
        animal.transform.rotation = this.transform.rotation;
    }

    //配置する動物の格納順の更新
    public void SponeAnimal(GameObject animal)
    {       
        //関数を参照
        AnimalController an = animal.gameObject.GetComponent<AnimalController>();
        //位置と回転を初期化
        ResetPositionAnimals(animal);
        //動物のステータスを初期化
        an.StopAnimal();
        //animalIndex.Enqueue(animal);
        StartCoroutine(SponeAnimalActive(animal));
    }


    //捕まえた時に行う動物の処理
    //呼び出し元ー＞PlayerPalamate,AnimalController
    public void ReturnAnimal(GameObject setAnimal)
    {
        //座標の初期化
        ResetPositionAnimals(setAnimal);
        AnimalController an = setAnimal.gameObject.GetComponent<AnimalController>();
        //ここでリストに追加する動物の情報をリセットする
        an.StopAnimal();
    }

    //画面外に出た時に動物をリストに追加して、新しい動物を追加する
    //呼び出し元ー＞AnimalController
    public void BackAnimalList(GameObject backAnimal)
    {
        //座標の初期化
        ResetPositionAnimals(backAnimal);
        AnimalController an = backAnimal.gameObject.GetComponent<AnimalController>();
        //戻した動物を行動不可状態にする
        an.StopAnimal();
        //ここでリストに再投入する
        animalIndex.Enqueue(backAnimal);
        //配置コルーチンを呼び出す
        StartCoroutine(NormalSponeAnimal());
    }
    #endregion

    #region//コルーチン処理
    //通常の動物の設置処理
    private IEnumerator SponeAnimalActive(GameObject animal)
    {
        //捕まえた動物をリストの末端に追加する
        animalIndex.Enqueue(animal);
        //リストの頭を変数化
        GameObject sponeAnimal = animalIndex.Dequeue(); 
        //関数の取得
        AnimalController ac = sponeAnimal.GetComponent<AnimalController>();
        //ここで次の場所が決まるまで処理を行う
        SetSponerAnimal(sponeAnimal);
        //待機時間を設定
        int waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);
        //ここで動物が動くようにする
        ac.ResetPar();
    }

    //画面外に出た時に動物を呼び出す処理
    private IEnumerator NormalSponeAnimal()
    {
        //リストの頭を変数化
        GameObject sponeAnimal = animalIndex.Dequeue();
        //関数の取得
        AnimalController ac = sponeAnimal.GetComponent<AnimalController>();
        //設置処理
        SetSponerAnimal(sponeAnimal);
        //待機時間を設定
        int waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);
        //ここで動物が動くようにする
        ac.ResetPar();
    }
    #endregion
}
