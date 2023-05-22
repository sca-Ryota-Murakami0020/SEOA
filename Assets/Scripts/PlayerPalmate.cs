using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPalmate : MonoBehaviour
{
    //�X���C�v�����̔���
    private bool doSwaip;
    //�J���[���擾���A�o�t���󂯂Ă���l��
    private bool doPoworUp;
    //���������擾���A�f�o�t���󂯂Ă�����
    private bool doPoworDwon;
    //1�Q�[�����̊l���X�R�A
    private int score;
    //
    private Touch touch;

    //�v���p�e�B
    public int Score {
        get { return this.score; }
        set { this.score = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))//Input.touchCount == 1 && touch.phase == TouchPhase.Began
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin,(Vector2)ray.direction,100);
            if(hit2d && hit2d.collider.tag == "��")
            {
                //�p�x�Ƌ����̌v�Z
                GetCow();
                Debug.Log("��������");
                Destroy(hit2d.collider.gameObject);
            }
        }
    }

    public void CanNotSwaip()
    {
        doSwaip = false;
    }

    public void GetCow()
    {
        score += 100;
    }

    public void GetMouse()
    {
        score += 30;
    }
}
