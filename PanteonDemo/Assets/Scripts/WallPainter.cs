using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallPainter : MonoBehaviour
{
    public Camera cam;
    public GameObject PaintBall;
    public Color disabled = Color.black;
    public Color paintColor = Color.red;

    void Start()
    {
        cam = GetComponent<Camera>();
        PaintBall.SetActive(true);
    }

    void Update()
    {
       
        PaintBall.GetComponent<Image>().color = disabled;
        RaycastHit hit;
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            return;

        
        if (!(hit.transform.name == "PaintingSurface"))
            
            return;
        PaintBall.GetComponent<Image>().color = Color.white;


        if (!Input.GetMouseButton(0))
            return;

       

        Renderer rend = hit.transform.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;

        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
            return;

        Texture2D tex = rend.material.mainTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;

        for(int a = Mathf.RoundToInt(pixelUV.x) -2 ; a < pixelUV.x + 2; a++)
        {
            for (int b = Mathf.RoundToInt(pixelUV.y) - 2; b < pixelUV.y + 2; b++)
            {
                if(a < tex.height && a >= 0 && b < tex.width && b >= 0)
                {
                    tex.SetPixel(a, b, paintColor);
                    tex.Apply();
                    hit.transform.GetComponent<PaintingWall>().CountPixels(paintColor);
                }
            }
        }

       /*tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.red);
        tex.Apply();
        hit.transform.GetComponent<PaintingWall>().CountPixels();*/
    }
}
