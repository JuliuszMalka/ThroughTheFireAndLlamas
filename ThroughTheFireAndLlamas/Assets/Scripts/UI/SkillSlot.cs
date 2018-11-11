using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, ISlots {

	public Skill skill = null;
	public Image slotIcon = null;
	
	void Awake() {
		//slotIcon = gameObject.GetComponentInChildren<Image>();
		slotIcon.enabled = false;
	}

	public void AddSkill(Skill _skill) {
		this.skill = _skill;
		this.slotIcon.sprite = _skill.skillIcon;
		this.slotIcon.enabled = true;
	}

	public void Clear() {
		this.skill = null;
		this.slotIcon.sprite = null;
		this.slotIcon.enabled = false;
	}

}
