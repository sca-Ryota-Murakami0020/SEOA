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
        //初期化
        fiverCowIndex = new Queue<GameObject>();
        fiverMouseIndex = new Queue<GameObject>();

        for (int count = 0; count < cowObject.Length; count++)
        {
            //オブジェクトを初期位置に移動させる
            cowObject[count].transform.position = resetPos.transform.position;
            //非表示にする
            cowObject[count].SetActive(false);
            //リストに投入する
            fiverCowIndex.Enqueue(cowObject[count]);
        }

        for (int count = 0; count < mouseObject.Length; count++)
        {
            //オブジェクトを初期位置に移動させる
            mouseObject[count].transform.position = resetPos.transform.position;
            //非表示にする
            mouseObject[count].SetActive(false);
            //リストに投入する
            fiverMouseIndex.Enqueue(mouseObject[count]);
            //Debug.Log("参照中");
        }
    }

    // Update is called once per frame
    void Update()
    {       
        Debug.Log("フィーバーフラグ：" + doingFiver);
    }

    //フィーバー状態にする
    public void ActiveFiver()
    {
        //Debug.Log("フィーバー入った");
        doingFiver = true;
    }

    //フィーバー状態を終了する
    public void NoActiveFiver()
    {
        doingFiver = false;
    }

    //動物達をスポナーに移動させる関数
    private void SetFiverAimals(GameObject setAnimal)
    {
        //座標変更
        setAnimal.transform.position = fiverSponer[sponerNumber].transform.position;
        //角度の変更
        setAnimal.transform.rotation = fiverSponer[sponerNumber].transform.rotation;
        //次に呼び出されるスポナーの位置を設定する
        //もしスポナーの数と同じ数値なら0に戻す。そうでなければ1を加算する
        sponerNumber = fiverSponer.Length >= sponerNumber ? sponerNumber = 0 : sponerNumber++;
    }

    //動物の位置を初期位置に戻す
    private void ResetPositionFiverAnimals(GameObject setAnimal)
    {
        //初期位置に移動する
        setAnimal.transform.position = resetPos.transform.position;
    }

    #region//牛用の処理
    //牛世のフィーバーフラグを立たせる
    public void CowFiver()
    {
        isCow = true;
    }

    //フィーバー直後に牛を設置する
    public void SetCowIndex()
    {
        //動物を各スポナーに出現させる
        for (int count = 0; count < fiverSponer.Length; count++)
        {
            //リストの頭を変数化
            GameObject sponeAnimal = fiverCowIndex.Dequeue();
            //非表示状態なので表示する
            sponeAnimal.SetActive(true);
            //出現する位置と進行方向を与える
            sponeAnimal.transform.position = fiverSponer[count].transform.position;
            sponeAnimal.transform.rotation = fiverSponer[count].transform.rotation;
            //呼び出したオブジェクトのスクリプトを参照する
            FiverAnimalC fc = sponeAnimal.GetComponent<FiverAnimalC>();
            //ステータスのリセット
            fc.ResetPar();
        }
        Debug.Log("牛設置");
    }

    //プレイヤーと帰ってきた動物から呼び出されるコルーチン起動用の関数
    public void FiverSponeCow(GameObject returnCow)
    {
        StartCoroutine(SponeCowActive(returnCow));
    }

    //牛の配置を行う
    private IEnumerator SponeCowActive(GameObject animal)
    {
        //捕まえた動物の位置を変更する
        ResetPositionFiverAnimals(animal);
        //関数を参照
        FiverAnimalC cowC = animal.gameObject.GetComponent<FiverAnimalC>();
        //ステータスのリセット
        cowC.ResetPar();
        //捕まえた動物をリストの末端に追加する
        fiverCowIndex.Enqueue(animal);
        //ここで次の出現する位置と進行方向を与え、スポナーに呼び出す動物を設置する
        GameObject sponeAnimal = fiverCowIndex.Dequeue();
        //スポーンの位置まで移動・回転
        SetFiverAimals(sponeAnimal);
        //関数の取得
        FiverAnimalC fc = sponeAnimal.GetComponent<FiverAnimalC>();
        //待機時間を設定
        yield return new WaitForSeconds(waitTime);
        //ここで動物が動くようにする
        fc.ResetPar();
    }

    //帰ってきた動物たちを元のリストに戻し、頭の動物を呼び出す
    private IEnumerator ReturnSponeCow(GameObject returnCow)
    {
        //帰ってきた牛を元のリストに戻す
        fiverCowIndex.Enqueue(returnCow);
        yield return null;
    }
    #endregion

    #region//ネズミ用の処理
    //ネズミフィーバー用のフラグを立たせる
    public void MouseFiver()
    {
        isMouse = true;
    }

    //フィーバー開始の初期設置
    public void SetMouseIndex()
    {
        //動物を各スポナーに出現させる
        for (int count = 0; count < fiverSponer.Length; count++)
        {
            //リストの頭を変数化
            GameObject sponeAnimal = fiverMouseIndex.Dequeue();
            //非表示状態なので表示にする
            sponeAnimal.SetActive(true);
            //出現する位置と進行方向を与える
            sponeAnimal.transform.position = fiverSponer[count].transform.position;
            sponeAnimal.transform.rotation = fiverSponer[count].transform.rotation;
            //呼び出したオブジェクトのスクリプトを参照する
            FiverAnimalC fc = sponeAnimal.GetComponent<FiverAnimalC>();
            //ステータスのリセット
            fc.ResetPar();
        }
        Debug.Log("nezumisetti");
    }

    //
    public void FiverSponeMouse(GameObject returnMouse)
    {
        FiverAnimalC fc = returnMouse.gameObject.GetComponent<FiverAnimalC>();

        //獲得リストの末端に獲得した動物を入れる
        StartCoroutine(SponeMouseActive(returnMouse));
    }

    private IEnumerator SponeMouseActive(GameObject animal)
    {
        //座標を初期化
        ResetPositionFiverAnimals(animal);
        //関数を参照
        FiverAnimalC mouseC = animal.gameObject.GetComponent<FiverAnimalC>();
        //ステータスのリセット
        mouseC.ResetPar();
        //捕まえた動物をリストの末端に追加する
        fiverMouseIndex.Enqueue(animal);
        //ここで次の出現する位置と進行方向を与え、スポナーに呼び出す動物を設置する
        GameObject sponeAnimal = fiverMouseIndex.Dequeue(); //ここは元々getAnimalsにしていたが、よく考えてみたらanimalsにしないと正しく参照する訳がないので修正した
        //関数の参照
        FiverAnimalC fc = sponeAnimal.GetComponent<FiverAnimalC>();
        //動物の設置
        SetFiverAimals(sponeAnimal);
        //待機時間を設定
        yield return new WaitForSeconds(waitTime);
        //ここで動物が動くようにする
        fc.ResetPar();
    }
    #endregion
}
