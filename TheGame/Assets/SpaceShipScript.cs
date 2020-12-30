using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipScript : MonoBehaviour
{
    public float speed = 5f;
    public float turnSmoothing = 15f;
    private Vector3 movement;
    private Rigidbody myRB;

    public bool activate = false;
    public bool canMove = false;
    public bool canLand = false;

    public Transform landingTarget;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        myRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Jump") && activate)
        {
            myRB.AddForce(Vector3.up * 10f);
        }

        if(Input.GetButton("Jump") && canLand)
        {
            Landing();
        }

        if(transform.position.y >= 8f)
        {
            activate = false;
            canMove = true;
            myRB.velocity = Vector3.zero;
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "LandingPlatform")
        {
            landingTarget = null;
            canLand = false;
        }
    }

    public void Landing()
    {
        transform.position = landingTarget.transform.GetChild(0).position;
        canMove = false;
        transform.eulerAngles = new Vector3(-90f, 0f, 0f);
        gm.ActivateWalk();
        canLand = false;
    }
}
