using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.iOS;
using UnityEngine.UI;

public class SampleEffect : MonoBehaviour
{
    //エフェクト
    [Header("呼び出すエフェクト")][SerializeField] private PlayEffect[] effects;
    //現在のエフェクト
    private int effectCurrent = 0;
    //エフェクト再生する場所
    private Queue<GameObject> animalInfo = new Queue<GameObject>();

    //エフェクト再生中か
    private bool isEffect = false;

    [SerializeField] private GameObject[] bubble = null;
    [SerializeField] private Vector3 startScale = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 endScale = new Vector3(1f, 1f, 0f);
    [SerializeField] private Text debug;
    private Color idle = Color.white;
    private Color touched = Color.red;

    private void Awake()
    {
        Generate();
    }
    private void Generate()
    {
        foreach (GameObject b in bubble)
        {
            if (b.transform.localScale != endScale) 
                StartCoroutine(Scaling(b, true));
        }
    }

    private void Update()
    {
        if (isEffect) return; // エフェクト再生中は処理しない
        PCControll();
        IOSControll();
        void PCControll()
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 100);
                if (hit2d.collider && hit2d.collider.tag == "animal")
                {
                    if (hit2d.collider.gameObject.GetComponent<SpriteRenderer>().color == touched) return;
                    hit2d.collider.gameObject.GetComponent<SpriteRenderer>().color = touched;
                    
                    animalInfo.Enqueue(hit2d.collider.gameObject);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                //演出処理開始
                Debug.Log("StartEffect");
                StartCoroutine(ActiveEffect());
            }
        }
        void IOSControll()
        {
            if(Input.touchCount > 0) 
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    debug.text = "moved = (" + touch.position.x.ToString("F1") + ", " + touch.position.y.ToString("F1") + ")";
                    Ray ray = Camera.main.ScreenPointToRay(Camera.main.ScreenToWorldPoint(touch.position));
                    RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 100);
                    if (hit2d.collider && hit2d.collider.tag == "bubble")
                    {
                        if (hit2d.collider.gameObject.GetComponent<SpriteRenderer>().color == touched) return;
                        hit2d.collider.gameObject.GetComponent<SpriteRenderer>().color = touched;
                        animalInfo.Enqueue(hit2d.collider.gameObject);
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    //演出処理開始
                    Debug.Log("StartEffect");
                    StartCoroutine(ActiveEffect());
                }
            }
        }
    }
    private void Effect(Vector2 pos)
    {
        // エフェクト再生する(座標)
        effects[effectCurrent].Play(pos);
        // 次のエフェクト再生する準備
        effectCurrent++;
        //エフェクトの呼び出しが一通り終わったらカウントを0にする
        if (effects.Length == effectCurrent) effectCurrent = 0;
    }

    private IEnumerator ActiveEffect()
    {
        //エフェクト再生中の判定にする
        isEffect = true;
        //Queueの中身の要素数が0ではない限り処理を行う
        while (animalInfo.Count != 0)
        {
            animalInfo.Peek().GetComponent<SpriteRenderer>().color = idle;
            StartCoroutine(Scaling(animalInfo.Peek(), false)); // 削除
            //エフェクト再生
            Effect(animalInfo.Dequeue().transform.position); // エフェクト再生
            yield return new WaitForSeconds(0.1f);
        }
        //処理が終了したらエフェクト再生を終了する
        isEffect = false;
        Generate();
    }

    private IEnumerator Scaling(GameObject g, bool b)
    {
        float time = 0f;
        if (b)
        {
            while (time <= 1f)
            {
                time += Time.deltaTime;
                float t = time / 1f;

                g.transform.localScale = Vector2.Lerp(startScale, endScale, t);
                yield return null;
            }
        }
        else g.transform.localScale = startScale;
        yield break;
    }
}
