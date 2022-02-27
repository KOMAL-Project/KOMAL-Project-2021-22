using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelReader : MonoBehaviour
{

    [SerializeField] private Texture2D level;
    public Tilemap tiles;
    [SerializeField]
    Tile[] tileList;
    public GameObject tileMapObj;

    
    // Start is called before the first frame update
    void Start()
    {
        tiles = tileMapObj.GetComponent<Tilemap>();

        GenerateLevel();
    }
    
    public void GenerateLevel()
    {
        for(int i = 0; i < level.width; i++)
        {
            for (int j = 0; j < level.height; j++)
            {
                Color tempColor = level.GetPixel(i,j);

                if(tempColor == Color.black)
                {
                    if (i % 2 == 0 && j % 2 == 0) tiles.SetTile(new Vector3Int(i,j,0), tileList[0]);
                    else if (i % 2 == 1 && j % 2 == 0) tiles.SetTile(new Vector3Int(i, j, 0), tileList[1]);
                    else if (i % 2 == 0 && j % 2 == 1) tiles.SetTile(new Vector3Int(i, j, 0), tileList[2]);
                    else tiles.SetTile(new Vector3Int(i, j, 0), tileList[3]);
                }
            }
        }
    }
}
