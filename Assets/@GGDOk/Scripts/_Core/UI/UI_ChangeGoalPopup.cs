using System;
using Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChangeGoalPopup : UI_Popup
{
#region Define UI  
    enum InputFields
    {
        NumberInputField
    }

    enum Buttons
    {
        Backbutton,
        MinusHundredButton,
        MinusTenButton,
        PlusTenButton,
        PlusHundredButton
    }
    enum Texts
    {
        
    }
#endregion


#region Private fields
    private TMP_InputField _inputField;
#endregion




    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind();        
    }

    private void Bind()
    {
        Bind<TMP_InputField>(typeof(InputFields));
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Get<Button>(Buttons.MinusHundredButton).onClick.AddListener(OnClickMinusHundredButton);
        Get<Button>(Buttons.MinusTenButton).onClick.AddListener(OnClickMinusTenButton);
        Get<Button>(Buttons.PlusTenButton).onClick.AddListener(OnClickPlusTenButton);
        Get<Button>(Buttons.PlusHundredButton).onClick.AddListener(OnClickPlusHundredButton);
        Get<Button>(Buttons.Backbutton).onClick.AddListener(CloseButton);
        
        _inputField = Get<TMP_InputField>(InputFields.NumberInputField);
    }
    
    
    
    
#region ButtonEvents
/// <summary>
/// 팝업 끄기
/// </summary>
    private void CloseButton()
    {
        ClosePopupUI();   
    }
/// <summary>
/// 목표 개수 -100
/// </summary>
    private void OnClickMinusHundredButton()
    {
        Debug.Log("-100");
    }
/// <summary>
/// 목표 개수 -10
/// </summary>
    private void OnClickMinusTenButton()
    {
        Debug.Log("-10");
    }
/// <summary>
/// 목표 개수 +10
/// </summary>
    private void OnClickPlusTenButton()
    {
        Debug.Log("+10");
    }
/// <summary>
/// 목표 개수 +100
/// </summary>
    private void OnClickPlusHundredButton()
    {
        Debug.Log("+100");
    }
    #endregion
}
