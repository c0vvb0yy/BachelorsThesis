using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Image _healthbarSprite;
    [SerializeField] float reduceSpeed = 1.2f;
    float _target = 1f;

    public void UpdateHealthbar(float maxHealth, float currentHealth){
        _target = currentHealth / maxHealth;
    }

    private void Update() {
        _healthbarSprite.fillAmount = Mathf.MoveTowards(_healthbarSprite.fillAmount, _target, reduceSpeed*Time.deltaTime);
    }
}
