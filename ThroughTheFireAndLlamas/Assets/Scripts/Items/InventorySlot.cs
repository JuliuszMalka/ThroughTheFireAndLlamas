using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

	public GameObject item = null;
	public Image itemIcon = null;
	public Button itemButton = null;
	public bool isEquipmentSlot = false;

	public void Add(GameObject toAdd) {
		itemIcon = item.GetComponent<Image>();
		item = toAdd;
	}
	public void Clear() {
		itemIcon = null;
		item = null;
	}
}
