using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManager;

public class AnimalManager : MonoBehaviour
{
    /*
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
    */

    #region//オブジェクト関係
    //スポナー
    [SerializeField] private GameObject[] animalSponer;
    //牛のオブジェクト
    [SerializeField] private GameObject cowObject;
    //ネズミのオブジェクト
    [SerializeField] private GameObject mouseObject;
    //フィーバー用の牛のオブジェクト
    [SerializeField] private GameObject feverCowObject;
    //フィーバー用のネズミのオブジェクト
    [SerializeField] private  GameObject feverMouseObject;
    #endregion

    #region//変数関係
    //前回のスポナーの番号
    private int oldSponerNumber = 0;
    //フィーバー中に呼び出す動物を牛だけに制限するフラグ
    private bool sponeAnimalCow = false;
    //フィーバー中に呼び出す動物をネズミだけに制限するフラグ
    private bool sponeAnimalMouse = false;
    //最大待機時間
    [Header("最大待機時間")]
    [SerializeField]
    private int maxWaitTime;
    //最小待機時間
    [Header("最小待機時間")]
    [SerializeField]
    private int minWaitTime;
    //PlayerPalmeta
    [SerializeField] private PlayerPalmate pp;
    //TimeManager
    [SerializeField] private TimeManager tm;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //最初の配置
    public void SetAnimals()
    {
        //最初の配置
        for (int count = 0; count < animalSponer.Length; count++)
        {
            //呼び出す動物を設定する
            GameObject selectAnimal = SelectAnimal();
            //配置先のスポナーから選ばれた動物を呼び出す
            Instantiate(SelectAnimal(),
                animalSponer[count].transform.position,
                animalSponer[count].transform.rotation);
        }
    }

    //出力する動物を設定する
    public GameObject SelectAnimal()
    {
        GameObject selectObject = null;
        int randNum = Random.Range(0, 2);
        int num = randNum % 2;
        //牛の出力
        if(num != 0) selectObject = cowObject;
        //
        if(num == 0) selectObject = mouseObject;
        //Debug.Log("呼び出すオブジェクト" + selectObject);
        return selectObject;
    }

    //動物をスポナーに呼び出す
    public void SponeAnimal()
    {
        //ローカル変数を作る
        int randNum = 0;
        //配置先の設定
        while (true)
        {
            randNum = Random.Range(0, animalSponer.Length);
            //前回出力したスポナーと異なるなら
            //新しく呼び出すスポナーを決定する
            if (randNum != oldSponerNumber)
            {
                break;
            }
        }

        //スポナーを設し、選ばれた動物を呼び出す
        //この際にスポナーが持っている回転と同じ値を動物に与える
        Instantiate(SelectAnimal(),
            animalSponer[randNum].transform.position,
            animalSponer[randNum].transform.rotation);
    }

    //ネズミ用のフィーバータイムを行う
    public void FeverMouse()
    {
        sponeAnimalMouse = true;
    }
    //フィーバーフラグをfalseにする。（ネズミ）
    public void FinishFeverMouse()
    {
        sponeAnimalMouse = false;
    }
    //牛用のフィーバータイムを行う
    public void FeverCow()
    {
        sponeAnimalCow = true;
    }
    //フィーバーフラグをfalseにする。（牛）
    public void FinishFeverCow()
    {
        sponeAnimalCow = false;
    }

    //
    public void SetStartFeverAnimals()
    {
        for(int count = 0; count < animalSponer.Length; count++)
        {
            //配置先のスポナーから選ばれた動物を呼び出す(ネズミ用)
            if (sponeAnimalMouse)
            {

                Instantiate(feverMouseObject,
                    animalSponer[count].transform.position,
                    animalSponer[count].transform.rotation);
                //Debug.Log("ネズミの配置");
            }

            //配置先のスポナーから選ばれた動物を呼び出す(牛用)
            if (sponeAnimalCow)
            {
                Instantiate(feverCowObject,
                    animalSponer[count].transform.position,
                    animalSponer[count].transform.rotation);
                //Debug.Log("牛の配置");
            }
        }
    }

    //フィーバー用の動物呼び出し
    public void SelectSponeFeverAnimal()
    {
        //牛用の出力
        if(sponeAnimalCow)
        {
            FeverSponeAnimal(feverCowObject);
            //Debug.Log("cow");
        }

        //ネズミ用の出力
        if(sponeAnimalMouse)
        {
            FeverSponeAnimal(feverMouseObject);
            //Debug.Log("mouse");
        }
    }

    //フィーバー用の動物の出力
    private void FeverSponeAnimal(GameObject animal)
    {
        //ローカル変数を作る
        int randNum = 0;
        //配置先の設定
        while (true)
        {
            randNum = Random.Range(0, animalSponer.Length);
            //前回出力したスポナーと異なるなら
            //新しく呼び出すスポナーを決定する
            if (randNum != oldSponerNumber)
            {
                break;
            }
        }

        //スポナーを設し、選ばれた動物を呼び出す
        //この際にスポナーが持っている回転と同じ値を動物に与える
        Instantiate(animal,
            animalSponer[randNum].transform.position,
            animalSponer[randNum].transform.rotation);
    }
}
