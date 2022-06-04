using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelReader : MonoBehaviour
{

    [SerializeField] private GameObject umbrellaPrefab, crabPrefab, megaCrabPrefab, checkpointPrefab, goalPrefab, collectPrefab;

    private enum GroundType {
        TEST,
        GRASS,
        SAND,
    }

    

    [SerializeField] private Texture2D level;
    [SerializeField] Tile[] tileList;
    [SerializeField] TileBase[] ruleTiles;
    [SerializeField] private GroundType groundType;

    public Tilemap tiles;
    public List<GameObject> umbrellas = new List<GameObject>();
    public List<GameObject> crabs = new List<GameObject>();
    public List<GameObject> checkpoints = new List<GameObject>();
    public GameObject goal;

    public GameObject tileMapObj;

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

                if (tempColor == Color.black)
                {
                    /*if (i % 2 == 0 && j % 2 == 0) tiles.SetTile(new Vector3Int(i, j, 0), tileList[0]);
                    else if (i % 2 == 1 && j % 2 == 0) tiles.SetTile(new Vector3Int(i, j, 0), tileList[1]);
                    else if (i % 2 == 0 && j % 2 == 1) tiles.SetTile(new Vector3Int(i, j, 0), tileList[2]);
                    else tiles.SetTile(new Vector3Int(i, j, 0), tileList[3]);*/

                    if (groundType == GroundType.TEST)
                    { //TEST
                        tiles.SetTile(new Vector3Int(i, j, 0), ruleTiles[0]);
                    }
                    else if (groundType == GroundType.GRASS)
                    { //GRASS
                        tiles.SetTile(new Vector3Int(i, j, 0), ruleTiles[1]);
                    }
                    else 
                    { //SAND
                        tiles.SetTile(new Vector3Int(i, j, 0), ruleTiles[2]);
                    }
                }
                else if (tempColor == Color.cyan && umbrellaPrefab) 
                {
                    //tiles.SetTile(new Vector3Int(i, j, 0), tileList[4]);

                    GameObject tempUmbrella = Instantiate(umbrellaPrefab, tiles.GetCellCenterWorld(new Vector3Int(i, j+1, 0)), Quaternion.identity);
                    umbrellas.Add(tempUmbrella);
                }
                else if(tempColor == Color.red)
                {
                    GameObject tempCheckpoint = Instantiate(checkpointPrefab, tiles.GetCellCenterWorld(new Vector3Int(i, j, 0)) + new Vector3(.5f, .5f, 0), Quaternion.identity);
                    checkpoints.Add(tempCheckpoint);
                }
                else if (tempColor == new Color(1, 127f/255f, 0))
                {
                    GameObject tempCrab;
                    tempCrab = Instantiate(crabPrefab, tiles.GetCellCenterWorld(new Vector3Int(i, j, 0)), Quaternion.identity);
                    crabs.Add(tempCrab);
                }
                else if(tempColor == new Color(222f / 255f, 107f / 255f, 0))
                {
                    Color megaCrabbish = new Color(222f / 255f, 107f / 255f, 0);
                    GameObject tempCrab;
                    if (level.GetPixel(i + 1, j + 1) == megaCrabbish && level.GetPixel(i, j + 1) == megaCrabbish && level.GetPixel(i + 1, j) == megaCrabbish) tempCrab = Instantiate(megaCrabPrefab, tiles.GetCellCenterWorld(new Vector3Int(i, j, 0)), Quaternion.identity);
                
                }
                else if (tempColor == new Color(1,1,0))
                {
                    GameObject tempCollect = Instantiate(collectPrefab, tiles.GetCellCenterWorld(new Vector3Int(i, j, 0)) + new Vector3(0, .5f, 0), Quaternion.identity);
                    
                }
                else if (tempColor == Color.green)
                {
                    GameObject tempGoal = Instantiate(goalPrefab, tiles.GetCellCenterWorld(new Vector3Int(i, j, 0)) + new Vector3(.5f, .5f, 0), Quaternion.identity);
                    goal = tempGoal;
                }

            }
        }
    }
}
