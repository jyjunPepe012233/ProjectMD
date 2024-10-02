using System.Collections;
using System.Collections.Generic;
using MinD;
using UnityEngine;

public class ItemObject : MonoBehaviour {

	public bool isPickable = true;

	[Space(10)]
	public Item item;
	public int itemCount = 1;
}
