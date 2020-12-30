﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;

    // Camera Targets

    private Transform flyTarget;
    public Transform landTarget;

    public float cameraHeight = 3f;
    public float cameraDistance = 3f;

    public float cameraRotation = 15f;

    public float cameraSmoothing = 0.1f;

    public bool canMoveCamera = true;
    public bool dialogueOn = false;


    // Start is called before the first frame update
    void Start()
    {
        flyTarget = GameObject.FindGameObjectWithTag("SpaceShip").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + cameraHeight, target.position.z - cameraDistance);
        transform.position = Vector3.Lerp(transform.position, newPos, cameraSmoothing*Time.deltaTime);
        Vector3 newRot = new Vector3(cameraRotation, transform.rotation.y, transform.rotation.z);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, newRot, cameraSmoothing * Time.deltaTime);

        if(canMoveCamera)
        {
            if (Input.GetAxis("Mouse Y") > 0.1f && cameraDistance > 10f)
            {
                cameraDistance = 10;
                cameraHeight = 5f;
                cameraRotation = 15f;
            }
            else if (Input.GetAxis("Mouse Y") < -0.1f && cameraDistance < 15f)
            {
                cameraDistance = 15f;
                cameraHeight = 7f;
                cameraRotation = 30f;
            }
        }
    }

    public void OverHeadCamera()
    {
        canMoveCamera = false;
        cameraDistance = 10f;
        cameraHeight = 7f;
        cameraRotation = 45f;
    }

    public void FlyCamera()
    {
        canMoveCamera = false;
        cameraDistance = 15f;
        cameraHeight = 20f;
        cameraRotation = 45f;
        target = flyTarget;
    }

    public void DialogueCamera()
    {
        cameraDistance = 7f;
        cameraHeight = 3f;
        cameraRotation = 15f;
        canMoveCamera = false;

    }

    public void ReturnCamera()
    {
        cameraDistance = 15f;
        cameraHeight = 7f;
        cameraRotation = 30f;
        canMoveCamera = true;
        target = landTarget;
    }
}
