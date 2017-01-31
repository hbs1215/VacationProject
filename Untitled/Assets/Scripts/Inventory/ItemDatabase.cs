using UnityEngine;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {
	public static ItemDatabase instance;
	public List<Item> items = new List<Item>();

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
		Add("axe",1,500, "Good Axe", ItemType.Equipment);
		Add("apple",1,50, "Delicious Apple", ItemType.Consumption);
	}  

	void Add(string itemName,int itemValue,int itemPrice, string itemDesc, ItemType itemType)
	{
		items.Add(new Item(itemName,itemValue,itemPrice, itemDesc, itemType,Resources.Load<Sprite>("ItemImage/" + itemName)));
	}
}