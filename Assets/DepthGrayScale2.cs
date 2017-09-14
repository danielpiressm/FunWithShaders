using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthGrayScale2 : MonoBehaviour
{

    Vector3[] positions;
    public float[] radiuses;
    public float[] intensities;
    float[] array;

    public float[,] texture;

    public RenderTexture rTex;

    public Material mat;
    //    public Material _kernelMaterial;
    public int M;
    public int N;
    public Shader shader;
    Camera mCamera;
    public RenderTexture initTexture;
    Texture2D decTex;
    // Use this for initialization
    void Awake()
    {
        
        mCamera = this.GetComponent<Camera>();
        rTex = mCamera.targetTexture;
        mCamera.SetReplacementShader(shader, "");
        decTex = new Texture2D(M, N, TextureFormat.RGBAFloat, false);
        //mCamera.targetTexture.name = "lol";
       // mCamera.ResetReplacementShader();
        //initTexture = mCamera.targetTexture;
       

        mCamera.depthTextureMode = DepthTextureMode.Depth;
    }

    float[] DecodeFloatTexture()
    {
        RenderTexture.active = rTex;
        decTex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        decTex.Apply();
        RenderTexture.active = null;
        Color[] colors = decTex.GetPixels();

        float[] results = new float[colors.Length * 4];
        Vector4 point = new Vector4();
        for (int i = 0; i < colors.Length; i++)
        {
            results[i * 4] = colors[i].r;
            results[i * 4 + 1] = colors[i].g;
            results[i * 4 + 2] = colors[i].b;
            results[i * 4 + 3] = colors[i].a;
            point = new Vector4(colors[i].r, colors[i].g, colors[i].b, colors[i].a);
        }
        return results;
    }

    float[] DecodeFloatTexture(RenderTexture tex)
    {
        Texture2D decTex = new Texture2D(tex.width, tex.height, TextureFormat.RGBAFloat, false);
        RenderTexture.active = tex;
        decTex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        decTex.Apply();
        RenderTexture.active = null;
        Color[] colors = decTex.GetPixels();
        
        float[] results = new float[colors.Length * 4];
        //Vector4 point = new Vector4();
        for (int i = 0; i < colors.Length; i++)
        {
            results[i * 4] = colors[i].r;
            results[i * 4 + 1] = colors[i].g;
            results[i * 4 + 2] = colors[i].b;
            results[i * 4 + 3] = colors[i].a;
          //  point = new Vector4(colors[i].r, colors[i].g, colors[i].b, colors[i].a);
        }
        return results;
    }

    

    // Update is called once per frame
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination);
        array = DecodeFloatTexture();
        //DecodeFloatTexture(mCamera.targetTexture);
    }

}
