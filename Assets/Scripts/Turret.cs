using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    Player player;
    public static Turret instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
    public MovementModule movementModule;
    public FireModule fireModule;
    public HealthModule healthModule;
    public UIManager UIManager;
    void Start() {
        player = Player.instance;
        movementModule.Init(this);
        healthModule.Init(this);
        fireModule.Init(this);
    }
    void Update() {
        movementModule.Update();
        fireModule.Update();
        healthModule.Update();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Turret")) {
            var turret = other.GetComponent<Turret>();
            if (!player.stackManager.turretList.Contains(turret)) {
                player.stackManager.Add(turret);
                turret.transform.parent = transform.parent;
            }
        }
    }
    [Serializable]
    public class MovementModule {
        Turret turret;
        public bool taken = false;
        public GameObject cube;
        public float rotateSpeed;
        public void Init(Turret t) {
            turret = t;
        }
        public void Update() {
            Rotate();
            if (turret.player.stackManager.turretList.Contains(turret)) {
                turret.transform.position = new Vector3(turret.transform.position.x, turret.player.transform.position.y, turret.transform.position.z);
            }

        }
        public void Rotate() {
            if (!taken) {
                cube.transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
            }
        }
    }
    [Serializable]
    public class FireModule {
        Turret turret;
        public GameObject bulletPrefab;
        public Transform firePos;
        public float range;
        public float fireRate;
        public Transform enemyPos;
        public LayerMask enemyLayer;
        public bool fire = false;
        Quaternion firstRotation;

        public enum Priority { box, enemy };
        public Priority priority;

        public GameObject tip;

        Collider[] hitColliders;
        List<GameObject> bulletPool;
        public void Init(Turret t) {
            turret = t;
            bulletPool = new List<GameObject>();

            turret.StartCoroutine(Fire());
            for (int i = 0; i < 5; i++) {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.transform.position = firePos.position;
                bullet.SetActive(false);
                bulletPool.Add(bullet);
            }
            firstRotation = tip.transform.rotation;
        }
        public void Update() {
            if (turret.UIManager.tapToPlayModule.clicked) {
                Lookat();
                Shoot();
            }
        }
        public void Shoot() {
            switch (priority) {
                case Priority.box:
                    if (enemyPos == null) {
                        fire = false;
                        hitColliders = Physics.OverlapSphere(turret.transform.position, range, enemyLayer);
                        foreach (Collider col in hitColliders) {
                            enemyPos = col.transform;
                        }
                    } else {
                        fire = true;
                    }
                    break;
                case Priority.enemy:
                    hitColliders = Physics.OverlapSphere(turret.transform.position, range, enemyLayer);
                    foreach (Collider col in hitColliders) {
                        enemyPos = col.transform;
                    }
                    fire = true;

                    if (enemyPos == null) {
                        fire = false;
                    }
                    break;
                default:
                    break;
            }
        }
        GameObject GetBulletFromPool() {
            for (int i = 0; i < bulletPool.Count; i++) {
                if (!bulletPool[i].activeInHierarchy) {
                    bulletPool[i].transform.position = firePos.position;
                    bulletPool[i].GetComponent<Bullet>().SetTarget(enemyPos, turret);
                    return bulletPool[i];
                }
            }
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = firePos.position;
            bullet.GetComponent<Bullet>().SetTarget(enemyPos, turret);
            bullet.GetComponent<Bullet>().target = enemyPos;
            bulletPool.Add(bullet);

            return bullet;
        }
        public IEnumerator Fire() {
            while (true) {
                if (enemyPos != null && turret.player.stackManager.turretList.Contains(turret) && fire) {
                    if (!turret.player.gameController.finishManager.isFail) {
                        GameObject bullet = GetBulletFromPool();
                        bullet.SetActive(true);
                    }
                }
                yield return new WaitForSeconds(fireRate);
            }
        }
        void Lookat() {
            if (enemyPos && turret.player.stackManager.turretList.Contains(turret)) {
                tip.transform.LookAt(enemyPos);
            }
        }
    }
    [Serializable]
    public class HealthModule {
        public Health Health;
        Turret turret;
        public int firstHealth;
        public Material damagedMaterial;
        public GameObject explosionEffect;
        public void Init(Turret t) {
            turret = t;
            firstHealth = Health.health;
        }
        public void Update() {
            HealthFunc();
        }
        void HealthFunc() {
            if (Health.health == 1) {
                turret.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.Lerp(turret.transform.GetChild(0).GetComponent<Renderer>().material.color, damagedMaterial.color, 0.1f);
            }
            if (Health.health <= 0) {
                Explosion();
            }
        }
        public void Explosion() {
            turret.player.stackManager.turretList.Remove(turret);
            var _effect = Instantiate(explosionEffect, turret.transform.position, Quaternion.identity);
            Destroy(_effect, 2f);
            Destroy(turret.gameObject);

        }
    }
}
