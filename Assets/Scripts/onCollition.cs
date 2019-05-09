using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onCollition : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        Debug.Log("collide with: " + col.gameObject.name);
    }
}
