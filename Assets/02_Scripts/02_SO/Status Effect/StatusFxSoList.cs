using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MinD.StatusFx {
	public class StatusFxSoList : ScriptableObject {

		public List<InstantEffect> instantEffects;
		public List<StaticEffect> staticEffects;
		public List<TimedEffect> timedEffects;
		public List<StackingEffect> stackingEffects;
	}
}