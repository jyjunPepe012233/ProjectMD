using UnityEngine;

namespace MinD.Runtime.Entity {

public class EnemyEquipmentHandler : MonoBehaviour {

	[HideInInspector] public Enemy owner;
	
	[SerializeField] private GameObject[] equipments;

	

	public void SetActiveEquipment(string equipmentName, bool active) {

		foreach (GameObject equipment in equipments) {

			if (equipment.name.Equals(equipmentName))
				equipment.SetActive(active);

		}

	}


	/// <param name="Equipment Name + active boolean"></param>
	public void SetActiveEquipmentToOneString(string parameter) {

		string[] paramTemp = parameter.Split();

		foreach (GameObject equipment in equipments) {

			if (equipment.name.Equals(paramTemp[0]))
				equipment.SetActive(paramTemp[1] == "true");

		}

	}

}

}