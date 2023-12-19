using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;

    // Start is called before the first frame update
    void Start()
    {
        Unit.OnAnyActionPointChanged += Unit_OnAnyActionPointsChanged;
        UpdateActionPointsText();
        UpdateHealthBar();

        healthSystem.OnDamagedNPC += healthSystem_OnDamaged;

    }
    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    #region Events
    private void healthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
    #endregion
}
