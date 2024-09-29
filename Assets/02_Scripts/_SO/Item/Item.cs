using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Item : ScriptableObject {

	[Header("[ Setting ]")]
	public string itemName;
	[TextArea(20, 20)] public string itemDescription;
	[Space(5)]
	public int itemMaxCount;

	[Header("[ Runtime Data ]")]
	[HideInInspector] public int itemId; // ITEM'S ID IS GENERATE AUTOMATICALLY IN ItemDataList
	public int itemCount = 0;

}
