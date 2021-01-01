using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceShipScript : MonoBehaviour
{
    public float speed = 5f;
    public float turnSmoothing = 15f;
    private Vector3 movement;
    private Rigidbody myRB;
    private Animator myAnim;

    public float launchTimer;
    public float launchPower = 600f;

    public bool activate = false;
    public bool canMove = false;
    public bool canLand = false;

    public bool stage1 = true;
    public bool stage2 = false;

    public Transform landingTarget;

    public GameObject landButton;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm      = GameObject.Find("GameManager").GetComponent<GameManager>();
        myRB    = GetComponent<Rigidbody>();
        myAnim  = GetComponent<Animator>();
        myAnim.enabled = false;
        landButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Jump") && activate)
        {
           myRB.AddForce(Vector3.up * launchPower);
        }

        if(Input.GetButtonUp("Jump"))
        {
            //myAnim.enabled = false;
            launchTimer = 0f;
        }

        if(Input.GetButton("Jump") && canLand)
        {
            Landing();
        }

        if(transform.position.y >= 8f)
        {
            activate = false;
            canMove = true;
            myRB.useGravity = false;
            myRB.isKinematic = true;
            myRB.velocity = Vector3.zero;
            //RenderSettings.skybox = gm.skyBoxNight;
            //transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    private void FixedUpdate()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        if(canMove)
        {
            Move(hor, ver);
        }
    }

    public void Move(float hor, float ver)
    {
        movement.Set(hor, 0f, ver);
        movement = movement.normalized * speed * Time.deltaTime;
        myRB.MovePosition(transform.position + movement);

        if(hor !=0f || ver != 0f)
        {
            Rotating(hor,ver);
        }
    }

    public void Rotating(float hor, float ver)
    {
        Vector3 targetDirection = new Vector3(hor, 0f, ver);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        Quaternion newRotation = Quaternion.Lerp(myRB.rotation, targetRotation, turnSmoothing * Time.deltaTime);
        myRB.MoveRotation(newRotation);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "LandingPlatform")
        {
            landingTarget = other.gameObject.transform;
            canLand = true;
            landButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "LandingPlatform")
        {
            landingTarget = null;
            canLand = false;
            landButton.SetActive(false);
        }
    }

    public void Landing()
    {
        transform.position = landingTarget.transform.GetChild(0).position;
        canMove = false;
        transform.eulerAngles = new Vector3(-90f, 0f, 0f);
        gm.ActivateWalk();
        canLand = false;
        landButton.SetActive(false);
    }
}
