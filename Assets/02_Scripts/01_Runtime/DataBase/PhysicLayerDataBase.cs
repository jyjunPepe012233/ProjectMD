using UnityEngine;
using UnityEngine.Serialization;

namespace MinD {

	public class PhysicLayerDataBase : Singleton<PhysicLayerDataBase>{
		
		public LayerMask entityLayer;
		public LayerMask environmentLayer;

	}
}