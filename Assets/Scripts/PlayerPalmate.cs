using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerPalmate : MonoBehaviour
{
    //�X���C�v�����̔���
    //private bool doSwaip;
    //�J���[���擾���A�o�t���󂯂Ă���l��
    private bool doPoworUp;
    //���������擾���A�f�o�t���󂯂Ă�����
    private bool doPoworDwon;
    //1�Q�[�����̊l���X�R�A
    private int _score;
    //
    //private Touch touch;
    //GameManager
    private GameManager gm;
    //scoreManager
    private scoreManager sm;
    //���ʉ��֌W
    //���̖���
    [SerializeField] private AudioClip cowSE;
    //�l�Y�~�̖���
    [SerializeField] private AudioClip mouseSE;
    //�^�b�`��
    [SerializeField] private AudioClip touchSE;

    //�v���p�e�B
    public int Score {
        get { return this._score; }
        set { this._score = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        sm = GameObject.Find("Scorecounter").GetComponent<scoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))//Input.touchCount == 1 && touch.phase == TouchPhase.Began
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin,(Vector2)ray.direction,100);
            //if(Physics2D.Raycast(ray,out hit2d,))
            if(hit2d.collider && hit2d.collider.tag == "animal")
            {
                AnimalController animalController = GetComponent<AnimalController>();
                int score = animalController.Score;
                Destroy(hit2d.collider.gameObject);
            }
        }
    }

    public void CanNotSwaip()
    {
        //doSwaip = false;
    }
    void GetAnimal(int score)
    {
        _score += score;
    }
}
