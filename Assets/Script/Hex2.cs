using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hex2 : MonoBehaviour
{
    public GameObject altigenPrefab; // Altýgen prefabý
    public int satirSayisi = 5; // Bal peteðindeki satýr sayýsý
    public int sutunSayisi = 7; // Bal peteðindeki sütun sayýsý
    public float altigenGenislik = 1.0f; // Altýgenlerin geniþliði
    public float aralik = 0.1f; // Altýgenler arasýndaki boþluk

    private void Start()
    {
        // Paneli bul
        RectTransform panel = GetComponent<RectTransform>();

        Vector2 panelBoyutu = panel.sizeDelta;

        Vector2 altigenBoyutu = new Vector2(altigenGenislik, altigenGenislik);

        // Panelin altýnda altýgenleri oluþturmak için baþlangýç pozisyonunu hesapla
        Vector2 baslangicPozisyon = new Vector2(
            -panelBoyutu.x / 2 + altigenBoyutu.x / 2,
            -panelBoyutu.y / 2 + altigenBoyutu.y / 2
        );

        for (int i = 0; i < satirSayisi; i++)
        {
            for (int j = 0; j < sutunSayisi; j++)
            {
                Vector2 pozisyon = new Vector2(
                    baslangicPozisyon.x + (j * (altigenBoyutu.x + aralik)),
                    baslangicPozisyon.y + (i * (altigenBoyutu.y + aralik))
                );

                // Panel içinde altýgenleri oluþtur
                GameObject altigen = Instantiate(altigenPrefab, panel);
                altigen.GetComponent<RectTransform>().anchoredPosition = pozisyon;
            }
        }
    }
}
