using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class ItemManager : MonoBehaviour {

	public enum DamageType { //typ obrażeń fizycznych zadawanych przez broń
		Blunt = 0,
		Slash,
		Thrust
	}

	public enum ElementalDamageType { //typ obrażeń od elementów (jeśli broń takowe posiada)
		None = -1,
		Fire,
		Cold,
		Lightning,
		Acid
	}

	public enum ItemRarity { //rzadkość przedmiotu
		Normal = 1,
		Magic,
		Rare,
		Legendary
	}

	public enum ArmorType {//typ armora
		Helmet = 0,
		Body,
		Legs,
		Gloves
	}

	public struct WeaponData { //struktura od wartości w broniach które nie będą się zmieniać
		public uint weight;
		public uint basePrice;
		public float range;
		public Sprite weaponSprite;
		public WeaponData(uint weight, uint basePrice, float range) {
			this.range = range;
			this.weight = weight;
			this.basePrice = basePrice;
			this.weaponSprite = null;
		}
		public WeaponData(WeaponData baseObj) {
			this.range = baseObj.range;
			this.weight = baseObj.weight;
			this.basePrice = baseObj.basePrice;
			this.weaponSprite = baseObj.weaponSprite;
		}
	}
	
	public struct ArmorData {//same as above, ale dla armorów
		public uint weight;
		public uint basePrice;
		public Sprite armorSprite;
		public ArmorData(uint weight, uint basePrice) {
			this.weight = weight;
			this.basePrice = basePrice;
			this.armorSprite = null;
		}
		public ArmorData(ArmorData baseObj) {
			this.weight = baseObj.weight;
			this.basePrice = baseObj.basePrice;
			this.armorSprite = baseObj.armorSprite;
		}
	}

	public struct SerializableWeaponData {//jako że nie da się serializować sprite'ów, to używamy tych struktur do serializacji danych o itemach
		public uint weight;
		public uint basePrice;
		public float range;
	}

	public struct SerializableArmorData {
		public uint weight;
		public uint basePrice;
	}
	
	public Dictionary<string, List<string>> prefixesAndSuffixes = new Dictionary<string, List<string>>(); //prefixy i suffixy do broni
	public Dictionary<DamageType, List<string>> itemNames = new Dictionary<DamageType, List<string>>();//nazwy broni dla każdego typu
	public Dictionary<string, WeaponData> dataOfWeapons = new Dictionary<string, WeaponData>();//dane dla każdej z broni
	public Dictionary<string, ArmorData> dataOfArmors = new Dictionary<string, ArmorData>();//dane dla każdego z armor piece'ów
	//public Dictionary<DamageType, Dictionary<string, Dictionary<string, List<string>>>> itemNames = new Dictionary<DamageType, Dictionary<string, Dictionary<string, List<string>>>>();
	void Awake() {
		PlayerStatistics.GetInstance().GetWeaponSkills();
	}

	void Start () {
		DontDestroyOnLoad(this.gameObject);
	}

	void SaveItemData() {

		Dictionary<string, SerializableWeaponData> tempWepData = new Dictionary<string, SerializableWeaponData>();
		Dictionary<string, SerializableArmorData> tempArmData = new Dictionary<string, SerializableArmorData>();

		foreach (var key in dataOfWeapons.Keys.ToArray()) {//
			SerializableWeaponData temp = new SerializableWeaponData();
			temp.range = dataOfWeapons[key].range;
			temp.weight = dataOfWeapons[key].weight;
			temp.basePrice = dataOfWeapons[key].basePrice;
			tempWepData[key] = temp;
		}

		foreach (var key in dataOfArmors.Keys.ToArray()) {
			SerializableArmorData temp = new SerializableArmorData();
			temp.weight = dataOfArmors[key].weight;
			temp.basePrice = dataOfArmors[key].basePrice;
			tempArmData[key] = temp;
		}

		FileStream fs = new FileStream("JsonFiles/weaponData.json", FileMode.OpenOrCreate);

		using (StreamWriter writer = new StreamWriter(fs)) {
			writer.WriteLine(JsonConvert.SerializeObject(tempWepData, Formatting.Indented));
		}

		fs = new FileStream("JsonFiles/armorData.json", FileMode.OpenOrCreate);

		using (StreamWriter writer = new StreamWriter(fs)) {
			writer.WriteLine(JsonConvert.SerializeObject(tempArmData, Formatting.Indented));
		}
	}

	void LoadItemData() {
		Dictionary<string, SerializableWeaponData> tempWepData = new Dictionary<string, SerializableWeaponData>();
		Dictionary<string, SerializableArmorData> tempArmData = new Dictionary<string, SerializableArmorData>();
		using (StreamReader reader = new StreamReader("JsonFiles/weaponData.json")) {
			tempWepData = JsonConvert.DeserializeObject<Dictionary<string, SerializableWeaponData>>(reader.ReadToEnd());
		}
		using (StreamReader reader = new StreamReader("JsonFiles/armorData.json")) {
			tempArmData = JsonConvert.DeserializeObject<Dictionary<string, SerializableArmorData>>(reader.ReadToEnd());
		} 

		foreach (var key in tempWepData.Keys.ToArray()) {
			WeaponData temp = new WeaponData(tempWepData[key].weight, tempWepData[key].basePrice, tempWepData[key].range);
			dataOfWeapons[key] = temp;
		}

		foreach (var key in tempArmData.Keys.ToArray()) {
			ArmorData temp = new ArmorData(tempArmData[key].weight, tempArmData[key].basePrice);
			dataOfArmors[key] = temp;
		}

		var sprites = Resources.LoadAll("Sprites", typeof(Sprite));

		foreach (var x in sprites) {
			foreach (var key in dataOfWeapons.Keys.ToArray()) {
				if (key == x.name) {
					WeaponData temp = new WeaponData(dataOfWeapons[key]);
					temp.weaponSprite = (Sprite)x;
					dataOfWeapons[key] = temp;
				}
			}
		}

		foreach (var x in sprites) {
			foreach (var key in dataOfArmors.Keys.ToArray()) {
				if (key == x.name) {
					ArmorData temp = new ArmorData(dataOfArmors[key]);
					temp.armorSprite = (Sprite)x;
					dataOfArmors[key] = temp;
				}
			}
		}
	}

	void LoadFromJson() {
		using (StreamReader reader = new StreamReader("JsonFiles/weaponNames.json")) {
			itemNames = JsonConvert.DeserializeObject<Dictionary<DamageType, List<string>>>(reader.ReadToEnd());
		}
		using (StreamReader reader = new StreamReader("JsonFiles/prefixesAndSuffixes.json")) {
			prefixesAndSuffixes = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(reader.ReadToEnd());
		}
	}

	void CreateWeaponData() { /*zrobić rozpiskę i wrzucić do pliku tekstowego, potem będziemy z niego wczytywać */
		int lineCount = CountFileLines("WeaponGeneration/itemData.txt");
		WeaponData[] data = new WeaponData[lineCount];
		var sprites = Resources.LoadAll("Sprites", typeof(Sprite));
		using (StreamReader reader = new StreamReader("WeaponGeneration/itemData.txt")) {
			for (int i = 0; i < lineCount; ++i) {
				string[] temp = reader.ReadLine().Split();
				data[i].weight = System.Convert.ToUInt32(temp[1]);
				data[i].basePrice = System.Convert.ToUInt32(temp[2]);
				data[i].range = System.Convert.ToSingle(temp[3]);
				dataOfWeapons.Add(temp[0], data[i]);
				for (int j = 0; j < sprites.Length; ++j) {
					foreach (string key in dataOfWeapons.Keys.ToList()) {
						if (key == sprites[i].name) {
							WeaponData tempData = dataOfWeapons[key];
							tempData.weaponSprite = (Sprite)sprites[i];
							dataOfWeapons[key] = tempData;
						}
					}
				}
			}
		}
	}

	void CreateArmorData() {
		int lineCount = CountFileLines("WeaponGeneration/armorData.txt");
		ArmorData[] data = new ArmorData[lineCount];
		var sprites = Resources.LoadAll("Sprites", typeof(Sprite));
		using (StreamReader reader = new StreamReader("WeaponGeneration/armorData.txt")) {
			for (int i = 0; i < lineCount; ++i) {
				string[] temp = reader.ReadLine().Split();
				data[i].weight = System.Convert.ToUInt32(temp[1]);
				data[i].basePrice = System.Convert.ToUInt32(temp[2]);
				dataOfArmors.Add(temp[0], data[i]);
				for (int j = 0; j < sprites.Length; ++j) {
					foreach (string key in dataOfArmors.Keys.ToList()) {
						if (key == sprites[i].name) {
							ArmorData tempData = dataOfArmors[key];
							tempData.armorSprite = (Sprite)sprites[i];
							dataOfArmors[key] = tempData;
						}
					}
				}
			}
		}
	}

	void CreateDictionary() {
		DamageType[] damageTypeNames = (System.Enum.GetValues(typeof(DamageType))) as DamageType[];

		var files = Directory.GetFiles("WeaponNames");
		int fileCount = files.Length, namesIndex = 0;

		for (int i = 0; i < fileCount; ++i) {
			List<string> names = new List<string>();
			int lineCount = CountFileLines(files[i]);
			using (StreamReader reader = new StreamReader(files[i])) {
				for (int j = 0; j < lineCount; ++j) {
					names.Add(reader.ReadLine());
				}
			}
			itemNames.Add(damageTypeNames[namesIndex++], names);
		}
	}

	void SaveToJson() {
		using (StreamWriter writer = new StreamWriter("JsonFiles/prefixesAndSuffixes.json")) {
			writer.WriteLine(JsonConvert.SerializeObject(prefixesAndSuffixes, Formatting.Indented));
		}
		using (StreamWriter writer = new StreamWriter("JsonFiles/weaponNames.json")) {
			writer.WriteLine(JsonConvert.SerializeObject(itemNames, Formatting.Indented));
		}
	}

	void PrefixesAndSuffixesInit() {
		prefixesAndSuffixes["prefix"] = new List<string>();
		prefixesAndSuffixes["suffix"] = new List<string>();
		prefixesAndSuffixes["legendaries"] = new List<string>();
		using (StreamReader reader = new StreamReader("WeaponGeneration/prefixes.txt")) {
			while (!reader.EndOfStream) prefixesAndSuffixes["prefix"].Add(reader.ReadLine());
		}
		using (StreamReader reader = new StreamReader("WeaponGeneration/suffixes.txt")) {
			while (!reader.EndOfStream) prefixesAndSuffixes["suffix"].Add(reader.ReadLine());
		}
		using (StreamReader reader = new StreamReader("WeaponGeneration/legendaries.txt")) {
			while (!reader.EndOfStream) prefixesAndSuffixes["legendaries"].Add(reader.ReadLine());
		}
	}

	int CountFileLines(string filename) {
		int count = 0;
		using (StreamReader reader = new StreamReader(filename)) {
			while (!reader.EndOfStream) {
				++count; 
				reader.ReadLine();
			}
		}
		return count;
	}

	string GenerateWeaponName(DamageType typeOfDamage) {
		string weaponName = "";
		weaponName += prefixesAndSuffixes["prefix"][Random.Range(0, prefixesAndSuffixes["prefix"].Count)];
		weaponName += " ";
		weaponName += itemNames[typeOfDamage][Random.Range(0, itemNames[typeOfDamage].Count)];
		weaponName += " ";
		weaponName += prefixesAndSuffixes["suffix"][Random.Range(0, prefixesAndSuffixes["suffix"].Count)];
		return weaponName;
	}

	public GameObject GenerateWeapon() { //dorobić: generowanie sell price'a, obrażenia względem itemlevela/levelTaga
		GameObject weapon = new GameObject(); //tworzymy pusty obiekt na scenie
		SpriteRenderer weaponSpriteRenderer = weapon.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;//dodajemy sprite renderer, żeby móc dodać sprite'a
		Weapon weaponScript = weapon.AddComponent(typeof(Weapon)) as Weapon;//dodajemy skrypt od broni

		int rarity = (int)(Random.value * 100f);
		if (rarity > 35) weaponScript.rarityOfItem = ItemRarity.Normal;
		else if (rarity <= 35 && rarity > 15) weaponScript.rarityOfItem = ItemRarity.Magic;
		else if (rarity <= 15 && rarity > 3) weaponScript.rarityOfItem = ItemRarity.Rare;
		else weaponScript.rarityOfItem = ItemRarity.Legendary; 

		//weaponScript.GenerateSellPrice();
		weaponScript.damageType = (DamageType)Random.Range(0, System.Enum.GetNames(typeof(DamageType)).Length);

		string weaponName = GenerateWeaponName(weaponScript.damageType);

		//weaponSpriteRenderer.sprite = weaponSprites[weaponName];
		weaponScript.itemName = weaponName;
		weapon.name = weaponName;

		int elementalDamageChance = Random.Range(1, 101);
		if (elementalDamageChance < 25) {
			weaponScript.elemType = (ElementalDamageType)Random.Range(0, System.Enum.GetNames(typeof(ElementalDamageType)).Length - 1);
		} 
		weapon.SetActive(false);
		return weapon;
	}
}
