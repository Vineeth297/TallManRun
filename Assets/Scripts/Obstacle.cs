using UnityEngine;

public class Obstacle : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<PlayerMovementControl>().ShrinkThePlayer(10);
		}
	}
}
