using System.Linq;
using MinD.Runtime.Object.Interactables;

namespace MinD.Runtime.Managers {

public class WorldDataManager : Singleton<WorldDataManager> {
	
	public GuffinsAnchor[] worldAnchors;



	public void Awake() {
		
		worldAnchors = FindObjectsOfType<GuffinsAnchor>();
		worldAnchors.OrderBy(i => i.anchorInfo.anchorId);

	}

}

}