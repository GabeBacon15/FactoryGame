using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public float chunkStandardSize = 10;

    public int subdivisions;
    public float scale, heightScale;
    public Material sourceMat;
    private Material material;
    float[,] noiseMap;

    int xCoord, yCoord;


    void Start()
    {
        material = new Material(sourceMat);
        this.GetComponent<MeshRenderer>().material = material;
    }

    public void setPosition(int x, int y)
    {
        xCoord = x;
        yCoord = y;
        this.name = "Terrain (" + x + ", " + y + ")";
        this.transform.position = new Vector3(x, 0, y) * chunkStandardSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            noiseMap = NoiseGenerator.generatePerlinNoise(subdivisions + 1, subdivisions + 1, scale, subdivisions / scale * xCoord, -subdivisions / scale * yCoord, 6165498);
            //this.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = createTexture(noiseMap);
            this.GetComponent<MeshFilter>().mesh = makePlaneMesh(chunkStandardSize, chunkStandardSize, subdivisions);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            this.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = createTexture(noiseMap);
        }
    }

    private Texture2D createTexture(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        Texture2D texture = new Texture2D(width, height);
        Color[] colorMap = new Color[width*height];
        for(int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }
        colorMap[0] = Color.green;
        colorMap[colorMap.Length-1] = Color.red;
        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }
    private Vector3[] raisedTerrain(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        //Debug.Log("NoiseMap (" + width + ", " + height + ")");
        Vector3[] vertices = new Vector3[width*height];
        Vector3[] myVertices = this.GetComponent<MeshFilter>().mesh.vertices;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                //Debug.Log("I: " + (y * width + x) + " (" + x + ", " + y + ")");
                vertices[y * width + x] = new Vector3(myVertices[y * width + x].x, noiseMap[x, y] * heightScale, myVertices[y * width + x].z);
            }
        }

        return vertices;
    }

    private Mesh makePlaneMesh(float width, float height, int subs)
    {
        Vector3[] vertices = new Vector3[(subs + 1) * (subs + 1)];
        int[] tris = new int[subs * subs * 6];
        Vector2[] uv = new Vector2[vertices.Length];

        for (int y = 0; y < subs + 1; y++)
        {
            for (int x = 0; x < subs + 1; x++)
            {
                vertices[y * (subs + 1) + x] = new Vector3((x * (width / subs))-(width/2), noiseMap[x, y]*heightScale, ((subs - y) * (height / subs)) - (height / 2));
                uv[y * (subs + 1) + x] = new Vector2(((float)x) / (subs + 1), ((float)y) / (subs + 1));
            }
        }
        int i = 0;
        for (int y = 0; y < subs; y++)
        {
            for (int x = 0; x < subs; x++)
            {
                tris[i * 6 + 0] = y * (subs + 1) + x;
                tris[i * 6 + 1] = y * (subs + 1) + x + 1;
                tris[i * 6 + 2] = (y + 1) * (subs + 1) + x;

                tris[i * 6 + 3] = y * (subs + 1) + x + 1;
                tris[i * 6 + 4] = (y + 1) * (subs + 1) + x + 1;
                tris[i * 6 + 5] = (y + 1) * (subs + 1) + x;

                //Debug.Log("Tris: (" + tris[i * 6 + 0] + ", " + tris[i * 6 + 1] + ", " + tris[i * 6 + 2] + ") " + "(" + tris[i * 6 + 3] + ", " + tris[i * 6 + 4] + ", " + tris[i * 6 + 5] + ")");
                i++;
            }
        }
        Mesh mesh = new Mesh();
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.uv = uv;

        mesh.Optimize();
        mesh.RecalculateNormals();

        return mesh;
    }
}
