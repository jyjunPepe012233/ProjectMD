using MinD.Runtime.Entity;

namespace MinD.Runtime.Managers {

public class PlayerManager : Singleton<PlayerManager> {

	public Player currentPlayerCharacter {
		get {
			if (currentPlayerCharacter_ == null ||
			    !currentPlayerCharacter.gameObject.activeSelf)
				// IF CURRENT PLAYER CHARACTER HASN'T BEEN ASSIGNED OR PLAYER IS MISSING
				currentPlayerCharacter_ = FindObjectOfType<Player>();
			
			return currentPlayerCharacter_;
		}
	}
	private Player currentPlayerCharacter_;


	

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