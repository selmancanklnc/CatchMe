using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilAvatar : MonoBehaviour
{
    [SerializeField] private RawImage mainMenuProfileImage;
    [SerializeField] private List<Texture> profileImages;

    //private const string ProfileImageKey = "ProfileImageIndex";
    private string userId;
    public int textureWidth = 256;
    public int textureHeight = 256;


    // Start is called before the first frame update
    void Start()
    {

        userId = Config.userId;
        // Kaydedilmiþ profil resmini yükle
        //int savedIndex = PlayerPrefs.GetInt(ProfileImageKey, 0);
        ChangeProfileImage(Config.avatarIndex);

    }

    public void ChangeProfileImage(int index)
    {
        if (index >= 0 && index < profileImages.Count)
        {
            mainMenuProfileImage.texture = profileImages[index];
            Config.SaveAvatarIndex(userId, index);



            RenderTexture renderTexture = new RenderTexture(textureWidth, textureHeight, 24);
            Graphics.Blit(mainMenuProfileImage.texture, renderTexture);
            RenderTexture.active = renderTexture;

            Texture2D texture = new Texture2D(textureWidth, textureHeight);
            texture.ReadPixels(new Rect(0, 0, textureWidth, textureHeight), 0, 0);
            texture.Apply();

            byte[] pngData = texture.EncodeToPNG();
            string base64Image = System.Convert.ToBase64String(pngData);
            Config.avatarImage = base64Image;


            RenderTexture.active = null;
            Destroy(renderTexture);




        }
    }
}
