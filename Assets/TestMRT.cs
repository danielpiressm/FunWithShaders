using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]

public class TestMRT : MonoBehaviour
{
    private Material testMRTMaterial = null;
    private RenderTexture[] mrtTex = new RenderTexture[2];
    private RenderBuffer[] mrtRB = new RenderBuffer[2];
    public Material mat;
    Texture2D decTex;

    void Start()
    {
        mrtTex[0] = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGBFloat);
        mrtTex[1] = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGBFloat);
        mrtRB[0] = mrtTex[0].colorBuffer;
        mrtRB[1] = mrtTex[1].colorBuffer;
        decTex = new Texture2D(3, 3, TextureFormat.RGBAFloat, false);
        this.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
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

    /*public override bool CheckResources()
    {
        CheckSupport(true);

        testMRTMaterial = CheckShaderAndCreateMaterial (Shader.Find("Custom/TestMRT"), testMRTMaterial);

        if (!isSupported)
            ReportAutoDisable();
        return isSupported;
    }*/

    void OnPreRender()
    {
        

    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var rt1 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGBFloat);
        var rt2 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGBFloat);

        mrtRB[0] = rt1.colorBuffer;
        mrtRB[1] = rt2.colorBuffer;

       // RTUtility.MultiTargetBlit(mrtTex, mat);

        // Blit with a MRT.
        Graphics.SetRenderTarget(mrtRB, rt1.depthBuffer);
        Graphics.Blit(null, mat, 0);

        var rt3 = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGBFloat);
        float[] array = DecodeFloatTexture(rt1);
        float[] array2 = DecodeFloatTexture(rt2);

        RenderTexture.ReleaseTemporary(rt1);
        RenderTexture.ReleaseTemporary(rt2);
    }

    void OnPostRender()
    {
        
        /*RTUtility.MultiTargetBlit(mrtTex, mat, 0);
        float[] array1 = DecodeFloatTexture(mrtTex[0]);
        float[] array2 = DecodeFloatTexture(mrtTex[1]);*/
    }

    void OnRenderImage2(RenderTexture source, RenderTexture destination)
    {
        /*if (CheckResources() == false)
        {
            Graphics.Blit(source, destination);
            return;
        }*/

        RenderTexture oldRT = RenderTexture.active;

        Graphics.SetRenderTarget(mrtRB, mrtTex[0].depthBuffer);

        GL.Clear(false, true, Color.clear);

        GL.PushMatrix();
        GL.LoadOrtho();

        testMRTMaterial.SetPass(0);     //Pass 0 outputs 2 render textures.

        //Render the full screen quad manually.
        GL.Begin(GL.QUADS);
        GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(0.0f, 0.0f, 0.1f);
        GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, 0.0f, 0.1f);
        GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 0.1f);
        GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(0.0f, 1.0f, 0.1f);
        GL.End();

        GL.PopMatrix();

        RenderTexture.active = oldRT;

        //Show the result
        testMRTMaterial.SetTexture("_Tex0", mrtTex[0]);
        testMRTMaterial.SetTexture("_Tex1", mrtTex[1]);
        Graphics.Blit(source, destination, testMRTMaterial, 1);
    }
}