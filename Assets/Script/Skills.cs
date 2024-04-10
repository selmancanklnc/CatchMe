using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Skills : MonoBehaviour
{
    private int currentBalance = 0; // Baþlangýç bakiyesi
    private int selectedSkillIndex;
    public GameObject popupPanel;
    public TMP_Text popUpText;


    public TMP_Text skillValue1;
    public TMP_Text skillValue2;
    public TMP_Text skillValue3;
    public TMP_Text skillValue4;
    public TMP_Text skillValue5;
    public TMP_Text skillValue6;
    public TMP_Text skillValue7;
    public TMP_Text skillValue8;


    private void Start()
    {
        currentBalance = Inventory.Coin;

        UpdateBalanceText();
    }
  
    public void PurchaseSkill()
    {

        Skill selectedSkill = skills[selectedSkillIndex];
        if (currentBalance >= selectedSkill.price)
        {
            currentBalance -= selectedSkill.price;
            Inventory.Coin = currentBalance;
            if (selectedSkill.skillName=="One Hit")
            {
                var skill = Inventory.OneHitSkill;
                skill++;
                Inventory.OneHitSkill = skill;
                
            }
            if (selectedSkill.skillName == "Meteor")
            {
                var skill = Inventory.MeteorSkill;
                skill++;
                Inventory.MeteorSkill = skill;
            }
            if (selectedSkill.skillName == "Change Reality")
            {
                var skill = Inventory.ChangeReailtySkill;
                skill++;
                Inventory.ChangeReailtySkill = skill;
            }
            if (selectedSkill.skillName == "Time Shift")
            {
                var skill = Inventory.TimeShiftSkill;
                skill++;
                Inventory.TimeShiftSkill = skill;
            }
             

            FirestoreExample.UpdateSkill();
            UpdateBalanceText();

            // Yeteneði satýn alýndý olarak uyar.
            popupPanel.SetActive(true);
            popUpText.text = "Skill purchased.";
            
        }
        else
        {
            // Yetersiz bakiye ile ilgili bir uyarý göster.
            popupPanel.SetActive(true);
            popUpText.text = "Insufficient Coins!";
            
        }

      

    }

    private void UpdateBalanceText()
    {
        balanceText.text = $"{currentBalance}";
        skillValue1.text = Inventory.OneHitSkill.ToString();
        skillValue2.text = Inventory.MeteorSkill.ToString();
        skillValue3.text = Inventory.ChangeReailtySkill.ToString();
        skillValue4.text = Inventory.TimeShiftSkill.ToString();
        skillValue5.text = Inventory.OneHitSkill.ToString();
        skillValue6.text = Inventory.MeteorSkill.ToString();
        skillValue7.text = Inventory.ChangeReailtySkill.ToString();
        skillValue8.text = Inventory.TimeShiftSkill.ToString();
    }

    [System.Serializable]
    public class Skill
    {
        public string skillName;
        public string skillDescription;
        public string skillPrice;
        public Sprite skillIcon;
        public int price;
    }


    [SerializeField] private Skill[] skills;
    [SerializeField] private GameObject skillDetailPanel;
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text skillDescriptionText;
    [SerializeField] private TMP_Text skillPrice;
    [SerializeField] private Image skillIconImage;
    [SerializeField] private TMP_Text balanceText;

    public void ShowSkillDetails(int skillIndex)
    {
        selectedSkillIndex = skillIndex;
        Skill selectedSkill = skills[skillIndex];
        skillNameText.text = selectedSkill.skillName;
        skillDescriptionText.text = selectedSkill.skillDescription;
        skillPrice.text = selectedSkill.skillPrice;
        skillIconImage.sprite = selectedSkill.skillIcon;

        skillDetailPanel.SetActive(true);
    }
}
