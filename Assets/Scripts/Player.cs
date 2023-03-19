using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class Player : MonoBehaviour {
    public MovementModule movementModule;
    public StackManager stackManager;
    public UIManager UIManager;
    public RoadMovement roadMovement;
    public BuffManager buffManager;
    public GameController gameController;

    public static Player instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }
    void Start() {
        movementModule.Init(this);
        stackManager.Init(this);
        buffManager.Init(this);
    }
    void Update() {
        movementModule.Update();
        stackManager.Update();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Turret")) {
            var turret = other.GetComponent<Turret>();
            stackManager.Add(turret);
            turret.transform.SetParent(transform);
        }
        if (other.CompareTag("Finish")) {
            gameController.finishManager.isFinish = true;
        }
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ocean")) {
            gameController.finishManager.isFail = true;
        }
    }
    [Serializable]
    public class MovementModule {
        Player player;
        public FloatingJoystick fj;
        public float moveSpeed;
        public float rotateSpeed;
        public bool isGround = true;
        public void Init(Player p) {
            player = p;
        }
        public void Update() {
            Move();
            Rotate();
            Vector3 customGravity = 4 * Physics.gravity;
            player.GetComponent<Rigidbody>().AddForce(customGravity, ForceMode.Acceleration);
        }
        void Move() {
            if ((Input.touchCount > 0 || (fj.Vertical != 0 || fj.Horizontal != 0)) && player.UIManager.tapToPlayModule.clicked && !player.gameController.finishManager.isFinish &&!player.gameController.finishManager.isFail) {
                player.transform.position += new Vector3(-fj.Vertical, 0, fj.Horizontal) * moveSpeed * Time.deltaTime;
                player.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        void Rotate() {
            if (!player.gameController.finishManager.isFail) {
                player.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
            }
        }
    }
    [Serializable]
    public class StackManager {
        Player player;
        public List<Turret> turretList;

        bool _added = false;
        public void Init(Player p) {
            player = p;
            turretList = new List<Turret>();
        }
        public void Update() {
            ListControl();
        }
        public void Add(Turret turret) {
            if (!turretList.Contains(turret)) {
                turretList.Add(turret);
                turret.movementModule.taken = true;
                _added = true;
            }
        }
        public void ListControl() {
            if (turretList.Count==0 && _added) {
                player.gameController.finishManager.isFail = true;
            }
        }
    }
    [Serializable]
    public class BuffManager {
        Player player;
        public Turret[] turrets;
        public void Init(Player p) {
            player = p;
            turrets = FindObjectsOfType<Turret>();
        }
        public void MoveSpeedBuff(float moveSpeed) {
            player.movementModule.moveSpeed += moveSpeed;
        }
        public void FireSpeedBuff(float fireRate) {
            foreach (var turret in turrets) {
                if (turret!=null) {
                    turret.fireModule.fireRate -= fireRate;
                }
      
            }
        }
        public void RadarRangeBuff(float radarRange) {
            foreach (var turret in turrets) {
                if (turret!=null) {
                    turret.fireModule.range += radarRange;
                }
            }

        }
    }
}
