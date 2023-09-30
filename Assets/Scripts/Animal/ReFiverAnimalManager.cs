using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManeger;

public class ReFiverAnimalManager : MonoBehaviour
{
    #region//オブジェクト関係
    //スポナー
    [SerializeField] private GameObject[] feverAnimalSponer;
    //牛のオブジェクト
    [SerializeField] private GameObject feverCowObject;
    //ネズミのオブジェクト
    [SerializeField] private GameObject feverMouseObject;
    #endregion

    #region//変数関係
    //前回のスポナーの番号
    private int oldSponerNumber = 0;
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
    #endregion

    public static AnimalManager instance;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetAnimals()
    {
        //最初の配置
        for (int count = 0; count < feverAnimalSponer.Length; count++)
        {
            //呼び出す動物を設定する
            GameObject selectAnimal = SelectAnimal();
            //配置先のスポナーから選ばれた動物を呼び出す
            Instantiate(SelectAnimal(),
                feverAnimalSponer[count].transform.position,
                feverAnimalSponer[count].transform.rotation);
        }
    }

    public GameObject SelectAnimal()
    {
        GameObject selectObject = null;
        int randNum = Random.Range(0, 2);
        int num = randNum % 2;
        //牛の出力
        if (num != 0) selectObject = feverCowObject;
        //
        if (num == 0) selectObject = feverMouseObject;
        Debug.Log("呼び出すオブジェクト" + selectObject);
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
            randNum = Random.Range(0, feverAnimalSponer.Length - 1);
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
            feverAnimalSponer[randNum].transform.position,
            feverAnimalSponer[randNum].transform.rotation);
    }
}
