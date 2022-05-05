using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementControl : MonoBehaviour
{
	public float movementSpeed;
	public float xSpeed;
	public float xForce;
	public float leftBoundary;
	public float rightBoundary;
	public float rotationLerpValue = 0.1f;
	
	private float _swipeSpeed;
	

	private void Update()
	{
		
		if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

	#if UNITY_EDITOR
		xForce = Input.GetMouseButton(0) ? Input.GetAxis("Mouse X") * xSpeed : 0;
	#elif UNITY_ANDROID
        if(Input.touchCount> 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		  {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			xForce = touchDeltaPosition.x*_swipeSpeed*Mathf.Deg2Rad;
          }
		if(Input.touchCount> 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
		  {
			xForce = 0;
          }
	#endif
		
		if (Input.GetMouseButton(0))
		{
			transform.Translate((Vector3.forward * movementSpeed + new Vector3(xForce * xSpeed, 0f, 0f)) * Time.deltaTime, Space.World);
		}

		if(transform.position.x < leftBoundary)
			transform.position = new Vector3(leftBoundary,transform.position.y,transform.position.z);
		else if (transform.position.x > rightBoundary)
			transform.position = new Vector3(rightBoundary, transform.position.y, transform.position.z);

		if (xForce < 0f)
		{
			//rotate anticlockwise
			transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0,-90,0),rotationLerpValue );
		}else if (xForce > 0f)
		{
			//rotate clockwise
			transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0,90,0),rotationLerpValue );
		}
		else
		{
			//face forward
			transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0,0,0),rotationLerpValue );
		}
	}
}
