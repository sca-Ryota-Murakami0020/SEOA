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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount == 1 )
        {

        }
    }

    public void CanNotSwaip()
    {
        doSwaip = false;

    }
}
