using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CoinPanelController : MonoBehaviour {
    public int coin = 0;
    public Text text;
    void Start() {
        UpdateText();
    }
    void Update() {
        UpdateText();
    }
    void UpdateText() {
        text.text = coin.ToString();
    }
    public void IncreaseCoin() {
        coin++;
    }
}
