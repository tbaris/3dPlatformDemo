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
       
        PaintBall.GetComponent<Image>().color = disabled; //change paint icon color to gray
        RaycastHit hit;
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            return;

        
        if (!(hit.transform.name == "PaintingSurface"))
            
            return;
        PaintBall.GetComponent<Image>().color = Color.white; //if camera aims to the wall change paint icon color to normal


        if (!Input.GetMouseButton(0))// ends if mouse not clicked
            return;

       

        Renderer rend = hit.transform.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;

        if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null) // checks if there is a paintable texture
            return;

        Texture2D tex = rend.material.mainTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord;

        pixelUV.x *= tex.width; //convert hit coordinates to texture pixel info
        pixelUV.y *= tex.height;

        for(int a = Mathf.RoundToInt(pixelUV.x) -2 ; a < pixelUV.x + 2; a++) //paints aimed point and a square around it
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

       
    }
}
