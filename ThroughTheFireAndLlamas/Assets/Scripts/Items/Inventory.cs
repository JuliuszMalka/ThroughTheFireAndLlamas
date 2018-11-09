using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	public static Inventory instance = null;

	public RectTransform invParent = null;
	public RectTransform equipmentParent = null;

	public InventorySlot[] invSlots = null;
	public InventorySlot[] equipmentSlots = null;

	public Button[] slotButtons = null;
	public Button[] equipmentButtons = null;
	
	public int rows = 0;
	public int columns = 0;

	void Awake() {
		columns = rows = 6;
		slotButtons = invParent.gameObject.GetComponentsInChildren<Button>();
		equipmentButtons = equipmentParent.gameObject.GetComponentsInChildren<Button>();
		foreach (InventorySlot slot in invSlots) {
			slot.Clear();
		}
	}

	public bool AddToInventory(GameObject obj) {
		if (this.IsFull()) {
			Debug.LogError("Inventory full!");
			return false;
		}
		foreach (InventorySlot slot in invSlots) {
			if (slot.item == null) {
				slot.Add(obj);
				break;
			}
		}
		return true;
	}

	public bool IsEmpty() {
		foreach (InventorySlot slot in invSlots) {
			if (slot.item != null) return false;
		}
		return true;
	}

	public bool IsFull() {
		foreach (InventorySlot slot in invSlots) {
			if (slot.item == null) return false;
		}
		return true;
	}

}
