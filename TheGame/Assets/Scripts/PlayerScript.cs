﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public bool canMove = true;
    public float speed = 10f;
    public float jumpSpeed = 12f;
    public float turnSmoothing = 10f;
    public bool canJump = false;
    public float coolDownTime = 0f;

    private Vector3 movement;
    private Rigidbody myRB;
    private Animator myAnim;

    // Camera variables
    public Transform cameraTarget;
    public float cameraPoint;
    public CameraScript cameraScript;

    public GameObject seed;

    public float rayCheckLength = 0.4f;
    public ParticleSystem steps;
    public ParticleSystem stepPuff;

    // Variables for turnip carrying and throwing
    public bool canPickTurnip = false;
    public bool holdingTurnip = false;
    public GameObject activeTurnip;

    private DialogueManager dialogueManager;
    private NPCScript npc;

    // Variables for digging mechanic
    public bool canDig = false;

    // Variable for gamemanager that is persistent throughout the game
    private GameManager gm;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        myRB = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        cameraScript = Camera.main.GetComponent<CameraScript>();
    }

    // Update is called once per frame
    void Update()
    {
        myAnim.SetFloat("yVelocity",myRB.velocity.y);
        if(Input.GetButtonDown("Jump") && canJump && canMove)
        {
            myAnim.SetBool("isJumping",true);
            myRB.AddForce(Vector3.up * jumpSpeed);
        }

        if(Input.GetButtonDown("Fire1"))
        {
            if(canPickTurnip && !holdingTurnip)
            {
                activeTurnip.transform.parent = transform;
                activeTurnip.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
                canPickTurnip = false;
                holdingTurnip = true;
            }

            else if(holdingTurnip)
            {
                activeTurnip.transform.parent = null;
                activeTurnip.AddComponent<CapsuleCollider>();
                activeTurnip.AddComponent<Rigidbody>();
                activeTurnip.GetComponent<Rigidbody>().AddForce(transform.forward * 500);
                holdingTurnip = false;
                canPickTurnip = true;
            }

        }
    }

    public void Move(float hor, float ver)
    {

        movement.Set(hor, 0f, ver);
        movement = movement.normalized * speed * Time.deltaTime;

        if(hor == 0f && ver == 0f)
        {
            myRB.velocity = new Vector3(0f,myRB.velocity.y,0f);
        }

        if(hor !=0f || ver !=0f)
        {
            coolDownTime += Time.deltaTime;
            if (steps.isStopped && canJump)
            {
                steps.Play();
            }
            Rotating(hor, ver);
            myAnim.SetBool("isRunning", true);
        }
        else
        {
            coolDownTime = 0f;
            steps.Stop();
            myAnim.SetBool("isRunning", false);
        }

        if(!canJump)
        {
            steps.Stop();
        }

        if(coolDownTime>0.1f)
        {
            myRB.MovePosition(transform.position + movement);
        }

    }

    public void Rotating(float hor, float ver)
    {
        Vector3 targetDirection = new Vector3(hor, 0f, ver);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        Quaternion newRotation = Quaternion.Lerp(myRB.rotation, targetRotation, turnSmoothing * Time.deltaTime);
        myRB.MoveRotation(newRotation);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        if(canMove)
        {
            Move(hor, ver);
        }

        cameraTarget.position = new Vector3(transform.position.x, cameraPoint, transform.position.z);

        if(Physics.Raycast(transform.position,Vector3.down,out hit, rayCheckLength))
        {

            cameraPoint = hit.point.y;

            if (!canJump)
            {
                myAnim.SetBool("isJumping", false);
                if (myRB.velocity.y < -2f)
                {
                    Instantiate(stepPuff, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
                }
            }

            if(hit.transform.tag == "Wheel")
            {
                transform.parent = hit.transform;
            }
            else
            {
                transform.parent = null;
            }

            if(hit.transform.tag == "SandCube" && Input.GetButtonDown("Fire1"))
            {
                print("OnSand");
                hit.transform.gameObject.SetActive(false);
                canDig = true;
            }

            canJump = true;
        }
        else
        {
            canJump = false;    
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Turnip" && !holdingTurnip)
        {
            canPickTurnip = true;
            activeTurnip = other.gameObject;
        }

        if(other.gameObject.name == "ForestArea")
        {
            cameraScript.OverHeadCamera();
        }

        if(other.gameObject.name == "DoorOutToIn")
        {
            ChangeLevel(1);
        }

        if (other.gameObject.name == "DoorInToOut")
        {
            ChangeLevel(0);
            dialogueManager.ReturnLevel();
        }

        if (other.gameObject.name == "DoorToHomeUpStairs")
        {
            ChangeLevel(3);
        }

        if(other.gameObject.name == "DoorToDownStairs")
        {
            ChangeLevel(1);
        }

        if (other.gameObject.name == "DoorToFirePlace")
        {
            ChangeLevel(2);
        }

        if(other.gameObject.name == "SpaceShipTrigger")
        {
            gm.ActivateFly();
            gameObject.SetActive(false);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Turnip" && !holdingTurnip)
        {
            canPickTurnip = true;
            activeTurnip = other.gameObject;
        }

        if (other.gameObject.tag == "DialogueTrigger")
        {
            npc = other.gameObject.GetComponent<NPCScript>();
            if (Input.GetButtonDown("Fire1") && !dialogueManager.dialogueWindow.activeSelf)
            {
                other.gameObject.GetComponent<NPCScript>().dialogueIndicator.SetActive(false);
                dialogueManager.dialogueWindow.SetActive(true);
                npc.SetDialogue();
                canMove = false;
                cameraScript.DialogueCamera();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Turnip" && !holdingTurnip)
        {
            canPickTurnip = false;
            activeTurnip = null;
        }

        if (other.gameObject.name == "ForestArea")
        {
            cameraScript.ReturnCamera();
        }
    }

    public void ChangeLevel(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }
}
