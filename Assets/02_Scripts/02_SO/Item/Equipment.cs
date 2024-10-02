using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : Item {

	public abstract void OnEquip(Player owner);

	public abstract void Execute(Player owner);

	public abstract void OnUnequip(Player owner);

}