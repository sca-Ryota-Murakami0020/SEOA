using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slidreTimeC : MonoBehaviour
{
    //�X���C�v�ł��鎞��
    private float slidTime;
    //�������p�̕ϐ�
    private float defultTime = 3.0f;
    //�v���C���[�̑�����Ǘ����Ă���X�N���v�g
    private PlayerPalmate pl;
    //�X���C�v�ł��鎞�Ԃ���������Q�[�W
    [SerializeField] private Slider swaipGaze;

    // Start is called before the first frame update
    void Start()
    {
        //�Q�[�W�̏�����
        slidTime = defultTime;
        pl = GetComponent<PlayerPalmate>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Slid()
    {
        slidTime -= Time.deltaTime;
        if(slidTime <= 0.0f)
        {
            pl.CanNotSwaip();
        }
    }

    private void ResetTime()
    {
        slidTime = defultTime;
    }
}
