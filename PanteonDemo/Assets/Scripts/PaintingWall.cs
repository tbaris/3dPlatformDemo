using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PaintingWall : MonoBehaviour
{
    public int redPixels = 0;
    public TextMeshProUGUI redDisplay;

    private Texture2D texture;
    
    // Start is called before the first frame update
    void Start()
    {
        texture = new Texture2D(64, 64);
        GetComponent<Renderer>().material.mainTexture = texture;

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                
                texture.SetPixel(x, y, Color.white);
            }
        }
        texture.Apply();
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void CountPixels(Color countColor)
    {
        redPixels = 0;
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                if (texture.GetPixel(x, y) == countColor)
                {
                    redPixels++;
                }

            }
        }

        redDisplay.text = ((redPixels * 100) / (texture.height * texture.width)).ToString() + "%";
    }
}
