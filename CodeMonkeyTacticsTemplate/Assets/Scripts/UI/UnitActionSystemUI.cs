using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;
    [SerializeField] private TextMeshProUGUI unitNameUI;
    [SerializeField] private TextMeshProUGUI actionPointsUI;
    [SerializeField] private Image healthbarImage;

    private List<ActionButtonUI> actionButtonUILists;

    private void Awake()
    {
        actionButtonUILists = new List<ActionButtonUI>();
    }

    private void Start()
    {
        //Delegates
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        Unit.OnAnyActionPointChanged += Unit_OnAnyActionPointsChanged;
        HealthSystem.OnDamagedPlayer += HealthSystem_OnDamagedPlayer;
        HealthSystem.OnHealed += HealthSystem_OnHeal;

        

        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateUnitName();
        UpdateActionPoints();
    }

    private void CreateUnitActionButtons()
    {
        foreach( Transform buttonTransform in actionButtonContainer)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUILists.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform  actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainer);
            ActionButtonUI actionUI =  actionButtonTransform.GetComponent<ActionButtonUI>();
            actionUI.SetBaseAction(baseAction);
            actionButtonUILists.Add(actionUI);
        }
    }
    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionUI in actionButtonUILists)
        {
            actionUI.UpdateSelectedVisual();
        }
    }

    private void UpdateUnitName()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if(selectedUnit != null) 
             unitNameUI.text = selectedUnit.name.ToUpper();
    }

    private void UpdateHealthBar(HealthSystem healthSystem)
    {
        healthbarImage.fillAmount = healthSystem.GetHealthNormalized();
    }
    private void UpdateActionPoints()
    {
       Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        actionPointsUI.text = "AP: " + selectedUnit.GetActionPoints() + " / " + selectedUnit.GetMaxActionPoints();
    }


    #region Events
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
        UpdateUnitName();
    }
    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }
    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }
    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
        UpdateUnitName();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }
    private void HealthSystem_OnDamagedPlayer(object sender, EventArgs e)
    {
        // Attempt to cast sender to HealthSystem, returns null if not possible
        HealthSystem healthSystem = sender as HealthSystem;
        if (healthSystem != null)
             UpdateHealthBar(healthSystem);
    }

    private void HealthSystem_OnHeal (object sender, EventArgs e)
    {
        // Attempt to cast sender to HealthSystem, returns null if not possible
        HealthSystem healthSystem = sender as HealthSystem;
        if (healthSystem != null)
            UpdateHealthBar(healthSystem);
    }
    #endregion

}
