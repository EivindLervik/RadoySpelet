using UnityEngine;
using System.Collections;

public class PlayerMoveScript : MonoBehaviour {

    public GameObject kamera;

    public float acceleration;
    public float maxSpeed;

    public float boredWaitTime;
    public bool isCinematic;

    private Rigidbody body;
    private Animator anime;

    private Vector3 targetForward;
    private bool boredWait;

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

            body.AddForce(movement * Time.deltaTime * 500.0f * acceleration);
        }
    }

	

    private void Animation()
    {
		if(Input.GetAxis("Vertical") != 0.0f || Input.GetAxis("Horizontal") != 0.0f)
        {
            anime.SetBool("Moving", true);
        }
        else
        {
            anime.SetBool("Moving", false);
        }

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
