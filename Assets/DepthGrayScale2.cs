using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthGrayScale2 : MonoBehaviour
{

    Vector3[] positions;
    float[] array;
    float[] lastFrameArray;
    TestTask tTask;

    public float[,] texture;

    public RenderTexture rTex;

    RenderTexture texA;
    RenderTexture texB;

    public Material mat;
    //    public Material _kernelMaterial;
    public int M;
    public int N;
    public Shader shader;
    Camera mCamera;
    public RenderTexture initTexture;
    Texture2D decTex;

    Camera ppCamera;

    public GameObject go1;
    public GameObject go2;

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
        // mCamera.
        //mCamera.targetTexture.name = "lol";
        // mCamera.ResetReplacementShader();
        //initTexture = mCamera.targetTexture;


        //mCamera.depthTextureMode = DepthTextureMode.MotionVectors;
    }

    void OnEnable()
    {
        texA = RenderTexture.GetTemporary(Screen.width, Screen.height, 32, RenderTextureFormat.ARGBFloat);
        texB = RenderTexture.GetTemporary(Screen.width, Screen.height, 32, RenderTextureFormat.ARGBFloat);
    }

    void OnDisable()
    {
        RenderTexture.ReleaseTemporary(texA);
        RenderTexture.ReleaseTemporary(texB);
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
        //Shader.SetGlobalFloat("_test", 4);
        array = DecodeFloatTexture();
        tTask.setArray1(array);

        float[] arrayOfUV = new float[M*N*2];
        float[] arrayOfMyU = new float[M * N];
        float[] arrayOfMyV = new float[M * N];
        float[] arrayOfV = new float[M * N];
        float[] arrayOfF = new float[M * N];
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
        
        //do something with the array

        lastFrameArray = array;
        

        //DecodeFloatTexture(mCamera.targetTexture);
    }

    
}