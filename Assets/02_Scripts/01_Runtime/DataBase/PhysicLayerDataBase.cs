using UnityEngine;
using UnityEngine.Serialization;

namespace MinD.StatusFx {

	public class PhysicLayerDataBase : Singleton<PhysicLayerDataBase>{
		
		public LayerMask entityLayer;
		public LayerMask environmentLayer;

	}
}