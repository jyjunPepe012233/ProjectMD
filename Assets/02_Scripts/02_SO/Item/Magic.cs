using UnityEngine;
using UnityEngine.InputSystem.Users;

namespace MinD {

	public abstract class Magic : Item {
		
		[Space(20)]
		
		[Header("[ Magic Status ]")]
		[Range(1, 3)] public int memoryCost = 1;
		public int mpCost;
		public int staminaCost;
		
		[Header("[ Optional Status ]")]
		public int mpCostDuring;
		public int staminaCostDuring;

		[HideInInspector] public Player castPlayer;
		

		public abstract void OnUse();
		// WRITE THE CODE WHEN RUNNING MAGIC IS STARTED
		
		public abstract void Tick();
		// WRITE THE CODE WHEN RUNNING IN EVERY FRAME
		
		public abstract void OnReleaseInput();
		// WRITE THE CODE WHEN RUNNING IF PLAYER'S USE MAGIC INPUT IS ENDED

		public abstract void OnExit();
		// WRITE THE CODE WHEN RUNNING MAGIC IS PERFECTLY END
		// AND IF YOU WANT TO END THE MAGIC, USE EndCurrentMagic FUNCTION IN PLAYER COMBAT HANDLER
		// THAT FUNCTION WILL CALL THIS FUNCTION
		
		
		
		public virtual void InstantiateWarmupFX() {}
		
		public virtual void OnSuccessfullyCast() {}
		
		public virtual void OnCastIsEnd() {}
		

	}

}