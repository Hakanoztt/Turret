using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Bullet : MonoBehaviour {
    public Turret turret;
    public float speed = 5;
    Vector3 direction;
    public Transform target;
    private void Start() {
        turret = Turret.instance;
    }
    void FixedUpdate() {
        Movement();
    }
    void Movement() {
        if (target != null) {
            direction = target.position - transform.position;
            direction += new Vector3(0, 1, 0);
            transform.forward = direction.normalized;
            transform.position += direction.normalized * speed * Time.deltaTime;
        } else {
            //transform.position += transform.forward * speed * Time.deltaTime;
          gameObject.SetActive(false);
        }
    }
    public void SetTarget(Transform newTarget, Turret newTurret) {
        target = newTarget;
        turret = newTurret;
    }
}