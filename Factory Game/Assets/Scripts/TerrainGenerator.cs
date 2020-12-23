using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    Dictionary<Vector2, GameObject> chunks;
    public GameObject chunkPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        chunks = new Dictionary<Vector2, GameObject>();
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                chunks.Add(new Vector2(i, j), Instantiate(chunkPrefab));
                chunks[new Vector2(i, j)].GetComponent<Chunk>().setPosition(i, j);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<Vector2> getInRadiusOf(int x, int y, int radius)
    {
        List<Vector2> cells = new List<Vector2>();
        int width;
        int height;
        cells.Add(new Vector2(x, y));

        for (int i = x - radius; i < x + radius; i++)
        {
            width = i - x;
            if (i < x)
            {
                width = x - i;
            }
            for (int j = y - radius; j < y + radius; j++)
            {
                if (i == x && j == y)
                    continue;
                height = j - y;
                if (j < y)
                {
                    height = y - j;
                }
                if (Mathf.Sqrt(Mathf.Pow(width, 2) + Mathf.Pow(height, 2)) < radius)
                {
                    cells.Add(new Vector2(i, j));
                }
            }
        }
        return cells;
    }

}