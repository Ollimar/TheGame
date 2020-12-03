using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;

    public float cameraSmoothing = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z - 12f),cameraSmoothing*Time.deltaTime);
    }
}
