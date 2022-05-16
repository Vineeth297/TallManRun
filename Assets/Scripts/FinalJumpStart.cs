using DG.Tweening;
using UnityEngine;

public class FinalJumpStart : MonoBehaviour
{
	[SerializeField] private Transform jumpTarget;
	
	private Camera _camera;
	private void Start()
	{
		_camera= Camera.main;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player")) return;
		
		PlayerMovementControl.Instance.PlayKickAnimation();
		PlayerMovementControl.Instance.xSpeed = 0f;
		_camera.transform.DORotate(new Vector3(42f, 0f, 0f), 2f);
		_camera.transform.DOMoveX(0f, 1f);
		_camera.transform.DOMoveY(_camera.transform.position.y + 13f, 1f);
		
		print(other.transform.DOKill(false));
		
		PlayerMovementControl.Instance.JumpKarao(jumpTarget.position);
		
		// other.GetComponent<PlayerMovementControl>().lengthEffector.transform.DOJump(jumpTarget.position, 5f, 1, 3f).SetEase(Ease.InCubic);
	}
}
