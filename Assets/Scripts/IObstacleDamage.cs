using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObstacleDamage
{
	void DamageThePlayer()
	{
		PlayerMovementControl.Instance.MakePlayerShort(1);
		PlayerMovementControl.Instance.DeBuffThePlayer(1);
	}
}
