using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.iOS;
using UnityEngine.UI;

public class SampleEffect : MonoBehaviour
{
    //�G�t�F�N�g
    [Header("�Ăяo���G�t�F�N�g")][SerializeField] private PlayEffect[] effects;
    //���݂̃G�t�F�N�g
    private int effectCurrent = 0;
    //�G�t�F�N�g�Đ�����ꏊ
    private Queue<GameObject> animalInfo = new Queue<GameObject>();

    //�G�t�F�N�g�Đ�����
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
        if (isEffect) return; // �G�t�F�N�g�Đ����͏������Ȃ�
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
                //���o�����J�n
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
                    //���o�����J�n
                    Debug.Log("StartEffect");
                    StartCoroutine(ActiveEffect());
                }
            }
        }
    }
    private void Effect(Vector2 pos)
    {
        // �G�t�F�N�g�Đ�����(���W)
        effects[effectCurrent].Play(pos);
        // ���̃G�t�F�N�g�Đ����鏀��
        effectCurrent++;
        //�G�t�F�N�g�̌Ăяo������ʂ�I�������J�E���g��0�ɂ���
        if (effects.Length == effectCurrent) effectCurrent = 0;
    }

    private IEnumerator ActiveEffect()
    {
        //�G�t�F�N�g�Đ����̔���ɂ���
        isEffect = true;
        //Queue�̒��g�̗v�f����0�ł͂Ȃ����菈�����s��
        while (animalInfo.Count != 0)
        {
            animalInfo.Peek().GetComponent<SpriteRenderer>().color = idle;
            StartCoroutine(Scaling(animalInfo.Peek(), false)); // �폜
            //�G�t�F�N�g�Đ�
            Effect(animalInfo.Dequeue().transform.position); // �G�t�F�N�g�Đ�
            yield return new WaitForSeconds(0.1f);
        }
        //�������I��������G�t�F�N�g�Đ����I������
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
