using UnityEngine;
using UnityEngine.InputSystem.Users;

namespace MinD {

	public abstract class Magic : Item {

		public int memoryCost;
		public int mpCost;

		public abstract void OnUse();

	}

}