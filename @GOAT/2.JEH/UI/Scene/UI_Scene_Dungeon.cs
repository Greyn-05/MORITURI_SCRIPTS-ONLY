using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Scene_Dungeon : UI_Scene
{
    [SerializeField] private SliderBar _playerHpSlider;
    [SerializeField] private Slider _plauyerStaminaSlider;

    [SerializeField] private SliderBar _enemyHpSlider;  //TODO 현재 싸우는 적의 EnemyInfo 정보를 읽을수 있어야한다
    [SerializeField] private TextMeshProUGUI _enemyNameText;
    [SerializeField] private TextMeshProUGUI _enemyHpPerText;

    [SerializeField] private GameObject FarringImagesObj;
    private List<GameObject> farringImages = new();

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;


        for (int i = 0; i < FarringImagesObj.transform.childCount; i++)
        {
            farringImages.Add(FarringImagesObj.transform.GetChild(i).gameObject);

        }
        _enemyNameText.text = Main.Game.currentEnemyStatus.Name;

        _playerHpSlider._mainSlider.minValue = 0;
        _playerHpSlider._mainSlider.maxValue = Main.Player.Status.HP.MaxValue;
        _playerHpSlider._mainSlider.value = Main.Player.Status.HP.CurValue;
        _playerHpSlider._subSlider.minValue = 0;
        _playerHpSlider._subSlider.maxValue = Main.Player.Status.HP.MaxValue;
        _playerHpSlider._subSlider.value = Main.Player.Status.HP.CurValue;

        _plauyerStaminaSlider.minValue = 0;
        _plauyerStaminaSlider.maxValue = Main.Player.Status.Stamina.MaxValue;
        _plauyerStaminaSlider.value = Main.Player.Status.Stamina.CurValue;

        _enemyHpSlider._mainSlider.minValue = 0;
        _enemyHpSlider._mainSlider.maxValue = Main.Game.currentEnemyStatus.HP.MaxValue;
        _enemyHpSlider._mainSlider.value = Main.Game.currentEnemyStatus.HP.CurValue;
        _enemyHpSlider._subSlider.minValue = 0;
        _enemyHpSlider._subSlider.maxValue = Main.Game.currentEnemyStatus.HP.MaxValue;
        _enemyHpSlider._subSlider.value = Main.Game.currentEnemyStatus.HP.CurValue;

        _enemyHpPerText.text = $"{Mathf.Floor(Main.Game.currentEnemyStatus.HP.CurValue / (Main.Game.currentEnemyStatus.HP.MaxValue / 100) * 10f) / 10f}%";

        // 보스 초기화

        return true;
    }

    private void FixedUpdate() //TODO 게임 환경 변할될떄 호출되는 액션에 Refresh 넣을것 
    {
        Refresh();

        for (int i = 0; i < farringImages.Count; i++)
            farringImages[i].SetActive(false); 

        for (int i = 0; i < Main.Player.ParryCount; i++)
            farringImages[i].SetActive(true); 

    }

    public void Refresh()
    {
        _playerHpSlider.Refresh(Main.Player.Status.HP.CurValue);
        _plauyerStaminaSlider.value = Main.Player.Status.Stamina.CurValue;

        _enemyHpSlider.Refresh(Main.Game.currentEnemyStatus.HP.CurValue);
        _enemyHpPerText.text = $"{Mathf.Floor(Main.Game.currentEnemyStatus.HP.CurValue / (Main.Game.currentEnemyStatus.HP.MaxValue / 100) * 10f) / 10f}%";

    }
}
