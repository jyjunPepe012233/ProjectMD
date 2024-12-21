using System.Linq;
using MinD.Runtime.Object.Interactables;
using UnityEngine;

namespace MinD.Runtime.Managers {

public class WorldDataManager : Singleton<WorldDataManager> {
	
	public GuffinsAnchor[] worldAnchors;
	


	public void Awake() {

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		
		worldAnchors = FindObjectsOfType<GuffinsAnchor>();
		worldAnchors.OrderBy(i => i.anchorInfo.anchorId);
		for (int i = 0; i < worldAnchors.Length; i++) {
			worldAnchors[i].LoadGuffinsAnchorData(false); // NEED FIX THIS WITH SAVE DATA DEVELOPING
		}
		
	}

}

}