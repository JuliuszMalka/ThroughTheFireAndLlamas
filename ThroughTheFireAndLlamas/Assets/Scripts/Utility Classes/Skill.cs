using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour {

	public bool isAllocated = false;
	public List<Skill> neighbourNodes = new List<Skill>();
	public string ID = "default";
	public Sprite skillIcon = null;

	// public void PlayerReferenceInit(Transform _player) {
	// 	player = _player;
	// }

	public virtual void Allocate() {
		this.isAllocated = true;
		PlayerStatistics.GetInstance().AddSkill(UseSkill, ID);
		UIManager.instance.AddSkill(this);
	}

	public virtual void UseSkill(Pair<Vector3, Vector3> coords) {
		Debug.Log(coords.GetFirstValue().ToString() + ", " + coords.GetSecondValue().ToString());
	}
}
