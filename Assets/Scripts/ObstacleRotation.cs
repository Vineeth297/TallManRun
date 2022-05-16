using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotation : MonoBehaviour
{
	public float angle;
	private void Update()
    {
		transform.Rotate(Vector3.up * (angle * Time.deltaTime));       
    }
}
