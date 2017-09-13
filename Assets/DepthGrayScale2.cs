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
    // Use this for initialization
    void Awake()
    {
        rTex = CreateBuffer();
        mat = CreateMaterial(shader);
        mCamera = this.GetComponent<Camera>();
        rTex = mCamera.targetTexture;
        mCamera.SetReplacementShader(shader, "");
        //mCamera.targetTexture.name = "lol";
       // mCamera.ResetReplacementShader();
        //initTexture = mCamera.targetTexture;
       

        //mCamera.depthTextureMode = DepthTextureMode.Depth;
    }

    float[] DecodeFloatTexture()
    {
        Texture2D decTex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBAFloat, false);
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

    

    // Update is called once per frame
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination);
        GameObject obj = GameObject.Find("SQUARE");
        obj.GetComponent<Renderer>().material.mainTexture = mCamera.targetTexture;
        array = DecodeFloatTexture(mCamera.targetTexture);

        float max = 0;
        float min = -10;
        for (int i = 0; i < array.Length; i++)
        {
            if (max > array[i])
                max = array[i];
            if (min <= array[i] && array[i] != 0)
                min = array[i];
        }

        Debug.Log("max = " + max + " min =" + min);
    }


  void Update()
    {
       // mCamera.SetReplacementShader(shader, "");

    }



    RenderTexture CreateBuffer()
    {
        var buffer = new RenderTexture(M, N, 0, RenderTextureFormat.ARGB32);
        buffer.hideFlags = HideFlags.DontSave;
        buffer.filterMode = FilterMode.Point;
        buffer.wrapMode = TextureWrapMode.Repeat;
       
        return buffer;
    }

    Material CreateMaterial(Shader shader)
    {
        var material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;
        return material;
    }
}
