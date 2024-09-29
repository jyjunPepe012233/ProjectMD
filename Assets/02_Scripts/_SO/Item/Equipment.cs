using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : Item {

	public abstract void OnEquip();

	public abstract void Execute();

	public abstract void OnUnequip();

}