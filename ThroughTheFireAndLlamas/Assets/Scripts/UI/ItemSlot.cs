using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, ISlots {

	public GameObject item = null;
	public Image itemIcon = null;

	void Awake() {
		//itemIcon = gameObject.GetComponentInChildren<Image>();
		itemIcon.enabled = false;
	}

	public void AddItem(GameObject obj) {
		this.item = obj;
		this.itemIcon.sprite = obj.GetComponent<SpriteRenderer>().sprite;
		this.itemIcon.enabled =true;
	}

	public void Clear() {
		this.item = null;
		this.itemIcon.sprite = null;
		this.itemIcon.enabled = false;
	}
}
