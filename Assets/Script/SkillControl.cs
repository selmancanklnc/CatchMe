using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;


public class SkillControl : MonoBehaviour
{
    public Material woodMaterial;
    public static int oldPosition = 0;
    public static bool isMeteorActive = false;
    public int meteorCount = 3;
    public static int position = 0;
    public GameObject player;
    public static bool oneHitActive = false;
    public static bool timeshiftActive = false;

    static int meteorSkill { get; set; }
    static int oneHitSkill { get; set; }
    static int changeReailtySkill { get; set; }
    static int timeShiftSkill { get; set; }


    bool realitycontrol = false;
    public GameObject realityAnimation;


    public GameObject timeshiftAnimation;
    public new ParticleSystem particleSystem;
    public GameObject timeshiftLight;
    private float activeDuration = 1.8f;


    public TMP_Text skill1Value;
    public TMP_Text skill2Value;
    public TMP_Text skill3Value;
    public TMP_Text skill4Value;


    public GameObject meteorPrefab;
    public GameObject explosionPrefab; // Patlama efekti prefabýný ekleyin



    public GameObject skillTextPanel;
    public GameObject onehitTextPanel;
    public GameObject meteorTextPanel;
    public GameObject changeRealityTextPanel;
    public GameObject timeshiftTextPanel;

    public GameObject meteorskillImageClone;
    public GameObject changeRealityskillImageClone;
    public GameObject timeshiftskillImageClone;

    // Start is called before the first frame update
    void Start()
    {
        meteorSkill = Inventory.MeteorSkill;
        oneHitSkill = Inventory.OneHitSkill;
        changeReailtySkill = Inventory.ChangeReailtySkill;
        timeShiftSkill = Inventory.TimeShiftSkill;



        //test(kapatýlacak)
        //meteorSkill = 10;
        //oneHitSkill = 10;
        //changeReailtySkill = 10;
        //timeShiftSkill = 10;




       
           
      


    }





    // Update is called once per frame
    void Update()
    {
        skill1Value.text = oneHitSkill.ToString();
        skill2Value.text = meteorSkill.ToString();
        skill3Value.text = changeReailtySkill.ToString();
        skill4Value.text = timeShiftSkill.ToString();


    }
    void ShowSkillText(SkillTypeEnum skillType)
    {

        if (skillType == SkillTypeEnum.OneHitSkill)
        {
            skillTextPanel.SetActive(true);
            onehitTextPanel.SetActive(true);
            StartCoroutine(OneHitDeactive(2f));
        }

        if (skillType == SkillTypeEnum.MeteorSkill)
        {
            skillTextPanel.SetActive(true);
            meteorTextPanel.SetActive(true);
            StartCoroutine(MeteorDeactive(2f));
        }
        if (skillType == SkillTypeEnum.ChangeReailtySkill)
        {
            skillTextPanel.SetActive(true);
            changeRealityTextPanel.SetActive(true);
            StartCoroutine(ChangeRealityDeactive(2f));
        }
        if (skillType == SkillTypeEnum.TimeShiftSkill)
        {
            skillTextPanel.SetActive(true);
            timeshiftTextPanel.SetActive(true);
            StartCoroutine(TimeShiftDeactive(2f));
        }
    }
    private IEnumerator OneHitDeactive(float delay)
    {
        yield return new WaitForSeconds(delay);

        skillTextPanel.SetActive(false);
        onehitTextPanel.SetActive(false);
    }
    private IEnumerator MeteorDeactive(float delay)
    {
        yield return new WaitForSeconds(delay);

        skillTextPanel.SetActive(false);
        meteorTextPanel.SetActive(false);
    }
    private IEnumerator ChangeRealityDeactive(float delay)
    {
        yield return new WaitForSeconds(delay);

        skillTextPanel.SetActive(false);
        changeRealityTextPanel.SetActive(false);
    }
    private IEnumerator TimeShiftDeactive(float delay)
    {
        yield return new WaitForSeconds(delay);

        skillTextPanel.SetActive(false);
        timeshiftTextPanel.SetActive(false);
    }

    public void Meteor()
    {
        if (meteorCount <= 0)
        {
            return;
        }
        if (oneHitActive)
        {
            return;
        }
        var skill = meteorSkill;
        if (skill < 1)
        {
            return;
        }

        var floors = GameObject.FindGameObjectsWithTag("floor").Where(a => a.name != $"floor{position}" && a.activeSelf).ToList();
        floors.ShuffleMe();
        var newFloors = floors.Take(3).ToList();
        foreach (var item in newFloors)
        {
            StartCoroutine(SpawnMeteor(item));
            Config.closedObjects.Add(item);
        }
        skill--;
        meteorSkill = skill;
        isMeteorActive = true;
        ShowSkillText(SkillTypeEnum.MeteorSkill);
        meteorCount--;
        var instance = FloorCodes.GetInstance();
        if (instance != null)
        {
            instance.WinCheck();
        }
    }

    IEnumerator SpawnMeteor(GameObject floor)
    {
        Vector3 floorPosition = floor.transform.position;
        Vector3 spawnPosition = new Vector3(floorPosition.x, floorPosition.y + 35, floorPosition.z);
        GameObject meteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);

        Rigidbody meteorRigidbody = meteor.GetComponent<Rigidbody>();
        meteorRigidbody.velocity = new Vector3(0, -30, 0); // Baþlangýç hýzýný ayarlayýn

        // Meteorun zemine çarpmasýný bekleyin
        yield return new WaitForSeconds(1);

        // Meteorun zemine çarptýðýný varsayarak zemini kapatýn
        floor.SetActive(false);

        // Patlama efektini baþlatýn
        GameObject explosion = Instantiate(explosionPrefab, new Vector3(floorPosition.x, floorPosition.y, floorPosition.z), Quaternion.identity);
        Destroy(explosion, 2f); // Patlama efektini 3 saniye sonra yok edin

        // Meteoru yok edin
        Destroy(meteor);
    }

    public void ChangeReality()
    {
        if (oneHitActive)
        {
            return;
        }



        if (realitycontrol == false)
        {
            var skill = changeReailtySkill;
            if (skill < 1)
            {
                return;
            }
            var floors = GameObject.FindGameObjectsWithTag("floor").Where(a => a.activeSelf).ToList();
            if (floors.All(item => item.GetComponent<Renderer>().material.mainTexture == woodMaterial.mainTexture))
            {
                return;
            }
            realitycontrol = true;
            ShowSkillText(SkillTypeEnum.ChangeReailtySkill);
            realityAnimation.SetActive(true);
         

            foreach (var item in floors)
            {
                item.GetComponent<Renderer>().material = woodMaterial;

            }
            skill--;
            changeReailtySkill = skill;
        }


    }
    public void TimeShift()
    {
        if (oneHitActive)
        {
            return;
        }
        if (timeshiftActive)
        {
            var skill = timeShiftSkill;
            if (skill < 1)
            {
                return;
            }
            var target = GameObject.Find($"floor{oldPosition}").transform;
            position = oldPosition;
            player.transform.position = target.position;

            if (!timeshiftAnimation.activeSelf)
            {
                timeshiftAnimation.SetActive(true);
            }

            timeshiftActive = false;
            particleSystem.Play();
            StartCoroutine(ActivateAndDeactivate());
            ShowSkillText(SkillTypeEnum.TimeShiftSkill);
            skill--;
            timeShiftSkill = skill;

        }
        else
        {
            return;
        }

    }
    private IEnumerator ActivateAndDeactivate()
    {
        while (true)
        {
            timeshiftLight.SetActive(true); // GameObject'i aktif hale getirin

            yield return new WaitForSeconds(activeDuration); // Belirlediðiniz süre boyunca bekleyin

            timeshiftLight.SetActive(false); // GameObject'i devre dýþý býrakýn

            yield break; // Coroutine'i sonlandýrýn
        }
    }
    public void OneHit()
    {
        if (oneHitActive)
        {
            return;
        }
        var skill = oneHitSkill;
        if (skill < 1)
        {
            return;
        }
        oneHitActive = true;
        ShowSkillText(SkillTypeEnum.OneHitSkill);
        skill--;
        oneHitSkill = skill;
        meteorskillImageClone.SetActive(true);
        changeRealityskillImageClone.SetActive(true);
        timeshiftskillImageClone.SetActive(true);

    }
    public static void UpdateSkill()
    {
        Inventory.MeteorSkill = meteorSkill;
        Inventory.OneHitSkill = oneHitSkill;
        Inventory.TimeShiftSkill = timeShiftSkill;
        Inventory.ChangeReailtySkill = changeReailtySkill;
        FirestoreExample.UpdateSkill();
    }
}
enum SkillTypeEnum : byte
{
    MeteorSkill, OneHitSkill, TimeShiftSkill, ChangeReailtySkill

}
