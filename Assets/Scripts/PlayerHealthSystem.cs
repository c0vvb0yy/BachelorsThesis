using System.Collections.Generic;
using StarterAssets;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] int maxHealth; 
    private int _currentHealth;
    [SerializeField] GameObject onHitEffect; 
    [SerializeField] Healthbar healthBar; 
    [SerializeField] AudioClip onHitSound;
    
    [SerializeField] GameObject deathScreenCanvas;
    ThirdPersonController _player;
    Animator _animator;
    AudioSource _audio;
    Vector3 _startPos;

    DialogueVariableManager _variableStorage;

    public bool IsDead;

    void OnEnable(){
        DataManager.OnLoad += Deserialize;
    }
    void OnDisable() {
        DataManager.OnLoad -= Deserialize;
    }
    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = maxHealth;
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _player = GetComponent<ThirdPersonController>();
        _variableStorage = GameObject.FindWithTag("DVS").GetComponent<DialogueVariableManager>();
        _startPos = transform.position;
        LeanTween.reset();
        LeanTween.moveLocalY(deathScreenCanvas, Screen.height, 1.5f).setEaseInExpo();
    }

    public void TakeDamage(GameObject enemy, int amount){
        _currentHealth -= amount;
        healthBar.UpdateHealthbar(maxHealth, _currentHealth);
        _variableStorage.UpdatePlayerHealth(_currentHealth);
        _animator.SetTrigger("TakeDamage"); 
        if(_currentHealth <= 0){
            Die(enemy);
        }
    }

    void Die(GameObject enemy){
        IsDead = true;
        _animator.SetTrigger("Death");
        _player.RestrainMovement();
        LeanTween.moveLocalY(deathScreenCanvas, 0, 1.5f).setEaseInExpo().setDelay(3.5f).setOnComplete(Respawn);
        var eventData = new Dictionary<string, object>{
            {"KilledBy", enemy.name},
        }; 
        _variableStorage.UpdatePlayerDeathStatus(true);
        AnalyticsService.Instance.CustomData("PlayerDeath", eventData);
    }

    void Respawn(){
        _player.Teleport(_startPos);
        _currentHealth = maxHealth;
        healthBar.UpdateHealthbar(maxHealth, _currentHealth);
        GetComponent<PlayerInteraction>().StartDialogue(GameObject.Find("Witch").GetComponent<NPCDialogueManager>());
        LeanTween.moveLocalY(deathScreenCanvas, Screen.height, 1.5f).setEaseInExpo().setDelay(1);
        _animator.SetTrigger("Respawn");
        _variableStorage.UpdatePlayerDeathStatus(false);
        IsDead = false;
    }

    public void SpawnHitEffect(Vector3 point){
        GameObject hitVFX = Instantiate(onHitEffect, this.transform);
        hitVFX.transform.position = point;
        _audio.clip = onHitSound;
        _audio.Play();
        Destroy(hitVFX, 2f);
    }

    [YarnCommand]
    public void Heal(){
        var eventData = new Dictionary<string, object>{
            {"CurrentHitpoints", _currentHealth},
            {"HitpointPercentage", (_currentHealth/maxHealth)*100},
            {"Difference", maxHealth - _currentHealth}
        };
        AnalyticsService.Instance.CustomData("PlayerHeal", eventData);
        _currentHealth = maxHealth;
        _variableStorage.UpdatePlayerHealth(maxHealth);
        healthBar.UpdateHealthbar(maxHealth, _currentHealth);
    }

    private void Deserialize(SaveData data){
        _currentHealth = data.playerHealth;
        healthBar.UpdateHealthbar(maxHealth, _currentHealth);
    }

    public int getHealth(){
        return _currentHealth;
    }
}
