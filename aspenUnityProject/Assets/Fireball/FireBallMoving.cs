using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallMoving : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject fireBall;

    void Start()
    {
        
    }

    // Change the coordinates to shoot the fire ball in that specific direction (Base: X Position / 15). 
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(fireBall, shootingPoint.transform.position, Quaternion.identity);
            this.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(15, 0, 0));
        }
    }
}
