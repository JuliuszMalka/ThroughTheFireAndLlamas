using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISlots {}

public class UIManager : MonoBehaviour {

	public Transform player = null;

	public static UIManager instance = null;

	public RectTransform skillSlotsParent = null;
	public RectTransform itemSlotsParent = null;

	public SkillSlot[] skillSlots = null;
	public ItemSlot[] itemSlots = null;

	public ItemSlot activeSlot = null;

	public int currentActiveIndex = 0;

	void Awake() {
		skillSlots = skillSlotsParent.gameObject.GetComponentsInChildren<SkillSlot>();	
		itemSlots = itemSlotsParent.gameObject.GetComponentsInChildren<ItemSlot>();

		skillSlotsParent.gameObject.SetActive(false);
		itemSlotsParent.gameObject.SetActive(true);

		activeSlot = itemSlots[0];
		currentActiveIndex = 0;

		player = GameObject.FindWithTag("Player").transform;
		foreach (var slot in skillSlots) {
			slot.Clear();
		}
		foreach (var slot in itemSlots) {
			slot.Clear();
		}
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Tab)) {
			SwapUIBars();
		}

		if (Input.GetKeyDown(KeyCode.E) && itemSlotsParent.gameObject.activeSelf) {
			if (currentActiveIndex == itemSlots.Length) currentActiveIndex = 0;
			activeSlot = itemSlots[currentActiveIndex++];
			Debug.Log(activeSlot.name);
		}

		if (Input.GetKeyDown(KeyCode.Q) && itemSlotsParent.gameObject.activeSelf) {
			if (currentActiveIndex < 0) currentActiveIndex = itemSlots.Length - 1;
			activeSlot = itemSlots[currentActiveIndex--];
			Debug.Log(activeSlot.name);
		}

		if (skillSlotsParent.gameObject.activeSelf) {
			int action = 0;
			string temp = Input.inputString;
			if (!string.IsNullOrEmpty(temp)) {
				if (System.Int32.TryParse(temp, out action)) {
					if (action >= 1 && action <= 9) SendActionToPlayer(action);
				}
			}
		}

		if (itemSlotsParent.gameObject.activeSelf) {
			int action = 0;
			string temp = Input.inputString;
			if (!string.IsNullOrEmpty(temp)) {
				if (System.Int32.TryParse(temp, out action)) {
					if (action >= 1 && action <= 9) activeSlot = itemSlots[action - 1];
					Debug.Log(activeSlot.name);
				}
			}
		}
	}

	void SwapUIBars() {
		skillSlotsParent.gameObject.SetActive(!skillSlotsParent.gameObject.activeSelf);
		itemSlotsParent.gameObject.SetActive(!itemSlotsParent.gameObject.activeSelf);

		if (itemSlotsParent.gameObject.activeSelf) {
			activeSlot = itemSlots[currentActiveIndex];
		}
	}

	void SendActionToPlayer(int ID) {
		PlayerBehaviour temp = player.gameObject.GetComponent<PlayerBehaviour>();
		if (temp.action) return;
		if (skillSlots[ID - 1].skill == null) {
			Debug.LogWarning("Slot is empty");
			return;
		}
		temp.action = true;
		temp.skillID = skillSlots[ID - 1].skill.ID;
		Debug.Log(temp.skillID);
	}

	bool IsFull(SkillSlot[] array) {
		foreach (SkillSlot slot in array) {
			if (slot.skill == null) return false;
		}
		return true;
	}

	bool IsFull(ItemSlot[] array) {
		foreach (ItemSlot slot in array) {
			if (slot.item == null) return false;
		}
		return true;
	}

	public void AddSkill(Skill skill) {
		if (!IsFull(skillSlots)) {
			foreach (SkillSlot slot in skillSlots) {
				if (slot.skill == null) {
					slot.AddSkill(skill);
					break;
				}
			}
		}
	}

	public void AddItem(GameObject obj) {
		if (!IsFull(itemSlots)) {
			foreach (ItemSlot slot in itemSlots) {
				if (slot.item == null) {
					slot.AddItem(obj);
					break;
				}
			}
		}
	}
}
