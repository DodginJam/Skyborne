using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{


    public GameObject plane;

    public Vector3 followpostion;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;


        followpostion.x = plane.transform.position.x;

        followpostion.z = plane.transform.position.z;   

        gameObject.transform.position = followpostion;

    }
}
