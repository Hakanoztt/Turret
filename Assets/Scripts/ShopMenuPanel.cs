using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShopMenuPanel : MonoBehaviour {

    public Player player;
    public CoinPanelController coinPanelController;

    public Transform playerTransform;
    public ParticleSystem buffEfect;

    [Serializable]
    public struct Prices {
        [Serializable]
        public struct MoveSpeedPrices {
            public int moveSpeedPriceLevel1;
            public int moveSpeedPriceLevel2;
            public int moveSpeedPriceLevel3;
        }
        [Serializable]
        public struct FireSpeedPrices {
            public int fireSpeedPriceLevel1;
            public int fireSpeedPriceLevel2;
            public int fireSpeedPriceLevel3;
        }
        [Serializable]
        public struct RadarRangePrices {
            public int radarRangePriceLevel1;
            public int radarRangePriceLevel2;
            public int radarRangePriceLevel3;
        }

        public MoveSpeedPrices moveSpeedPrices;
        public FireSpeedPrices fireSpeedPrices;
        public RadarRangePrices radarRangePrices;
    }
    [Serializable]
    public struct Buffs {
        [Serializable]
        public struct MoveSpeedBuffs {
            public float Level2Buff;
            public float Level3Buff;
        }
        [Serializable]
        public struct FireSpeedBuffs {
            public float Level2Buff;
            public float Level3Buff;
        }
        [Serializable]
        public struct RadarRangeBuffs {
            public float Level2Buff;
            public float Level3Buff;
        }

        public MoveSpeedBuffs moveSpeedBuffs;
        public FireSpeedBuffs fireSpeedBuffs;
        public RadarRangeBuffs radarRangeBuffs;
    }
    [Serializable]
    public struct Texts {
        [Serializable]
        public struct Levels {
            public Text moveSpeedLevelText;
            public Text fireSpeedLevelText;
            public Text radarRangeLevelText;
        }
        [Serializable]
        public struct Prices {
            public Text moveSpeedPriceText;
            public Text fireSpeedPriceText;
            public Text radarRangePriceText;
        }

        public Levels levels;
        public Prices prices;
    }

    public Buffs buffs;
    public Prices prices;
    public Texts texts;

    int _moveSpeedBuff = 0;
    int _fireSpeedBuff = 0;
    int _radarRangeBuff = 0;

    private void Start() {
        texts.prices.moveSpeedPriceText.text = prices.moveSpeedPrices.moveSpeedPriceLevel1.ToString();
        texts.prices.fireSpeedPriceText.text = prices.fireSpeedPrices.fireSpeedPriceLevel1.ToString();
        texts.prices.radarRangePriceText.text = prices.radarRangePrices.radarRangePriceLevel1.ToString();
    }
    public void MoveSpeedOnClick() {
        switch (_moveSpeedBuff) {
            case 0:
                if (coinPanelController.coin > prices.moveSpeedPrices.moveSpeedPriceLevel1) {
                    texts.levels.moveSpeedLevelText.text = "level 2";
                    var _effect = Instantiate(buffEfect, playerTransform.position, Quaternion.identity);
                    Destroy(_effect, 2);
                    player.buffManager.MoveSpeedBuff(buffs.moveSpeedBuffs.Level2Buff);
                    texts.prices.moveSpeedPriceText.text = prices.moveSpeedPrices.moveSpeedPriceLevel2.ToString();
                    coinPanelController.coin -= prices.moveSpeedPrices.moveSpeedPriceLevel1;
                    _moveSpeedBuff++;
                }
                break;
            case 1:
                if (coinPanelController.coin > prices.moveSpeedPrices.moveSpeedPriceLevel2) {
                    texts.levels.moveSpeedLevelText.text = "level 3";
                    var _effect = Instantiate(buffEfect, playerTransform.position, Quaternion.identity);
                    Destroy(_effect, 2);
                    player.buffManager.MoveSpeedBuff(buffs.moveSpeedBuffs.Level3Buff);
                    texts.prices.moveSpeedPriceText.text = prices.moveSpeedPrices.moveSpeedPriceLevel3.ToString();
                    coinPanelController.coin -= prices.moveSpeedPrices.moveSpeedPriceLevel2;
                    _moveSpeedBuff++;
                }
                break;
            default:
                break;
        }
    }
    public void FireSpeedOnClick() {
        switch (_fireSpeedBuff) {
            case 0:
                if (coinPanelController.coin > prices.fireSpeedPrices.fireSpeedPriceLevel1) {
                    texts.levels.fireSpeedLevelText.text = "level 2";
                    var _effect = Instantiate(buffEfect, playerTransform.position, Quaternion.identity);
                    Destroy(_effect, 2);
                    player.buffManager.FireSpeedBuff(buffs.fireSpeedBuffs.Level2Buff);
                    texts.prices.fireSpeedPriceText.text = prices.fireSpeedPrices.fireSpeedPriceLevel2.ToString();
                    _fireSpeedBuff++;
                    coinPanelController.coin -= prices.fireSpeedPrices.fireSpeedPriceLevel1;
                }

                break;
            case 1:
                if (coinPanelController.coin > prices.fireSpeedPrices.fireSpeedPriceLevel2) {
                    texts.levels.fireSpeedLevelText.text = "level 3";
                    var _effect = Instantiate(buffEfect, playerTransform.position, Quaternion.identity);
                    Destroy(_effect, 2);
                    player.buffManager.FireSpeedBuff(buffs.fireSpeedBuffs.Level3Buff);
                    texts.prices.fireSpeedPriceText.text = prices.fireSpeedPrices.fireSpeedPriceLevel3.ToString();
                    _fireSpeedBuff++;
                    coinPanelController.coin -= prices.fireSpeedPrices.fireSpeedPriceLevel2;
                }
                break;
            default:
                break;
        }
    }
    public void RadarRangeOnClick() {
        switch (_radarRangeBuff) {
            case 0:
                if (coinPanelController.coin > prices.radarRangePrices.radarRangePriceLevel1) {
                    texts.levels.radarRangeLevelText.text = "level 2";
                    var _effect = Instantiate(buffEfect, playerTransform.position, Quaternion.identity);
                    Destroy(_effect, 2);
                    player.buffManager.RadarRangeBuff(buffs.radarRangeBuffs.Level2Buff);
                    texts.prices.radarRangePriceText.text = prices.radarRangePrices.radarRangePriceLevel2.ToString();
                    _radarRangeBuff++;
                    coinPanelController.coin -= prices.radarRangePrices.radarRangePriceLevel1;
                }

                break;
            case 1:
                if (coinPanelController.coin > prices.radarRangePrices.radarRangePriceLevel2) {
                    texts.levels.radarRangeLevelText.text = "level 3";
                    var _effect = Instantiate(buffEfect, playerTransform.position, Quaternion.identity);
                    Destroy(_effect, 2);
                    player.buffManager.RadarRangeBuff(buffs.radarRangeBuffs.Level3Buff);
                    texts.prices.radarRangePriceText.text = prices.radarRangePrices.radarRangePriceLevel3.ToString();
                    _radarRangeBuff++;
                    coinPanelController.coin -= prices.radarRangePrices.radarRangePriceLevel2;
                }
                break;
            default:
                break;
        }
    }
}
