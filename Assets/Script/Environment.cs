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
    public List<ObjectCategory> categories = new List<ObjectCategory>(); // Kategorileri i�eren liste
    public float animationDuration = 10f; // Animasyon s�resi (saniye)
    public float inactiveInterval = 20f; // Pasif olma aral��� (saniye)

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
            // Her d�ng� ba��nda t�m objeleri pasif yap
            DeactivateAllObjects();

            // Her kategoriden bir tane rastgele objeyi se� ve etkinle�tir
            foreach (ObjectCategory category in categories)
            {
                ActivateRandomObject(category);
            }

            // Animasyon s�resi kadar bekleyin
            yield return new WaitForSeconds(animationDuration);

            // T�m objeleri pasif yap
            DeactivateAllObjects();

            // Pasif aral��� kadar bekleyin
            yield return new WaitForSeconds(inactiveInterval);
        }
    }

    void ActivateRandomObject(ObjectCategory category)
    {
        // E�er bu kategoride zaten bir aktif obje varsa, i�lem yapma
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
