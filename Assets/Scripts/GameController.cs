using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class GameController : MonoBehaviour {
    Player player;
    public FinishManager finishManager;
    public UIManager uiManager;
    private void Start() {
        finishManager.Init(this);
        player = Player.instance;
    }
    private void Update() {
        finishManager.Update();
    }

    [Serializable]
    public class FinishManager {
        GameController gameController;
        public bool isFinish;
        public bool isFail = false;
        public void Init(GameController gc) {
            gameController = gc;
        }
        public void Update() {
            WellFinishFunc();
            FailFinishFunc();
        }
        public void WellFinishFunc() {
            if (isFinish) {
                var turretList = gameController.player.stackManager.turretList;
                foreach (var turret in turretList) {
                    turret.transform.DOScale(new Vector3(0, 0, 0), 1f);
                }
                if (turretList[0].transform.localScale.y < 0.1f) {
                    foreach (var turret in turretList) {
                        turret.gameObject.SetActive(false);
                    }
                }
                gameController.uiManager.winModule.Win();
            }
        }
        public void FailFinishFunc() {
            if (isFail) {
                gameController.uiManager.failModule.Fail();
            }
        }

    }
}


