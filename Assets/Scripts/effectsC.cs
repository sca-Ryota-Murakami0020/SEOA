using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectsC : MonoBehaviour
{
    [Header("�i�[����G�t�F�N�g")][SerializeField]
    private Animator effect = null;
    //�����ʒu�̃x�N�g��
    [Header("�����ʒu")][SerializeField]
    private Vector2 resetPos;

    private void Awake()
    {
        this.transform.position = resetPos;
    }

    public void PlayEffect(GameObject animal)
    {
        //�G�t�F�N�g�̈ʒu�𓮕��̍��W�ɍ��킹�Ĉړ�����
        this.transform.position = animal.transform.position;
        //�����ŃA�j���[�V��������Trigger���N��������
        effect.SetTrigger("Play");
        //�\�����Ԃ��߂邽�߂̃R���[�`�����N��������
        StartCoroutine(BackEffect());
    }

    private IEnumerator BackEffect()
    {
        //0.5�b�ԑҋ@�����̂��ɏ����ʒu�ɖ߂�
        yield return new WaitForSeconds(0.5f);
        this.transform.position = resetPos;
    }
}
