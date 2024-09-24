using MinD.StatusFx;
using UnityEngine;


public class WorldStatusFxManager : Singleton<WorldStatusFxManager> {

	private StatusEffectDataList effectData;
	public StatusEffectDataList EffectData => effectData;
	

	public InstantEffect InstantiateInstantEffect(InstantEffectType type) {

		foreach (InstantEffect effect in effectData.instantEffects) {

			if (effect.enumId == type)
				return Instantiate(effect);
		}

		Debug.LogError("Can't Find Effect In List As Parameter Type");
		return null;
	}
	
	public StaticEffect InstantiateStaticEffect(StaticEffectType type) {

		foreach (StaticEffect effect in effectData.staticEffects) {

			if (effect.enumId == type)
				return Instantiate(effect);
		}

		Debug.LogError("Can't Find Effect In List As Parameter Type");
		return null;
	}
	
	public TimedEffect InstantiateTimedEffect(TimedEffectType type) {

		foreach (TimedEffect effect in effectData.timedEffects) {

			if (effect.enumId == type)
				return Instantiate(effect);
		}

		Debug.LogError("Can't Find Effect In List As Parameter Type");
		return null;
	}
	
	public StackingEffect InstantiateStackingEffect(StackingEffectType type) {

		foreach (StackingEffect effect in effectData.stackingEffects) {

			if (effect.enumId == type)
				return Instantiate(effect);
		}

		Debug.LogError("Can't Find Effect In List As Parameter Type");
		return null;
	}

}
