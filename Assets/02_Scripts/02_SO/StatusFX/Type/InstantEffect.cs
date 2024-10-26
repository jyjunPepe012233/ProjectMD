using MinD.Enums;
using MinD.Runtime.Entity;
using UnityEngine;

namespace MinD.SO.StatusFX {

public abstract class InstantEffect : ScriptableObject {

	public InstantEffectType enumId;

	public void OnInstantiate(BaseEntity owner) {

		if (owner is Player player) {
			OnInstantiateAs(player);
		}

		if (owner is Enemy enemy)
			OnInstantiateAs(enemy);

	}

	protected abstract void OnInstantiateAs(Player target);
	protected abstract void OnInstantiateAs(Enemy target);

}

}