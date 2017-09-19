using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthGrayScale2 : MonoBehaviour
{
    float[] array;
    float[] lastFrameArray;
    TestTask tTask;


    public RenderTexture rTex;
    public RenderTexture lastFrameTex;


    public Material mat;
    public int M;
    public int N;
    public Shader shader;
    Camera mCamera;
    Texture2D decTex;
    int countFrames = 0;

    public float testt = 0.0f;
    // Use this for initialization
    void Awake()
    {
        decTex = new Texture2D(M, N, TextureFormat.RGBAFloat, false);

        mCamera = this.GetComponent<Camera>();

        rTex = mCamera.targetTexture;
        rTex.width = M;
        rTex.height = N;
        mCamera.SetReplacementShader(shader, "");
        
        decTex = new Texture2D(M, N, TextureFormat.RGBAFloat, false);
        tTask = transform.parent.GetComponent<TestTask>();
        rTex.width = M;
        rTex.height = N;
        Shader.SetGlobalInt("sizeImage", M);
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
        array = DecodeFloatTexture();
        tTask.setArray1(array);

        float[] arrayOfUV = new float[M*N*2];
        int j = 0;
        for(int i = 3;i < array.Length;i+=4)
        {
            float F = array[i];
            float u = Mathf.Floor(F);
            float v = (F - u);
            arrayOfUV[j] = u;
            arrayOfUV[j + 1] = v;
            j+=2;
        }
        RenderTexture rTex2 = new RenderTexture(rTex.width, rTex.height, 16, RenderTextureFormat.ARGBFloat);

        //a partir daqui sao contas marotas (e logicamente so consigo fazer isso após 1 frame :-) )
        if(countFrames > 0)
        {
            Shader.SetGlobalTexture("_floatArray", rTex);
            Shader.SetGlobalTexture("_lastFrameArray", lastFrameTex);
            Graphics.Blit(rTex, rTex2, mat);
            float[] tmpArray = DecodeFloatTexture(rTex2);
        }

        //agora guarda o frame anterior :-)
        lastFrameTex = rTex;
        lastFrameArray = array;
        countFrames++;
    }

    
}