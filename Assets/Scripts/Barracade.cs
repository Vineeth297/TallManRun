using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracade : MonoBehaviour
{
	private Rigidbody _rb;
	public float forceToApply;
	public float angleOfForce;

	private float _xComponent;
	private float _yComponent;

	private Collider _collider;

	private void Start()
	{
		_rb = GetComponent<Rigidbody>();
        
		//angleOfForce = Random.Range(45f, 360f);
		//angleOfForce = 90f;
        
		_collider = GetComponent<Collider>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player")) return;
		transform.parent = null;
	//	PlayerMovementControl.Instance.ShrinkThePlayer();
		//Fly away
		//_rb.isKinematic = false;
		AddForceAtAnAngle(forceToApply, angleOfForce);
		SoundManager.Instance.PlaySound(SoundManager.Instance.obstacleHitSound);
		PlayerMovementControl.Instance.MakePlayerShort(1);
		PlayerMovementControl.Instance.DeBuffThePlayer(1);
		
		_collider.enabled = false;
	}

	
	private void AddForceAtAnAngle(float force, float angle)
	{
		angle *= Mathf.Deg2Rad;
		_xComponent = Mathf.Cos(angle) * force;
		_yComponent = Mathf.Sin(angle) * force;
        
		var forceApplied = new Vector3(_xComponent, _yComponent, 30);
        
		_rb.AddForce(forceApplied);
	}

}
