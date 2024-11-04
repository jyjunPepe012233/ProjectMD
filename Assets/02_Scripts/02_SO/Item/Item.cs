using MinD.Enums;
using UnityEngine;

namespace MinD.SO.Item {

public abstract class Item : ScriptableObject {

	[HideInInspector] public int itemId; // ITEM'S ID IS GENERATE AUTOMATICALLY IN ItemDataList

	
	[Header("[ Setting ]")]
	public string itemName;
	[TextArea(20, 20)] public string itemDescription;
	
	[Space(5)] public ItemRarityEnum itemRarity;
	[Space(5)] public int itemMaxCount = 1;

	
	[Header("[ Runtime Data ]")]
	public int itemCount = 0;

	public Sprite itemImage;
	
	[Header("[ IDs ]")]
	[HideInInspector] public int slotId;        // 슬롯 ID
	[HideInInspector] public int categoryId;    // 카테고리 ID
}

}