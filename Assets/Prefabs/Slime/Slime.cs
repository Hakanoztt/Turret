using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Slime : MonoBehaviour {
    public Health Health;
    public GameObject miniEnemy;
    public int increaseMiniEnemySpeed;
    public ParticleSystem damageEffect;
    public ParticleSystem destroyEffect;

    public List<GameObject> miniEnemies;
    public List<Turret> turrets = new List<Turret>();

    public int MiniEnemyAmount;
    public float speed;

    Vector3 _targetTurret;
    bool _swell = false;
    Player player;

    private void Start() {
        player = Player.instance;
        InstantiateMiniEnemies();
    }
    private void Update() {
        _targetTurret = GetClosestTurretPosition();
        UpdateTurretList();
        MoveTowardsTargetTurret();
        LookAt(_targetTurret);
    }
    void MoveTowardsTargetTurret() {
        if (_swell) {
            if (!player.gameController.finishManager.isFail && !player.gameController.finishManager.isFinish) {
                transform.position = Vector3.MoveTowards(transform.position, _targetTurret, speed * Time.deltaTime);
            } else if (player.gameController.finishManager.isFail && player.gameController.finishManager.isFinish) {
                transform.position = Vector3.MoveTowards(transform.position, _targetTurret, speed * Time.deltaTime);
            }
        }
    }
    void LookAt(Vector3 target) {
        transform.LookAt(target);
    }
    Vector3 GetClosestTurretPosition() {

        Vector3 closestTurret = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        if (turrets.Count <= 0) {
            closestTurret = transform.position;
        }

        foreach (Turret turret in turrets) {
            if (turret != null) {
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

    void InstantiateMiniEnemies() {
        for (int i = 0; i < MiniEnemyAmount; i++) {
            var _instantiatedObj = Instantiate(miniEnemy, transform.position + new Vector3(0, 0, Random.Range(-15, 15)), Quaternion.identity);
            _instantiatedObj.GetComponent<Rigidbody>().velocity = Vector3.up * 5;
            _instantiatedObj.GetComponent<MiniEnemy>().speed = Random.Range(increaseMiniEnemySpeed + 0.5f, increaseMiniEnemySpeed + 2f);
            _instantiatedObj.SetActive(false);
            miniEnemies.Add(_instantiatedObj);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Bullet")) {
            other.gameObject.SetActive(false);
            transform.DOScale(transform.localScale + new Vector3(0.10f, 0.10f, 0.10f), 0.3f);
            Health.TakeDamage(Health.damageAmount);
            var _damageEffect = Instantiate(damageEffect, transform.position, Quaternion.identity);
            Destroy(_damageEffect, 2f);
            _swell = true;
            if (Health.health <= 0) {
                foreach (var miniEnemy in miniEnemies) {
                    miniEnemy.SetActive(true);
                }
                var _destroyEffect = Instantiate(destroyEffect, transform.position, Quaternion.identity);
                Destroy(_destroyEffect, 2f);
                Destroy(gameObject);
            }
        }
        if (other.CompareTag("Turret")) {
            other.GetComponent<Turret>().healthModule.Explosion();
        }
    }
}
