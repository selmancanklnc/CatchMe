using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIButtons : MonoBehaviour
{

    [SerializeField] private Button button1;
    [SerializeField] private Button button1_2;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    [SerializeField] private string defaultColorHex = "#FFFFFF";
    [SerializeField] private string selectedColorHex = "#FF0000";

    private Color defaultColor;
    private Color selectedColor;
    private Button selectedButton;


    // Start is called before the first frame update
    void Start()
    {
        // Hexadecimal renkleri Color nesnelerine dönüþtür
        ColorUtility.TryParseHtmlString(defaultColorHex, out defaultColor);
        ColorUtility.TryParseHtmlString(selectedColorHex, out selectedColor);

        // Baþlangýçta 1. buton seçili olarak ayarla
        selectedButton = button1;
        SetButtonColor(button1, selectedColor);
        SetButtonColor(button2, defaultColor);
        SetButtonColor(button3, defaultColor);

        // Butonlara týklama olaylarýný ekle
        button1.onClick.AddListener(() => SelectButton(button1));
        button1_2.onClick.AddListener(() => SelectButton(button1));
        button2.onClick.AddListener(() => SelectButton(button2));
        button3.onClick.AddListener(() => SelectButton(button3));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SelectButton(Button newSelectedButton)
    {
        if (selectedButton != newSelectedButton)
        {
            SetButtonColor(selectedButton, defaultColor);
            selectedButton = newSelectedButton;
            SetButtonColor(selectedButton, selectedColor);
        }
    }
    private void SetButtonColor(Button button, Color color)
    {
        var colors = button.colors;
        colors.normalColor = color;
        colors.highlightedColor = color;
        colors.pressedColor = color;
        colors.selectedColor = color;
        button.colors = colors;
    }


}
