using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float HP;
    
    public float AttackPower;
    public float Denfendar;

    AnimationCurve curve;
    AnimationEvent animationEvent;

    private void Start()
    {

    }

    public void SayHello()
    {
        Debug.Log("Hello!");
    }
}
