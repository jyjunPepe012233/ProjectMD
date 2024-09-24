using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public abstract class BaseEntity : MonoBehaviour {

	[HideInInspector] public EntityStatusFxHandler statusFx;
	
	[HideInInspector] public CharacterController cc;
	[HideInInspector] public Animator animator;
	
	
	protected virtual void Awake() {
		
		statusFx = GetComponent<EntityStatusFxHandler>();
		
		cc = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		
	}
}