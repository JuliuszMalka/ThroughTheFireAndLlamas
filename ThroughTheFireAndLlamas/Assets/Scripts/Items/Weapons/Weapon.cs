using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public enum WeaponType {
		Axe = 0,
		Club,
		Dagger,
		Hammer,
		Spear,
		Staff,
		Sword
	}

	public enum DexScaling {
		E = 1,
		D,
		C,
		B,
		A,
		S
	}

	public enum StrScaling {
		E = 1,
		D,
		C,
		B,
		A,
		S
	}


	public DamageRange<int> damageRange = new DamageRange<int>(0, 0);
	public DamageRange<int> elementalDamageRange = new DamageRange<int>(0, 0);
	public float attackSpeed 		= 0f;
	public int sellPrice 			= 0;
	public int itemLevel 			= 0;
	public int criticalStrikeChance = 0;
	public float criticalStrikeMultiplier = 0f;
	public string baseItemName 		= null;
	public string itemName 			= null;
	public ItemManager.DamageType damageType;
	public ItemManager.ElementalDamageType elemType = ItemManager.ElementalDamageType.None;
	public ItemManager.ItemRarity rarityOfItem;
	public ItemManager.WeaponData data = new ItemManager.WeaponData();
	


	public void PickUp() {
		Inventory.instance.AddToInventory(this.gameObject);
		gameObject.SetActive(false);
	}

	public static void AxeSkill(Vector3 direction) {
		Debug.Log("Invoked that method");
		Debug.Log("Parameter: " + direction.ToString());
	}

	public static void ClubSkill(Vector3 direction) {
		
	}

	public static void DaggerSkill(Vector3 direction) {

	}

	public static void HammerSkill(Vector3 direction) {
		
	}

	public static void SpearSkill(Vector3 direction) {
		
	}

	public static void StaffSkill(Vector3 direction) {
		
	}

	public static void SwordSkill(Vector3 direction) {
		
	}

	

	
}
