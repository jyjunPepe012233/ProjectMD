using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Item Data List", menuName = "MinD/Item/SO List", order = int.MinValue)]
public class ItemSOList : ScriptableObject {

	public List<Weapon> weaponList;

}