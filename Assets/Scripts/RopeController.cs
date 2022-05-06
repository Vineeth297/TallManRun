using DG.Tweening;
using UnityEngine;

public class RopeController : MonoBehaviour
{
	[SerializeField] private Transform ropeStart, ropeEnd;
	[SerializeField] private CylinderGeneration cylinder;

	private Quaternion _initLocalRot;

	private void Start()
	{
		_initLocalRot = transform.localRotation;
	}

	public void UpdateRope()
	{
		var direction = ropeEnd.position - ropeStart.position;
		var magnitude = direction.magnitude;
		cylinder.GetUpdated(direction, magnitude);
	}

	public void ReturnHome()
	{
		transform.DOLocalRotateQuaternion(_initLocalRot, 0.2f);
		ropeEnd.DOLocalMove(Vector3.zero, 0.2f)
			.OnUpdate(UpdateRope);
	}
}
