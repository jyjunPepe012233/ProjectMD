using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public abstract class BaseEntity : MonoBehaviour {

	[Header("[ Targets Points ]")]
	public List<Transform> bodyPoint;

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