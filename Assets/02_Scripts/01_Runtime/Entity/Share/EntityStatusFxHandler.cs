using System;
using System.Collections.Generic;
using UnityEngine;
using MinD.StatusFx;


public class EntityStatusFxHandler : MonoBehaviour {

	[HideInInspector] public BaseEntity owner;

	[Header("[ Owned Effect List ]")]
	[SerializeField] private List<StaticEffect> staticEffects;
	[SerializeField] private List<TimedEffect> timedEffects;
	[SerializeField] private List<StackingEffect> stackingEffects;
		// 비정상적인 접근으로 인한 영구적인 스테이터스 변동을 막기 위해 PRIVATE으로 제한함

	[Header("[ Immune ]")]
	public bool isImmuneToAllEffect;
	[Space(5)]
	public List<TimedEffectType> timedFxImmune;
	public List<StackingEffectType> stackingFxImmune;


	private void Awake() {
		owner = GetComponent<BaseEntity>();
	}
	
		

	#region Instant_Effect
	
	public void AddInstantEffect(InstantEffect effectInstance) {
		
		effectInstance.OnInstantiate(owner);
		Destroy(effectInstance);
		
	}
	
	#endregion
	
	
	#region Timed_Effect

	private void HandleTimedEffect() {

		foreach (TimedEffect effect in timedEffects) {
			
			effect.Execute();
			
			effect.remainTime -= Time.deltaTime;
			
			if (effect.remainTime < 0)
				RemoveTimedEffect(effect);
		}
	}
	
	public void AddTimedEffect(TimedEffect effect) {
		
		timedEffects.Add(effect);
		effect.OnInstantiate(owner);
	}
	
	public void RemoveTimedEffect(TimedEffect effect) {

		timedEffects.Remove(effect);
		
		effect.OnRemove();
		Destroy(effect);
	}
	
	public void RemoveTimedEffectByType(TimedEffectType type) {
		
		foreach (TimedEffect effect in timedEffects)
			if (effect.enumId == type) RemoveTimedEffect(effect);
	}
	
	
	#endregion

	
	#region Static_Effect

	public void AddStaticEffect(StaticEffect effect) {

		staticEffects.Add(effect);
		effect.OnInstantiate(owner);

	}
	
	public void RemoveStaticEffect(StaticEffect effect) {
		
		staticEffects.Add(effect);
		effect.OnRemove();
		
		Destroy(effect);
	}

	public void RemoveStaticEffectByType(StaticEffectType type) {

		foreach (StaticEffect effect in staticEffects)
			if (effect.enumId == type) RemoveStaticEffect(effect);
		
	}
	
	#endregion

	// TODO: StackingEffect 만들기
	#region Stacking_Effect
	
	private void HandleStackingEffect() {
		
		
	}
	
	public void AddStackingEffect(StackingEffectType type, float stackAmount) {
		
		
	}


	public void RemoveStackingEffect(StackingEffectType type) {

		
	}
	
	#endregion


	public void HandleAllEffect() {
		
		// 능동적인 상태이상(TIMED, STACKING) 객체의 생명 관리 메소드
		HandleTimedEffect();
		HandleStackingEffect();
		
	}
	
	
	public void RemoveAllEffect() {

		foreach (StaticEffect effect in staticEffects)
			RemoveStaticEffect(effect);
		
		foreach (TimedEffect effect in timedEffects)
			RemoveTimedEffect(effect);

		foreach (StackingEffect effect in stackingEffects)
			RemoveStackingEffect(effect.enumId);
	}
	
	
	
}