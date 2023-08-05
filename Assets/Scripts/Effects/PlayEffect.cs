using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEffect : MonoBehaviour
{
    [SerializeField] private Animator _myEffect = null;
    [SerializeField] private Vector2 resetPos = new Vector2(10000f, 10000f);
    private void Awake()
    {
        this.transform.position = resetPos;
    }
    public void Play(Vector2 pos)
    {
        this.transform.position = pos;
        _myEffect.SetTrigger("Play");
        StartCoroutine(ResetPos());
    }
    private IEnumerator ResetPos()
    {
        yield return new WaitForSeconds(0.5f);
        this.transform.position = resetPos;
    }
}
