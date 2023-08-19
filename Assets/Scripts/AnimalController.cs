using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    [SerializeField] int score;
    [SerializeField] float speed;
    [SerializeField] string name;
    public int Score {
        get {return  this.score;}
    }
    public string Name
    {
        get { return this.name;}
    }

    private void Update()
    {
        this.transform.position += this.transform.up * speed;
    }
}
