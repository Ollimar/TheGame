using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject spaceShip;

    public float timer;
    public float changeTimer;
    public float moveTimer;
    public float stopTime;
    public float wheelSpeed;
    public int wheels;
    public GameObject[] wheel;
    public bool rotate = false;

    public GameObject sun;
    public Material mySkybox;
    public Material skyBoxDay;
    public Material skyBoxNight;

    public Texture skyDay;
    public Texture skyNight;

    public GameObject[] npc;
    public GameObject[] storyBooks;

    // Missions and progression variables
    public bool[] missions;

    public static GameManager gameManager;

    // Variable for checking if we are in spaceship
    public bool flying = false;

    // Camera
    private CameraScript cam;


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Awake()
    {
        if(gameManager == null)
        {
            DontDestroyOnLoad(gameObject);
            gameManager = this;
        }

        else if(gameManager != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        wheel       = GameObject.FindGameObjectsWithTag("Wheel");
        player      = GameObject.FindGameObjectWithTag("Player");
        spaceShip   = GameObject.FindGameObjectWithTag("SpaceShip");
        cam         = Camera.main.GetComponent<CameraScript>();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player      = GameObject.FindGameObjectWithTag("Player");
        spaceShip   = GameObject.FindGameObjectWithTag("SpaceShip");
        cam         = Camera.main.GetComponent<CameraScript>();
        wheel       = GameObject.FindGameObjectsWithTag("Wheel");
        npc         = GameObject.FindGameObjectsWithTag("NPC");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // Experimental section for changing skybox with sun rotation
        /*
        sun.transform.Rotate(Vector3.up * 1f * Time.deltaTime);

        if(sun.transform.eulerAngles.y > 50f)
        {
            RenderSettings.skybox = mySkybox;
            print("Night");
            mySkybox.Lerp(mySkybox, skyBoxNight, 0.1f * Time.deltaTime);
        }
        */

        RenderSettings.skybox.SetFloat("_Rotation", Time.time*-1f);

        if(timer > changeTimer)
        {
            moveTimer += Time.deltaTime;
            rotate = true;

            if (moveTimer > stopTime)
            {
                timer = 0f;
                moveTimer = 0f;
                rotate = false;
            }
        }
    }

    void FixedUpdate()
    {

        // This is for rotating the wooden cogs

        /*
            if (rotate)
            {
                wheel[0].transform.Rotate(Vector3.up * wheelSpeed);
                wheel[1].transform.Rotate(Vector3.up * -wheelSpeed);
                wheel[2].transform.Rotate(Vector3.up * -wheelSpeed);
            }
        */

    }

    public void ActivateFly()
    {
        flying = true;
        spaceShip.GetComponent<SpaceShipScript>().activate = true;
        cam.FlyCamera();
        player.SetActive(false);
    }

    public void ActivateWalk()
    {
        flying = false;
        cam.ReturnCamera();
        player.transform.position = new Vector3(spaceShip.transform.position.x,spaceShip.transform.position.y,spaceShip.transform.position.z+5f);
        player.SetActive(true);
    }

    public void MissionComplete(int mission)
    {
        missions[mission] = true;
        int nPc = npc[mission].GetComponentInChildren<NPCScript>().npcNumber;
        npc[nPc].GetComponentInChildren<NPCScript>().missionComplete = true;
    }
}
