using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatusBarScript : MonoBehaviour
{
    public Slider hpSlider; 
    public Image damageImage; 
    public RectTransform healthBarRectTransform;
    public RectTransform damageBarRectTransform;
    public RectTransform backgroundRectTransform;
    public RectTransform borderRectTransform;
    public Player player; // Player 참조 추가
    public int maxHealth = 100; // int로 변경
    public int currentHealth;
    public float decreaseDuration = 1f;
    public float lengthChangeAmount = 20f; 

    private void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }

        maxHealth = player.curHp; // maxHp를 참조
        currentHealth = maxHealth;
        UpdateHpSlider(currentHealth);
        damageImage.fillAmount = 1; 

        SetPivotToLeft();
    }

    private void Update()
    {
        // Player의 curHp 값을 현재 체력으로 동기화
        int newHealth = player.curHp;

        if (newHealth != currentHealth)
        {
            currentHealth = newHealth;
            UpdateHpSlider(currentHealth);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            maxHealth += 20; // int로 변경
            player.curHp = maxHealth; // 최대 체력에 맞게 curHp 업데이트
            ChangeUIElementSizes(lengthChangeAmount);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (maxHealth > 20) // 최대 체력이 20 이하로 떨어지지 않도록 방지
            {
                maxHealth -= 20; // int로 변경
                player.curHp = Mathf.Min(player.curHp, maxHealth); // curHp가 maxHealth보다 크지 않게 조정
                ChangeUIElementSizes(-lengthChangeAmount);
            }
        }

        // R 키를 눌렀을 때 20 회복
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.curHp += 20; // 체력 회복
            player.curHp = Mathf.Min(player.curHp, maxHealth); // maxHealth를 초과하지 않도록 조정
        }
    }

    private void UpdateHpSlider(float health)
    {
        hpSlider.value = health / maxHealth;

        // 체력이 변경될 때 damageImage 업데이트
        StartCoroutine(UpdateDamageBar(decreaseDuration, health));
    }

    private IEnumerator UpdateDamageBar(float duration, float targetHealth)
    {
        float elapsedTime = 0f;
        float damageImageStart = damageImage.fillAmount;
        float damageImageTarget = targetHealth / maxHealth;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float lerpValue = Mathf.Lerp(damageImageStart, damageImageTarget, elapsedTime / duration);
            damageImage.fillAmount = lerpValue; 
            yield return null;
        }

        damageImage.fillAmount = damageImageTarget;
    }

    private void ChangeUIElementSizes(float amount)
    {
        healthBarRectTransform.sizeDelta = new Vector2(healthBarRectTransform.sizeDelta.x + amount, healthBarRectTransform.sizeDelta.y);
        damageBarRectTransform.sizeDelta = new Vector2(damageBarRectTransform.sizeDelta.x + amount, damageBarRectTransform.sizeDelta.y);
        backgroundRectTransform.sizeDelta = new Vector2(backgroundRectTransform.sizeDelta.x + amount, backgroundRectTransform.sizeDelta.y);
        borderRectTransform.sizeDelta = new Vector2(borderRectTransform.sizeDelta.x + amount, borderRectTransform.sizeDelta.y);
    }

    private void SetPivotToLeft()
    {
        healthBarRectTransform.pivot = new Vector2(0, 0.5f);
        damageBarRectTransform.pivot = new Vector2(0, 0.5f);
        backgroundRectTransform.pivot = new Vector2(0, 0.5f);
        borderRectTransform.pivot = new Vector2(0, 0.5f);
    }
}
