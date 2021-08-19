using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateTexture3D : MonoBehaviour
{

    HandleGrid mainGrid;

    GameObject cam;

    static int width = 7;
    static int height = 7;
    static int depth = 7;
    void Start(){
        CreateTexture();
    }

    [MenuItem("Assets/Resources")]

    static void CreateTexture(){


        TextureFormat format = TextureFormat.RGBA32;
        TextureWrapMode wrapMode = TextureWrapMode.Clamp;

        Texture3D texture = new Texture3D(width, height, depth, format, false);
        texture.wrapMode = wrapMode;

        Color[] colors = new Color[width * height * depth];

        for (int z = 0; z < depth; z++){
            int zOffset = z * width * height;
            for (int y = 0; y < height; y++){
                int yOffset = y * width;
                for (int x = 0; x < width; x++){
                    float value = Random.Range(-1f, 1f);
                    colors[x + yOffset + zOffset] = new Color(value, value, value, 1f);                    
                }
            }
        }

        texture.SetPixels(colors);

        texture.Apply();


        AssetDatabase.CreateAsset(texture, "Assets/Resources/Example3DTexture.asset");
    }

}
