using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnFall : MonoBehaviour
{
    private Rigidbody2D rb;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null )
        {
            // Apply torque to make the object rotate as it falls
            rb.AddTorque(2.5f, ForceMode2D.Impulse);
        }
        else
        {
            return;
        }    
    }
}
