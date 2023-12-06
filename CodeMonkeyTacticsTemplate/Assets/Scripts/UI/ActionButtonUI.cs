using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedBoarder;

    private BaseAction action;

    public void SetBaseAction(BaseAction action)
    {
        this.action = action;
        textMeshPro.text = action.GetActionName().ToUpper();

        //Set up button click event 
        //creating ananomous fucntions
        button.onClick.AddListener(() =>{
            UnitActionSystem.Instance.SetSelectedAction(action);
        });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedBoarder.SetActive(selectedBaseAction == action);
    }
}
