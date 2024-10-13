using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public abstract class BaseEntity : MonoBehaviour {

	[Header("[ Bound Targets Options ]")]
	public List<Transform> targetOptions;

	[HideInInspector] public EntityStatusFxHandler statusFx;
	
	[HideInInspector] public CharacterController cc;
	[HideInInspector] public Animator animator;
	
	
	
	protected virtual void Awake() {
		
		statusFx = GetComponent<EntityStatusFxHandler>();
		statusFx.owner = this;
		
		cc = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		
	}



	private void OnDrawGizmos() {
		
		Gizmos.color = Color.cyan;

		foreach (Transform targetOption in targetOptions) {
			Gizmos.DrawSphere(targetOption.position, 0.05f);
		}

	}
}