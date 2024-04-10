using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectCategory
{
    public string categoryName;
    public List<GameObject> objectsInCategory;
    [HideInInspector] public int activeObjectIndex = -1;
}

public class Environment : MonoBehaviour
{
    public List<ObjectCategory> categories = new List<ObjectCategory>(); // Kategorileri içeren liste
    public float animationDuration = 10f; // Animasyon süresi (saniye)
    public float inactiveInterval = 20f; // Pasif olma aralýðý (saniye)

    private float timer = 0f;
    private bool isActive = true;

    void Start()
    {
        StartCoroutine(ActivateRandomObjectRoutine());
    }

    IEnumerator ActivateRandomObjectRoutine()
    {
        while (true)
        {
            // Her döngü baþýnda tüm objeleri pasif yap
            DeactivateAllObjects();

            // Her kategoriden bir tane rastgele objeyi seç ve etkinleþtir
            foreach (ObjectCategory category in categories)
            {
                ActivateRandomObject(category);
            }

            // Animasyon süresi kadar bekleyin
            yield return new WaitForSeconds(animationDuration);

            // Tüm objeleri pasif yap
            DeactivateAllObjects();

            // Pasif aralýðý kadar bekleyin
            yield return new WaitForSeconds(inactiveInterval);
        }
    }

    void ActivateRandomObject(ObjectCategory category)
    {
        // Eðer bu kategoride zaten bir aktif obje varsa, iþlem yapma
        if (category.activeObjectIndex != -1)
        {
            return;
        }

        int randomIndex = Random.Range(0, category.objectsInCategory.Count);
        GameObject randomObject = category.objectsInCategory[randomIndex];
        randomObject.SetActive(true);
        category.activeObjectIndex = randomIndex;
    }

    void DeactivateAllObjects()
    {
        foreach (ObjectCategory category in categories)
        {
            if (category.activeObjectIndex != -1)
            {
                category.objectsInCategory[category.activeObjectIndex].SetActive(false);
                category.activeObjectIndex = -1;
            }
        }
    }
}
