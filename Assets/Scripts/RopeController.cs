using DG.Tweening;
using UnityEngine;

public class RopeController : MonoBehaviour
{
	private Quaternion _initLocalRot;

	private void Start()
	{
		_initLocalRot = transform.localRotation;
	}

	public void UpdateRope(Transform startingPosition, Transform endingPosition,CylinderGeneration cylinderGeneration)
	{
		var direction = endingPosition.position - startingPosition.position;
		var magnitude = direction.magnitude;
		cylinderGeneration.GetUpdated(direction, magnitude);
	}

	/*public void ReturnHome()
	{
		transform.DOLocalRotateQuaternion(_initLocalRot, 0.2f);
		ropeEnd.DOLocalMove(Vector3.zero, 0.2f)
			.OnUpdate(UpdateRope);
	}*/
}
