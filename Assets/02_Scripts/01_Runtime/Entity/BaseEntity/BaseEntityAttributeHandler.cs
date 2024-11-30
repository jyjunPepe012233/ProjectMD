using MinD.Structs;
using UnityEngine;

namespace MinD.Runtime.Entity {

public abstract class BaseEntityAttributeHandler<TOwner> : BaseEntityHandler<TOwner> where TOwner : BaseEntity {

	public abstract int MaxHp { get; set; }
	public abstract DamageNegation DamageNegation { get; set; }
	public abstract int PoiseBreakResistance { get; set; }
	
}

}