using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shootable : MonoBehaviour {

    public Health Health;
    public GameObject miniEnemy;
    public ParticleSystem damageEffect;
    private void Start() {
        Health.UpdateHealthText();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Bullet")) {
            other.gameObject.SetActive(false);
            Health.TakeDamage(Health.damageAmount);
           var _damageEffect= Instantiate(damageEffect, transform.position, Quaternion.identity);
            Destroy(_damageEffect, 2f);
           var _miniEnemy= Instantiate(miniEnemy, transform.position + new Vector3(Random.Range(0.75f,2), 0.50f, Random.Range(-1, 4)), Quaternion.identity);
            _miniEnemy.GetComponent<MiniEnemy>().speed += Random.Range(0f, 0.3f);
            if (Health.health <= 0) {
                Destroy(gameObject);        
            }
        }
        if (other.CompareTag("Turret")) {
            var turret = other.GetComponent<Turret>();
            turret.healthModule.Explosion();
        }
    }
    private void Update() {
        Health.UpdateHealthText();
    }
}

[System.Serializable]
public struct Health : IDamagable{
    public int health;
    public int damageAmount;
    public TextMeshPro healthText;


    public void TakeDamage(int damage) {    
        health -= damage;
    }
    public void UpdateHealthText() {
        healthText.text = health.ToString();
    }
}

public interface IDamagable {
    public void TakeDamage(int damage);
}