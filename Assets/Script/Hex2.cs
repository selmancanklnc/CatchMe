using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hex2 : MonoBehaviour
{
    public GameObject altigenPrefab; // Alt�gen prefab�
    public int satirSayisi = 5; // Bal pete�indeki sat�r say�s�
    public int sutunSayisi = 7; // Bal pete�indeki s�tun say�s�
    public float altigenGenislik = 1.0f; // Alt�genlerin geni�li�i
    public float aralik = 0.1f; // Alt�genler aras�ndaki bo�luk

    private void Start()
    {
        // Paneli bul
        RectTransform panel = GetComponent<RectTransform>();

        Vector2 panelBoyutu = panel.sizeDelta;

        Vector2 altigenBoyutu = new Vector2(altigenGenislik, altigenGenislik);

        // Panelin alt�nda alt�genleri olu�turmak i�in ba�lang�� pozisyonunu hesapla
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

                // Panel i�inde alt�genleri olu�tur
                GameObject altigen = Instantiate(altigenPrefab, panel);
                altigen.GetComponent<RectTransform>().anchoredPosition = pozisyon;
            }
        }
    }
}
