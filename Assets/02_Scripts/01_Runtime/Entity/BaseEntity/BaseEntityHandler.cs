using UnityEngine;

namespace MinD.Runtime.Entity {

public class BaseEntityHandler<TOwner> : MonoBehaviour where TOwner : BaseEntity {
	
	protected TOwner owner {
		get {
			if (ownerEntity == null) {
				ownerEntity = GetComponent<TOwner>();
			}
			return ownerEntity;
		}
	}
	private TOwner ownerEntity;
}

}