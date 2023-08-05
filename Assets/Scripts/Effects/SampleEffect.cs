using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleEffect : MonoBehaviour
{
    //エフェクト
    [Header("呼び出すエフェクト")][SerializeField] private PlayEffect[] effects;
    //現在のエフェクト
    private int effectCurrent = 0;
    //エフェクト再生する場所
    private Queue<Vector2> animalInfo = new Queue<Vector2>();

    //エフェクト再生中か
    private bool isEffect = false;


    private void Update()
    {
        if (isEffect) return; // エフェクト再生中は処理しない

        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            animalInfo.Enqueue(pos);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("StartEffect");
            StartCoroutine(ActiveEffect());
        }
    }
    private void Effect(Vector2 pos)
    {
        // エフェクト再生する(座標)
        effects[effectCurrent].Play(pos);
        // 次のエフェクト再生する準備
        effectCurrent++;
        if (effects.Length == effectCurrent) effectCurrent = 0;
    }

    private IEnumerator ActiveEffect()
    {
        isEffect = true;  
        while (animalInfo.Count != 0)
        {
            //エフェクト再生
            Effect(animalInfo.Dequeue());
            yield return new WaitForSeconds(0.1f);
        }
        isEffect = false;
    }
}
