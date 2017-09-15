using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthGrayScale3: MonoBehaviour
{

    Vector3[] positions;
    float[] array;
    TestTask tTask;

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

    public int countTrues = 0;
    public int countFalses = 0;

    public float testt = 0.0f;
    // Use this for initialization
    void Awake()
    {
        
        mCamera = this.GetComponent<Camera>();
        rTex = mCamera.targetTexture;
        rTex.width = M;
        rTex.height = N;
        mCamera.SetReplacementShader(shader, "");
        
        decTex = new Texture2D(M, N, TextureFormat.RGBAFloat, false);
       // mCamera.
        //mCamera.targetTexture.name = "lol";
       // mCamera.ResetReplacementShader();
        //initTexture = mCamera.targetTexture;
       

        mCamera.depthTextureMode = DepthTextureMode.MotionVectors;
        tTask = transform.parent.GetComponent<TestTask>();
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
        //Shader.SetGlobalFloat("_test", testt);
        Graphics.Blit(source, destination);
        array = DecodeFloatTexture();
        //Debug.Log("test = " + Shader.GetGlobalFloat("_test"));
        tTask.setArray2(array);


        //Debug.Log("time between = " + tTask.getTimeBetweenFrames() + " frames are equal =" + tTask.compareTwoArrays() );
        if (tTask.compareTwoArrays())
        {
            tTask.countTrues++;
        }
        else
            tTask.countFalses++;
        //DecodeFloatTexture(mCamera.targetTexture);
    }

}
