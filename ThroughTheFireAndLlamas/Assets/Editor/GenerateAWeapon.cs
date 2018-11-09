using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateAWeapon : EditorWindow {

	Weapon wep = null;
	string wepName = null;
	ItemManager.DamageType typeOfDamage;
	int minDamage = 0, maxDamage = 0;
	int price = 0;
	int criticalStrikeChance = 0;
	float criticalStrikeMultiplier = 0f;
	int itemLevel = 0;
	ItemManager.ItemRarity rarity = ItemManager.ItemRarity.Normal;
	ItemManager.ElementalDamageType elemType = ItemManager.ElementalDamageType.None;
	int minElemDamage = 0, maxElemDamage = 0;
	Vector3 spawnPos = Vector3.zero;
	//bool hasElemDamage = false;

	[MenuItem("LlamaTools/Create a Weapon")]
	public static void ShowWindow() {
		EditorWindow.GetWindow(typeof(GenerateAWeapon));
	}

	void OnGUI() {
		GUILayout.Label("Weapon Settings", EditorStyles.boldLabel);
		wepName = EditorGUILayout.TextField("Weapon's name", wepName);
		typeOfDamage = (ItemManager.DamageType)EditorGUILayout.EnumPopup("Weapon's damage type", typeOfDamage);
		minDamage = EditorGUILayout.IntField("Minimal damage", minDamage);
		maxDamage = EditorGUILayout.IntField("Maximum DAKKA", maxDamage);
		elemType = (ItemManager.ElementalDamageType)EditorGUILayout.EnumPopup("Weapon's elemental damage type", elemType);
		if (elemType != ItemManager.ElementalDamageType.None) {
			minElemDamage = EditorGUILayout.IntField("Minimal elemental damage", minElemDamage);
			maxElemDamage = EditorGUILayout.IntField("Maximum elemental DAKKA", maxElemDamage);
		}
		rarity = (ItemManager.ItemRarity)EditorGUILayout.EnumPopup("Weapon's rarity", rarity);
		price = EditorGUILayout.IntField("Weapon's Price", price);
		spawnPos = EditorGUILayout.Vector3Field("spawn position", spawnPos);
		if (GUILayout.Button("Generate!")) {
			GameObject spawned = AssignValues();
			spawned.transform.position = spawnPos;
		}
	}

	GameObject AssignValues() {
		GameObject temp = new GameObject();
		wep = temp.gameObject.AddComponent(typeof(Weapon)) as Weapon;
		wep.itemName = wepName;
		wep.sellPrice = price;
		wep.rarityOfItem = rarity;
		wep.elemType = elemType;
		if (elemType != ItemManager.ElementalDamageType.None) {
			wep.elementalDamageRange = new DamageRange<int>(minElemDamage, maxElemDamage);
		}
		wep.damageRange = new DamageRange<int>(minDamage, maxDamage);
		wep.damageType = typeOfDamage;
		return temp;
	}

}
