using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;

    public float cameraHeight = 3f;
    public float cameraDistance = 3f;

    public float cameraSmoothing = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + cameraHeight, target.position.z - cameraDistance);
        transform.position = Vector3.Lerp(transform.position, newPos, cameraSmoothing*Time.deltaTime);
    }
}
