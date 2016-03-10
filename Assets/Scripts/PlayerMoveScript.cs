using UnityEngine;
using System.Collections;

public class PlayerMoveScript : MonoBehaviour {

    public GameObject kamera;

    public float acceleration;
    public float sprintSpeed;
    public float walkSpeed;
    //public float maxSpeed;

    public float boredWaitTime;
    public bool isCinematic;

    private Rigidbody body;
    private Animator anime;

    private Vector3 targetForward;
    private bool boredWait;
    private bool sprinting;
    private bool walking;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody>();
        anime = GetComponentInChildren<Animator>();
        boredWait = false;
	}
	
	// Update is called once per frame
	void Update () {
        transform.forward = Vector3.Lerp(transform.forward, targetForward, 0.25f);
        Animation();
        InputHandeler();
	}

    void FixedUpdate(){
        if (!isCinematic)
        {
            Vector3 movement = new Vector3();

            if(Input.GetAxis("Vertical") > 0.0f)
            {
                movement += kamera.transform.forward;
                targetForward = movement;
            }
            if (Input.GetAxis("Vertical") < 0.0f)
            {
                movement -= kamera.transform.forward;
                targetForward = movement;
            }
            if (Input.GetAxis("Horizontal") > 0.0f)
            {
                movement += kamera.transform.right;
                targetForward = movement;
            }
            if (Input.GetAxis("Horizontal") < 0.0f)
            {
                movement -= kamera.transform.right;
                targetForward = movement;
            }

            movement.Normalize();

            // Modefiers
            float modefier = 1.0f;
            if (sprinting)
            {
                modefier = 2.0f * sprintSpeed;
            }
            else if (walking)
            {
                modefier = 0.5f * walkSpeed;
            }

            body.AddForce(movement * Time.deltaTime * 500.0f * acceleration * modefier);
        }
    }

	private void InputHandeler()
    {
        if (sprinting != Input.GetButton("Sprint"))
        {
            sprinting = Input.GetButton("Sprint");
        }

        else if (walking != Input.GetButton("Walk"))
        {
            walking = Input.GetButton("Walk");
        }
    }

    private void Animation()
    {
        // Move anim
		if(Input.GetAxis("Vertical") != 0.0f || Input.GetAxis("Horizontal") != 0.0f)
        {
            anime.SetBool("Moving", true);
        }
        else
        {
            anime.SetBool("Moving", false);
        }

        // Type of walk anim
        if (walking)
        {
            anime.SetInteger("MoveSpeed", 1);
        }
        else if (sprinting)
        {
            anime.SetInteger("MoveSpeed", 3);
        }
        else
        {
            anime.SetInteger("MoveSpeed", 2);
        }

        // Idle anim
        if (Input.anyKey)
        {
            if (boredWait)
            {
                boredWait = false;
                //print("OFF");
                anime.SetBool("Bored", boredWait);
                StopCoroutine("Bored");
            }
        }
        else
        {
            if (!boredWait)
            {
                //print("ON");
                StartCoroutine("Bored");
            }
        }
    }

    IEnumerator Bored()
    {
        boredWait = true;
        yield return new WaitForSeconds(boredWaitTime);
        anime.SetBool("Bored", true);
    }

	private void DoJump(Vector3 direction){
		
	}
}
