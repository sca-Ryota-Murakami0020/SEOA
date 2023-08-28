using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    [SerializeField] int score;
    [SerializeField] float speed;
    [SerializeField] string name;
    bool canGet = true;
    //[SerializeField] PlayerPalmate pp;

    public int Score
    {
        get {return  this.score;}
    }

    public string Name
    {
        get { return this.name;}
    }

    public bool CanGet
    {
        get { return this.canGet;}
        set { this.canGet = value;}
    }

    private void Update()
    {
        //if(!pp.DoStop)
        this.transform.position += this.transform.up * speed;
    }
}
