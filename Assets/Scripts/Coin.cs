using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public RectTransform coinPanel;
    CoinPanelController coinPanelController;
    bool _isCol = false;

    private void Start() {
        coinPanelController = FindObjectOfType<CoinPanelController>();
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            coinPanelController.IncreaseCoin();
            _isCol = true;
        }
        if (collision.gameObject.CompareTag("Ocean")) {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Turret")) {
            _isCol = true;
            coinPanelController.IncreaseCoin();
        }
    }
    private void Update() {
        MoveTowardsToTarget();
    }
    void MoveTowardsToTarget() {
        if (_isCol) {
            transform.position = Vector3.MoveTowards(transform.position, coinPanel.position, 0.5f);
            GetComponent<Rigidbody>().useGravity = false;
        }
   
    }
}
