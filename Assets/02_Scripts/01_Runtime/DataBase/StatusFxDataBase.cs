using MinD.StatusFx;
using UnityEngine;


namespace MinD.StatusFx {

	public class StatusFxDataBase : Singleton<StatusFxDataBase> {
	
		[SerializeField] private StatusFxSoList effectSoList;
	
	
		public InstantEffect GetEffectData(InstantEffectType type) {
	
			foreach (InstantEffect effect in effectSoList.instantEffects) {
	
				if (effect.enumId == type)
					return Instantiate(effect);
			}
	
			Debug.LogError("Can't Find Effect In List As Parameter Type");
			return null;
		}
	
		public StaticEffect GetEffectData(StaticEffectType type) {
	
			foreach (StaticEffect effect in effectSoList.staticEffects) {
	
				if (effect.enumId == type)
					return Instantiate(effect);
			}
	
			Debug.LogError("Can't Find Effect In List As Parameter Type");
			return null;
		}
	
		public TimedEffect GetEffectData(TimedEffectType type) {
	
			foreach (TimedEffect effect in effectSoList.timedEffects) {
	
				if (effect.enumId == type)
					return Instantiate(effect);
			}
	
			Debug.LogError("Can't Find Effect In List As Parameter Type");
			return null;
		}
	
		public StackingEffect GetEffectData(StackingEffectType type) {
	
			foreach (StackingEffect effect in effectSoList.stackingEffects) {
	
				if (effect.enumId == type)
					return Instantiate(effect);
			}
	
			Debug.LogError("Can't Find Effect In List As Parameter Type");
			return null;
		}
	
	}
}