using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DepthGrayScale : MonoBehaviour {

    Vector3[] positions;
    public float[] radiuses;
    public float[] intensities;

    public float[,] texture;

    public RenderTexture rTex;

    public Material mat;

	// Use this for initialization
	void Start () {
        int M = 256;
        int N = 256;
        //rTex = new RenderTexture(256, 256, 0, RenderTextureFormat.ARGBFloat);
        positions = new Vector3[M*N];

        for(int i = 0; i < M;i++)
        {
            for(int j = 0; j < N;j++)
            {
                positions[j + N * i] = new Vector3(2,2,2);
            }
        }

        for(int i = 0;i < 9;i++)
        {
            mat.SetInt("_Point" + i.ToString(),2);
        }


       // this.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        mat.SetInt("_Points_Length", positions.Length);
        for(int i = 0; i < positions.Length; i++)
        {
            mat.SetVector("_Points" + i.ToString(), positions[i]);
        }
	}

    float[] DecodeFloatTexture()
    {
        Texture2D decTex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBAFloat, false);
        RenderTexture.active = rTex;
        decTex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        decTex.Apply();
        RenderTexture.active = null;
        Color[] colors = decTex.GetPixels();
        // HERE YOU CAN GET ALL 4 FLOATS OUT OR JUST THOSE YOU NEED.
        // IN MY CASE ALL 4 VALUES HAVE A MEANING SO I'M GETTING THEM ALL.
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
    void Update () {
        /*if(Input.GetKeyDown("x"))
        {
            string str = "Array = [";
            for(int i = 0; i < mat.GetInt("_Points_Length");i++)
            {
                Vector4 point = mat.GetVector("_Points" + i.ToString());
                positions[i] = new Vector3(point.x, point.y, point.z);
            }
            //Vector4[] pointts = mat.GetVector("_Points");
            int x = 2;
            //int z = 2;
            for(int i = 0;i < 9; i++)
            {
                x = mat.GetInt("_Point" + i);
            }
            mat.SetInt("_Point1", 4);
            Debug.Log("BLA = " + mat.GetInt("_Point1"));
            mat.SetVector("_Points" + 4, new Vector4(10, 11, 12, 13));

            Vector4 point2 = mat.GetVector("_Points" + 0);
            Debug.Log("Vector = " + point2.w + "," + point2.x + "," + point2.y + "," + point2.z);

            //DecodeFloatTexture();
        }*/
        
		//mat.GetVector("_Points", )
	}

   


    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
      //  rTex = source;
      //  DecodeFloatTexture();
      
    }
}
