using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Skill {

	public override void Allocate() {
		base.Allocate();
	}	

	public override void UseSkill(Pair<Vector3, Vector3> coords) {
		Vector3 direction = coords.GetFirstValue() - coords.GetSecondValue();
		GameObject fireballPrefab = Instantiate(
			Resources.Load("fireball"),
			coords.GetFirstValue(),
			Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x), Vector3.up)
		) as GameObject;
	}
}
