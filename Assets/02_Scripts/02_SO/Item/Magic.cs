using UnityEngine;
using UnityEngine.InputSystem.Users;

namespace MinD {

	public abstract class Magic : Item {
		
		[Header("[ Magic Status ]")]
		public int memoryCost = 1;
		public int mpCost;
		public int staminaCost;

		[Header("[ Action Flag Setting ]")]
		public bool isPerformingAction = true;
		public bool applyRootMotion = true;
		public bool canRotate = false;
		public bool canMove = false;
		

		public abstract void OnUse(Player player);

		public virtual void InstantiateMagicObject(Player player) {
		}
		
		public virtual void InstantiateWarmupFX(Player player) {
		}

	}

}