using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniEnemy : MonoBehaviour {
    public Health Health;
    public List<Turret> turrets = new List<Turret>();
    public float speed = 5f;
    public int damageAmount = 1;
    public ParticleSystem damageEffect;
    public GameObject coin;

    public Vector3 _targetTurret;
    Player player;
    private void Start() {
        player = Player.instance;
        Health.health = SetHealth();
    }

    private void Update() {
        _targetTurret = GetClosestTurretPosition();
        UpdateTurretList();
        MoveTowardsTargetTurret();
        LookAt(_targetTurret);
        CheckHeigthPos();
        Destroy();
    }

    int SetHealth() {
        float rand = Random.value;
        if (rand <= 0.4f) {
            return 1;
        } else {
            return 2;
        }
    }
    Vector3 GetClosestTurretPosition() {

        Vector3 closestTurret = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        if (turrets.Count <= 0) {
            closestTurret = transform.position;
        }

        foreach (Turret turret in turrets) {
            if (turret!=null) {
                float distanceToTurret = Vector3.Distance(transform.position, turret.transform.position);
                if (distanceToTurret < closestDistance) {
                    closestTurret = turret.transform.position;
                    closestDistance = distanceToTurret;
                }
            }
        }
        return closestTurret;
    }
    void UpdateTurretList() {
        turrets.Clear();
        foreach (Turret turret in player.stackManager.turretList) {
            if (turret != null) {
                turrets.Add(turret);
            }
        }
    }
    void MoveTowardsTargetTurret() {
        transform.position = Vector3.MoveTowards(transform.position, _targetTurret, speed * Time.deltaTime);
    }
    void LookAt(Vector3 target) {
        transform.LookAt(target);
    }
    void CheckHeigthPos() {
        if (transform.position.y < 1) {
            Destroy(gameObject);
        }
    }
    void Destroy() {
        if (player.gameController.finishManager.isFail ||player.gameController.finishManager.isFinish) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Bullet")) {
            other.gameObject.SetActive(false);
           var _damageEffect= Instantiate(damageEffect, transform.position, Quaternion.identity);
            Destroy(_damageEffect, 2f);
            Health.TakeDamage(Health.damageAmount);
            if (Health.health <= 0) {
                var _coin = Instantiate(coin, transform.position, Quaternion.identity);
                _coin.GetComponent<Rigidbody>().velocity = new Vector3(0,Random.Range(0.5f,1f),Random.Range(0,0.3f)) * 5f;
                Destroy(gameObject);
            }
        }
        if (other.CompareTag("Turret") && player.stackManager.turretList.Contains(other.GetComponent<Turret>())) {
            Destroy(gameObject);
            var turret = other.GetComponent<Turret>();
            turret.healthModule.Health.TakeDamage(damageAmount);

        }
    }
}
