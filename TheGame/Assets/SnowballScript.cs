using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballScript : MonoBehaviour
{
    public Vector3 scale = new Vector3(1f,1f,1f);
    public bool rolling = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(rolling)
        {
            scale += new Vector3(0.1f, 0.1f, 0.1f);
            transform.localScale = scale;
        }
    }
}
