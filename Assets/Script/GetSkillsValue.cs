using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetSkillsValue : MonoBehaviour
{
    public TMP_Text skillValue1;
    public TMP_Text skillValue2;
    public TMP_Text skillValue3;
    public TMP_Text skillValue4;


    // Start is called before the first frame update
    void Start()
    {
        skillValue1.text = Inventory.OneHitSkill.ToString();
        skillValue2.text = Inventory.MeteorSkill.ToString();
        skillValue3.text = Inventory.ChangeReailtySkill.ToString();
        skillValue4.text = Inventory.TimeShiftSkill.ToString();



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
