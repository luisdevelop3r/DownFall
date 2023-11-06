using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movile : MonoBehaviour
{
    public Transform[] Pose;
    public float Speed;
    public int ID;
    public int suma;
    void Start()
    {
        transform.position = Pose[0].position;
    }
    void Update()
    {
        if (transform.position == Pose[ID].position)
        {
            ID += suma;
        }
        if (ID == Pose.Length - 1)
        {
            suma = -1;
        }
        if (ID == 0)
        {
            suma = 1;
        }


        transform.position = Vector3.MoveTowards(transform.position, Pose[ID].position, Speed * Time.deltaTime);



    }
}
