using MinD.Runtime.Entity;

namespace MinD.Runtime.Managers {

public class PlayerManager : Singleton<PlayerManager> {

	public static Player currentPlayerCharacter {
		get {
			if (currentPlayerCharacter_ == null)
				currentPlayerCharacter_ = FindObjectOfType<Player>();

			// IF CURRENT PLAYER CHARACTER HASN'T BEEN ASSIGNED OR PLAYER IS MISSING
			else if (currentPlayerCharacter_.gameObject.activeSelf) {
				currentPlayerCharacter_ = FindObjectOfType<Player>();
			}
			
			return currentPlayerCharacter_;
		}
	}
	private static Player currentPlayerCharacter_;


	

	public void RefreshPlayer() {
		
		// TODO: SAVE PLAYER DATA


		var player = currentPlayerCharacter;
		
		// TODO: LOAD ATTRIBUTE FROM SAVE DATA
		
		player.attribute.SetBaseAttributesAsPerStats();
		player.attribute.CalculateAttributesByEquipment();
		RestorePlayer();
		
		
		// TODO: LOAD INVENTORY FROM SAVE DATA
		
	}

	private void RestorePlayer() {
		
		var player = currentPlayerCharacter_;

		player.CurHp = player.attribute.maxHp;
		player.CurMp = player.attribute.maxMp;
		player.CurStamina = player.attribute.maxStamina;

	}
	
}

}