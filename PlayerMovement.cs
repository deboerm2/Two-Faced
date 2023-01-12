using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //Animation stuff
    public Animator animator;


    //sound stuff
    public AudioSource playerFootsteps;
    public AudioSource SwitchSound;
    


    //move speed of the player
    public float moveSpeed = 5f;
    private Rigidbody playerRigid;
    public bool hyde=false;
    public float timeremaining=10;
    private GameObject pauseMenu;

    //keys
    public bool hasGateKey;
    public bool hasHouseKey;
    public GameObject GateKeyUI;
    public GameObject HouseKeyUI;

    // Start is called before the first frame update
    void Start()
    {

        hasGateKey = false;
        GateKeyUI.SetActive(false);
        hasHouseKey = false;
        HouseKeyUI.SetActive(false);

        playerRigid = GetComponent<Rigidbody>();

        playerFootsteps = GetComponent<AudioSource>();

        pauseMenu = GameObject.Find("PauseMenu");
        pauseMenu.SetActive(false);
    }

    
    // Update is called once per frame
    void LateUpdate()
    {
        if(!hyde){
            if (timeremaining>0){
                //Debug.Log("time"+timeremaining);
                timeremaining-=Time.deltaTime;
            }
            else{
                StartCoroutine(switchSoundPlay());
                //hyde =true;
                timeremaining=10;
                //Play Switching sound
                //StartCoroutine(switchSoundPlay());
                //animation for switching art to Hyde
                //animator.SetBool("Hyde", true);

                //I moved all of the above code into the coroutine below to time the audio clip better
            }
        }
        HandleMovement();

        //PauseMenu stuff
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf == true)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        playerRigid.velocity = (new Vector3(horizontal, 0, vertical) * moveSpeed);

        //combines both horizontal and vertical for the animation check below
        float universal = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        //makes walk animation start if moving
        //animator.SetFloat("Speed", Mathf.Abs(universal));
        

        //makes sprite face direction they are walking
        if (horizontal > 0f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            
        } else if (horizontal < 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            
        }

        //if moving play walk sound
        if (Mathf.Abs(universal) > 0)
        {
            playerFootsteps.enabled = true;
            animator.SetFloat("Speed", 1f);
        } else if (Mathf.Abs(universal) == 0)
        {
            playerFootsteps.enabled = false;
            animator.SetFloat("Speed", 0f);
        }
    }

    void OnTriggerEnter(Collider collision){
        if (collision.gameObject.tag == "Tea"){
            //animation for switching art to Hyde
            animator.SetBool("Hyde", false);
            hyde =false;
            
        }


        if (collision.gameObject.tag == "tutorialEnd")
        {
            //load main scene
            SceneManager.LoadScene(1);
            Time.timeScale = 0;

        }

        if(collision.gameObject.tag == "GateKey")
        {
            hasGateKey = true;
            GateKeyUI.SetActive(true);
            Debug.Log("GATE");
        }

        if (collision.gameObject.tag == "HouseKey")
        {
            hasHouseKey = true;
            HouseKeyUI.SetActive(true);
            Debug.Log("HOUSE");
        }
    }
    //CoRoutine that plays switching sound
    public IEnumerator switchSoundPlay()
    {
        SwitchSound.enabled = true;
        yield return new WaitForSeconds(1f);
        hyde = true;
        animator.SetBool("Hyde", true);
        yield return new WaitForSeconds(1f);
        SwitchSound.enabled = false;
    }

    


}
