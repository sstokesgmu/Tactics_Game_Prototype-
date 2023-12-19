using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private GameObject enemyTurnBannerGO;

    private void Start()
    {
        endTurnButton.onClick.AddListener(() =>{TurnSystem.Instance.NextTurn();});
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged; 
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButton();
    }
    private void UpdateTurnText()
    {
        turnNumberText.text = "TURN " + TurnSystem.Instance.GetCurrentTurnNumber();
    }
    private void UpdateEnemyTurnVisual()
    {
        enemyTurnBannerGO.SetActive(!TurnSystem.Instance.GetIsPlayerTurn());
       
    }
    private void UpdateEndTurnButton()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.GetIsPlayerTurn());
    }
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButton();
    }
}
