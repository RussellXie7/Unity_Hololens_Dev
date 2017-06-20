using UnityEngine;
using System.Collections;

//This script is attached to a camera parent, not the camera, creating a simple pivot. 

public class CameraControl : MonoBehaviour {
	public float sensitivity = 2f;
	public Transform character; // The root of the character, used to assign rotation
	public float maxAngle = 70f, minAngle = -70f;

	float rotX;

	void Update (){
		character.Rotate (Vector3.up, Input.GetAxis ("Mouse X") * sensitivity); 		//rotate the character
			//rotate the camera
		rotX = Mathf.Clamp (rotX, minAngle, maxAngle);									//limits min and max rotation of the camera
		rotX += -Input.GetAxis ("Mouse Y") * sensitivity;								

		transform.localEulerAngles = new Vector3 (rotX, 0, 0);							//rotates the camera root
	}
}