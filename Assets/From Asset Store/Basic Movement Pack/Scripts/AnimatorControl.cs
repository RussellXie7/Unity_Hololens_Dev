using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public class AnimatorControl : MonoBehaviour {

	public bool grounded = false;	//This controller uses the character controller compnent, which keeps track of grounding. This is a debugging variable. 
	public float state = 2;			//In this controller, state 0 is prone, state 1 is crouched, and state 2 is standing.
	public float smooth = 0.25f;	//This variable controls to what degree the input and state will be lerped when passed to the animator. This is being done on lines 59 - 60

	[Header ("Character Movement Variables")]
	public float gravity = -9.81f;
	public bool sprint = false;
	public float proneSpeed = 0.5f;
	public float crouchSpeed = 1.5f;
	public float jogSpeed = 3f;
	public float sprintSpeed = 4f;
	public float jumpHeight = 2f;

	Animator animator;
	CharacterController controller;
	float lerpState;
	float velocityY; //This variable is used to track how fast the character is moving vertically when not grounded
	Vector2 input;

	void Start () {
		animator = GetComponent <Animator> ();
		controller = GetComponent <CharacterController> ();
	}
	
	void Update () {

		if (controller.isGrounded) //Determine if grounded.
			grounded = true;
		else
			grounded = false;

		animator.SetFloat ("State", lerpState);			//
		animator.SetBool ("Sprint", sprint);			//
		animator.SetBool ("Grounded", grounded);		//Setting all of the animator variables. 
		animator.SetFloat ("Horizontal", input.x);		//
		animator.SetFloat ("Vertical", input.y);		//
		animator.SetFloat ("Fall Speed", velocityY);	//Fall speed is used to interpolate between Fall and Fall_Flail (see base layer in Character animator)

		if (grounded) {
			
			if (Input.GetKey (KeyCode.LeftShift) && Mathf.Abs (Mathf.Round (input.x)) == 0 && input.y > 0) //Determine if sprint should be true or false
				sprint = true;
			else
				sprint = false;

			if (input.magnitude > 1) { //This is used to prevent the character from moving faster than the set speeds above when moving diagonally. 
				input.Normalize();
			}

			lerpState = Mathf.Lerp (lerpState, state, smooth);																															
			input = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));

			velocityY = 0;

			if (Input.GetKeyDown (KeyCode.Space) && state == 2) {
				velocityY = Mathf.Sqrt (2 * jumpHeight * -gravity); //Calculate the velocity required to achieve the jump height.

				animator.Play ("Jump");	//Play the jump animation 
			}

			if (Input.GetKeyDown (KeyCode.C) && state != 0)		//
				state--;        								//
																//Determining what state the character is in (prone, crouched, standing).
			if (Input.GetKeyDown (KeyCode.Space) && state != 2) //
				state++;										//

			Move (); //Only call move if grounded, helps to preserve velocity while in air
		} 


		if (!grounded) {
			if (sprint)	//This block is determining what the input should be multiplied by to achieve the correct in-air velocity.
				controller.Move ((transform.TransformDirection(new Vector3 (input.x, 0, input.y)) * sprintSpeed) * Time.deltaTime);
			else
				controller.Move (transform.TransformDirection((new Vector3 (input.x, 0, input.y)) * jogSpeed) * Time.deltaTime);
		}

		velocityY += gravity * Time.deltaTime;
		controller.Move (Vector3.up * velocityY * Time.deltaTime); //Always move the controller down, prevents the Character Controller from flipping between grounded and not grounded

	}

	void Move (){//Move the character based on the state, at the correct velocities. 
		if (state == 0) {
			controller.Move (transform.TransformDirection(new Vector3 (input.x, 0, input.y)) * Time.deltaTime * proneSpeed);
		}

		if (state == 1) {
			controller.Move (transform.TransformDirection(new Vector3 (input.x, 0, input.y)) * Time.deltaTime * crouchSpeed);
		}

		if (state == 2){
			if (sprint) {
				controller.Move (transform.TransformDirection(new Vector3 (input.x, 0, input.y)) * Time.deltaTime * sprintSpeed);
			} else {
				controller.Move (transform.TransformDirection(new Vector3 (input.x, 0, input.y)) * Time.deltaTime * jogSpeed);
			}
		}
	}


    public void SetAnimationState(string stateName)
    {
        switch (stateName)
        {
            case "Walk":
                sprint = false;
                break;
            case "Run":
                sprint = true;
                break;
        }
    }
}
