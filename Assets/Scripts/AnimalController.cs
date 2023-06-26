using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    [SerializeField] int score;
    [SerializeField] float speed;
    public int Score {
        get {return  score;}
    }
}
