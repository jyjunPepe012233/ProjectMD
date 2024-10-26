using UnityEngine;

namespace MinD.Runtime.DataBase {

public class ObjectDataBase : Singleton<ObjectDataBase> {

	[Header("[ Magics ]")]
	public GameObject[] magics;

	
	public GameObject InstantiateMagic(string magicName) {

		foreach (GameObject obj in magics) {

			if (obj.name == magicName)
				return Instantiate(obj);
		}

		Debug.LogError("!! CAN'T FIND OBJECT NAMED " + magicName + " IN OBJECT DATABASE !!");
		return null;

	}


}

}