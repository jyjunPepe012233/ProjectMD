using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public abstract class BaseEntity : MonoBehaviour {

	[Header("[ Targets ]")]
	public List<Transform> bodyTargets;

	[HideInInspector] public EntityStatusFxHandler statusFx;
	
	[HideInInspector] public CharacterController cc;
	[HideInInspector] public Animator animator;
	
	
	
	private void Awake() {
		
		statusFx = GetComponent<EntityStatusFxHandler>();
		statusFx.owner = this;
		
		cc = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		
	}
}