using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = 10f;
    public float jumpSpeed = 12f;
    public float turnSmoothing = 10f;
    public bool canJump = false;

    private Vector3 movement;
    private Rigidbody myRB;
    private Animator myAnim;

    public float rayCheckLength = 0.4f;
    public ParticleSystem steps;
    public ParticleSystem stepPuff;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        myAnim.SetFloat("yVelocity",myRB.velocity.y);
        if(Input.GetButtonDown("Jump") && canJump)
        {
            myAnim.SetBool("isJumping",true);
            myRB.AddForce(Vector3.up * jumpSpeed);
        }
    }

    public void Move(float hor, float ver)
    {

        movement.Set(hor, 0f, ver);
        movement = movement.normalized * speed * Time.deltaTime;

        if(hor !=0f || ver !=0f)
        {
            if(steps.isStopped && canJump)
            {
                steps.Play();
            }
            Rotating(hor, ver);
            myAnim.SetBool("isRunning", true);
        }
        else
        {
            steps.Stop();
            myAnim.SetBool("isRunning", false);
        }

        if(!canJump)
        {
            steps.Stop();
        }

        myRB.MovePosition(transform.position + movement);
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

        Move(hor, ver);


        if(Physics.Raycast(transform.position,Vector3.down,out hit, rayCheckLength))
        {
            if(!canJump)
            {
                myAnim.SetBool("isJumping", false);
            }

            if (myRB.velocity.y < -2f)
            {
               Instantiate(stepPuff, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            }

            canJump = true;
        }
        else
        {
            canJump = false;    
        }
    }
}
