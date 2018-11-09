using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Skill : ScriptableObject {

	public bool isAllocated = false;
	public Transform player = null;
	public List<Skill> neighbourNodes = new List<Skill>();
	public short ID = 0;

	public void PlayerReferenceInit(Transform _player) {
		player = _player;
	}

	public virtual void Allocate() {
		this.isAllocated = true;
		PlayerStatistics.GetInstance().AddSkill(UseSkill, "default");
	}

	public virtual void UseSkill(Pair<Vector3, Vector3> coords) {

	}
}
