using System.Collections.Generic;
using UnityEngine;

namespace MinD.Runtime.Entity {

[RequireComponent(typeof(EntityStatusFxHandler))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public abstract class BaseEntity : MonoBehaviour {

	[Header("[ Base Settings ]")]
	public bool isInvincible;
	public bool immunePoiseBreak;
	[Space(5)]
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

	protected virtual void Update() {
		
		statusFx.HandleAllEffect();
		
	}



	private void OnDrawGizmos() {

		Gizmos.color = Color.cyan;

		if (targetOptions != null) {
			foreach (Transform targetOption in targetOptions) {
				Gizmos.DrawSphere(targetOption.position, 0.05f);
			}
		}

	}
}

}