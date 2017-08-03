using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingArrow : MonoBehaviour
{
    public enum Orientation
    {
        AroundYAxis,
        AroundZAxis
    }

    public float speed = 270f;
    public Orientation orientation;
    private Vector3 axis;
    

    // Use this for initialization
    void Start()
    {
        if (orientation == Orientation.AroundYAxis) axis = transform.up;
        if (orientation == Orientation.AroundZAxis) axis = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, axis, Time.deltaTime * speed);
    }

}