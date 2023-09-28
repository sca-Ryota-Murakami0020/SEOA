using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    //�t�F�[�h�C���E�t�F�[�h�A�E�g���s��Canvas
    [Header("�t�F�[�h�֌W�p�A�j���[�V����")][SerializeField]
    protected Animator fadeAnimation;
    //�^�C�g���p�A�j���[�V����
    [Header("�^�C�g���p�A�j���[�V����")][SerializeField]
    protected Animator logAnimation;
    //�{�^���p�̃A�j���[�V����
    [Header("�{�^���p�A�j���[�V����")][SerializeField]
    protected Animator buttonAnimation;

    //�^�C�g�����S�̃A�j���[�V�������Đ�����
    public void StartLogAnimation()
    {
        logAnimation.SetTrigger("StartLogAnimation");
    }

    //���[�v����{�^���̃A�j���[�V�������Đ�����
    public void StartLoopAnimation()
    {
        logAnimation.SetTrigger("LoopingAni");
    }

    //�{�^���p�̃A�j���[�V�������s��
    public void StartButtonAnimation()
    {
        buttonAnimation.SetTrigger("StartButtonAnimation");
    }

    //�t�F�[�h�C�����s��
    public void StartFadeIn()
    {
        fadeAnimation.SetTrigger("StartFadeIn");
    }

    //�t�F�[�h�A�E�g���s��
    public void StartFadeOut()
    {
        fadeAnimation.SetTrigger("StartFadeOut");
    }

    //�^�C�g������Q�[����
    public void GoGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
