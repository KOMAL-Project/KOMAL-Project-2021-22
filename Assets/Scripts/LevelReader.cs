using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//[ExecuteInEditMode] // ONLY UNCOMMENT IF LEVEL PREFABS ARE NOT IN THE GAME. DO NOT RELOAD THE SCENE MULTIPLE TIMES WHILE THIS IS ON.
public class LevelReader : MonoBehaviour
{

    private bool run = true; // Extra switch you need to flip false for normal use. Should not be true normally, but should be true when trying to restore assets in the level.

    [SerializeField] private GameObject umbrellaPrefab, crabPrefab, megaCrabPrefab, checkpointPrefab, goalPrefab, collectPrefab;

    private enum GroundType {
        TEST,
        GRASS,
        SAND,
    }

    List<GameObject> gamePrefabs = new List<GameObject>();

    [SerializeField] private Texture2D level;
    [SerializeField] Tile[] tileList;
    [SerializeField] TileBase[] ruleTiles;
    [SerializeField] private GroundType groundType;
    [SerializeField] GameObject[] npcPrefabs;

    public Tilemap tiles;
    public List<GameObject> umbrellas = new List<GameObject>();
    public List<GameObject> crabs = new List<GameObject>();
    public List<GameObject> checkpoints = new List<GameObject>();
    public GameObject goal;
    

    public GameObject tileMapObj;

    // Level Color Definitions:
    // Terrain Colors
    Color platformColor = new Color(0,0,0);
    Color caveColor = new Color(64f / 255f, 64f / 255f, 64f / 255f);
    Color grassColor = new Color(182f / 255f, 255f / 255f, 0);
    Color sandColor = new Color(255f / 255f, 216f / 255f, 0);
    // Prefab Colors
    Color crabColor = new Color(255f / 255f, 127f / 255f, 0);
    Color megaCrabColor = new Color(222f / 255f, 107f / 255f, 0);
    Color umbrellaColor = new Color(0, 0, 1);
    Color checkpointColor = new Color(1, 0, 0);
    Color collectibleColor = new Color(178f / 255f, 0, 1);
    Color objectiveColor = new Color(1, 0 , 110f / 255f);

   

    Color[] npcColors = {
        new Color(255f / 255f, 133f / 255f, 133f / 255f), //red
        new Color(255f / 255f, 184f / 255f, 133f / 255f), //orange
        new Color(255f / 255f, 235f / 255f, 135f / 255f), //yellow
        new Color(225f / 255f, 255f / 255f, 135f / 255f), //highlighter
        new Color(175f / 255f, 255f / 255f, 133f / 255f), //lime
        new Color(135f / 255f, 255f / 255f, 195f / 255f), //aqua
        new Color(135f / 255f, 255f / 255f, 245f / 255f), //light bluei
    };

    

   
    void Awake()
    {
        tiles = tileMapObj.GetComponent<Tilemap>();
        if(run) GenerateLevel();
    }
    
    public void DestroyGamePrefabs()
    {
        foreach(GameObject g in gamePrefabs)
        {
            DestroyImmediate(g);
        }
    }

    public void GenerateLevel()
    {
        for(int i = 0; i < level.width; i++)
        {
            for (int j = 0; j < level.height; j++)
            {
                Color tempColor = level.GetPixel(i,j);


                if (tempColor == platformColor)
                { //TEST
                    tiles.SetTile(new Vector3Int(i, j, 0), ruleTiles[4]);
                }
                else if (tempColor == grassColor)
                { //GRASS
                    tiles.SetTile(new Vector3Int(i, j, 0), ruleTiles[1]);
                }
                else if(tempColor == sandColor)
                { //SAND
                    tiles.SetTile(new Vector3Int(i, j, 0), ruleTiles[2]);
                }
                else if(tempColor == caveColor)
                { //CAVE
                    tiles.SetTile(new Vector3Int(i, j, 0), ruleTiles[3]);
                }
                // PREFAB CHECKS
                else if (tempColor == umbrellaColor && umbrellaPrefab) 
                {
                    //tiles.SetTile(new Vector3Int(i, j, 0), tileList[4]);

                    GameObject tempUmbrella = Instantiate(umbrellaPrefab, tiles.GetCellCenterWorld(new Vector3Int(i, j+1, 0)), Quaternion.identity);
                    umbrellas.Add(tempUmbrella);
                    gamePrefabs.Add(tempUmbrella);
                }
                else if(tempColor == checkpointColor)
                {
                    GameObject tempCheckpoint = Instantiate(checkpointPrefab, tiles.GetCellCenterWorld(new Vector3Int(i, j, 0)) + new Vector3(.5f, .5f, 0), Quaternion.identity);
                    checkpoints.Add(tempCheckpoint);
                    gamePrefabs.Add(tempCheckpoint);
                }
                else if (tempColor == crabColor)
                {
                    GameObject tempCrab;
                    tempCrab = Instantiate(crabPrefab, tiles.GetCellCenterWorld(new Vector3Int(i, j, 0)), Quaternion.identity);
                    crabs.Add(tempCrab);
                    gamePrefabs.Add(tempCrab);
                }
                else if(tempColor == megaCrabColor)
                {
                    GameObject tempCrab = null;
                    if (level.GetPixel(i + 1, j + 1) == megaCrabColor && level.GetPixel(i, j + 1) == megaCrabColor && level.GetPixel(i + 1, j) == megaCrabColor) tempCrab = Instantiate(megaCrabPrefab, tiles.GetCellCenterWorld(new Vector3Int(i, j, 0)) + new Vector3(0, 1, 0), Quaternion.identity);
                    if(tempCrab != null) gamePrefabs.Add(tempCrab);
                
                }
                else if (tempColor == collectibleColor)
                {
                    GameObject tempCollect = Instantiate(collectPrefab, tiles.GetCellCenterWorld(new Vector3Int(i, j, 0)) + new Vector3(0, .5f, 0), Quaternion.identity);
                    gamePrefabs.Add(tempCollect);
                    
                }
                else if (tempColor == objectiveColor)
                {
                    GameObject tempGoal = Instantiate(goalPrefab, tiles.GetCellCenterWorld(new Vector3Int(i, j, 0)) + new Vector3(.5f, .5f, 0), Quaternion.identity);
                    goal = tempGoal;
                    gamePrefabs.Add(tempGoal);
                }
                else
                {
                    for(int k = 0; k < npcPrefabs.Length; k++) if(npcColors[k] == tempColor && npcPrefabs[k]) gamePrefabs.Add(Instantiate(npcPrefabs[k], tiles.GetCellCenterWorld(new Vector3Int(i, j, 0)), Quaternion.identity));
                }


                
            }
        }
    }
    
}
