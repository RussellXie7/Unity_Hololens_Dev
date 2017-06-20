using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAction : MonoBehaviour {

    private float rotationFactor;
    public float moveFactor = 5;
    private Vector3 manipulationPreviousPosition;
    

    private void Update()
    {
        PerformRotation();
    }

    void PerformManipulationStart(Vector3 position)
    {
        Destroy(transform.gameObject.GetComponent<Rigidbody>());
        manipulationPreviousPosition = position;
    }

    void PerformManipulationUpate(Vector3 position)
    {
        Vector3 moveVector = Vector3.zero;

        moveVector = position - manipulationPreviousPosition;

        manipulationPreviousPosition = position;

        transform.position += moveVector*moveFactor;
    }

    void PerformRotation()
    {
        if (Gesture.Instance.isNavigating)
        {
            rotationFactor = Gesture.Instance.NavigationPosition.x;
            transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
        }
    }
    //void OnSelect()
    //{
    //    Debug.Log("I received the message who called the OnSelect");
    //    if (!this.GetComponent<Rigidbody>())
    //    {
    //        Debug.Log("The collision detection is discrete to save some fps for now!");
    //        // add component funciton returns the component just added
    //        var rigidBody = this.gameObject.AddComponent<Rigidbody>();
    //        rigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
    //    }
    //}

    
}
