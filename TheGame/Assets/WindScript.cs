using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindScript : MonoBehaviour
{
    private Animator myAnim;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        float r = Random.Range(0, 2);
        if (r == 0)
        {
            myAnim.SetTrigger("ChangeWind");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePosition()
    {
        transform.parent.position = new Vector3(transform.parent.position.x + (Random.Range(-5f, 5f)), transform.parent.position.y + (Random.Range(-5f, 5f)), transform.parent.position.z + (Random.Range(-5f, 5f)));
        float r = Random.Range(0, 2);
        if(r==0)
        {
            myAnim.SetTrigger("ChangeWind");
        }
    }
}
