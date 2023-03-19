using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour {
    public TapToPlayModule tapToPlayModule;
    public RoadMovement roadMovement;
    public JoystickModule joystickModule;
    public SliderManager sliderManager;
    public CoinPanelController coinPanelController;

    public WinModule winModule;
    public FailModule failModule;
    void Start() {
        tapToPlayModule.Init(this);
        joystickModule.Init(this);
        sliderManager.Init(this);
        winModule.Init(this);
        failModule.Init(this);
    }
    void Update() {
        joystickModule.Update();
        sliderManager.Update();
        
    }

    public void TapToStart() {
        tapToPlayModule.TapToPlay();
    }
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    [Serializable]
    public class TapToPlayModule  {
        UIManager ui;
        public bool clicked = false;
        public GameObject tapToPlayPanel;
        public GameObject shopPanel;
        public void Init(UIManager u) {
            ui = u;
        }
        public void TapToPlay() {
            if (ui.roadMovement.allRoadsArrived) {
                clicked = true;
                tapToPlayPanel.SetActive(false);
                shopPanel.SetActive(false);
            }
          
        }
        //public void Update() {
        //    if (Input.GetMouseButtonDown(0) && ui.roadMovement.allRoadsArrived) {
        //        clicked = true;
        //        tapToPlayPanel.SetActive(false);
        //    }
        //}
    }
    [Serializable]
    public class JoystickModule {
        public GameObject joystick;
        UIManager ui;
        public void Init(UIManager u) {
            ui = u;
        }
        public void Update() {
            if (ui.tapToPlayModule.clicked) {
                joystick.gameObject.SetActive(true);
            }
        }
    }
    [Serializable]
    public class SliderManager {
        UIManager ui;
        public GameObject finishPos;
        public GameObject player;
        float maxDistance;
        public Slider levelSlider;
        public void Init(UIManager u) {
            ui = u;
            maxDistance = Vector3.Distance(player.transform.position,finishPos.transform.position);
            levelSlider.minValue = 0;
            levelSlider.maxValue = maxDistance;
        }
        public void Update() {
            float distance = Vector3.Distance(player.transform.position, finishPos.transform.position);
            levelSlider.value = maxDistance - distance;
        }


    }
    [Serializable]
    public class WinModule {
        UIManager ui;
        public GameObject winPanel;
        public GameObject slider;
        public GameObject coinPanel;
        public GameObject joystick;
        public Text goldText;
        public void Init(UIManager u) {
            ui = u;
        }
        public void Win() {
            winPanel.SetActive(true);
            slider.SetActive(false);
            coinPanel.SetActive(false);
            joystick.SetActive(false);
            goldText.text = ui.coinPanelController.coin.ToString();
        }
    }
    [Serializable]
    public class FailModule {
        UIManager ui;
        public GameObject failPanel;
        public GameObject slider;
        public GameObject coinPanel;
        public GameObject joystick;
        public Text goldText;
        public void Init(UIManager u) {
            ui = u;
        }
        public void Fail() {
            failPanel.SetActive(true);
            slider.SetActive(false);
            coinPanel.SetActive(false);
            joystick.SetActive(false);
            goldText.text = ui.coinPanelController.coin.ToString();
        }
    }
}
