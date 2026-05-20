using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_SarahS : LevelBuilder
{
    private GameObject levelExit;
    private bool isCleared = false;
    public Level_SarahS()
    {
        Author = Authors.SarahS;
    }

    public override void Customize()
    {
        if (God.Session.Level >= 3)
        {
            SceneManager.LoadScene("YouWin");
            return;
        }
        //God.Session.Level = 1;
        SpawnPlayer();
        if (God.Session.Level > 2)
        {
            God.Session.Level = 1;
           
        }
        
        int currentLevel = Mathf.Max(1, God.Session.Level);
        if (currentLevel == 1)
        {
            Size = new Vector2Int(2, 2);
            
        }
        else if (currentLevel == 2)
        {
            Size = new Vector2Int(3, 3);
            
        }
        else
        {
            Size = new Vector2Int(1, 1);
            
        }
        
        LinkOdds = 1.5f;
        RoomSize = new Vector2Int(11, 11);
    }

    public override void BuildGeoMap()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                AddGeo(new GeoTile(x, y, this));
            }
        }
    }
    
    public override void BuildMainPath()
    {
       
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                GeoTile current = GetGeo(x, y);
                if (current == null) continue;
                
                GeoTile right = GetGeo(x + 1, y);
                if (right != null)
                {
                    if (!current.Links.Contains(Directions.Right))
                        current.Links.Add(Directions.Right);
                    if (!right.Links.Contains(Directions.Left))
                        right.Links.Add(Directions.Left);
                    right.SetPath(GeoTile.GeoTileTypes.MainPath);
                }
                
                GeoTile up = GetGeo(x, y + 1);
                if (up != null)
                {
                    if (!current.Links.Contains(Directions.Up))
                        current.Links.Add(Directions.Up);
                    if (!up.Links.Contains(Directions.Down))
                        up.Links.Add(Directions.Down);
                    up.SetPath(GeoTile.GeoTileTypes.MainPath);
                }
            }
        }

        PlayerSpawn = GetGeo(0, 0);
        PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);
        
        Exit = GetGeo(Size.x - 1, Size.y - 1);
        Exit.SetPath(GeoTile.GeoTileTypes.Exit);
        
    }

    public override void SpawnThings()
    {
        GameObject existingMusicPlayer = GameObject.Find("MusicPlayer");
        if (existingMusicPlayer == null)
        {
            GameObject musicPlayer = new GameObject("MusicPlayer");
            AudioSource audioSource = musicPlayer.AddComponent<AudioSource>();
            audioSource.clip = Resources.Load<AudioClip>("SarahS/02 - Heads Will Roll");
            audioSource.loop = true;
            audioSource.Play();
            
            Object.DontDestroyOnLoad(musicPlayer);
        }
        
        
        if (SpawnPointsPlayer.Count == 0 && PlayerSpawn?.Room != null)
            SpawnPointsPlayer.Add(new SpawnRequest(GameTags.Player).SetPos(PlayerSpawn.Room.transform.position));

        
        int ghostCount = Mathf.CeilToInt((Size.x * Size.y) * 1f);
            List<GeoTile> availableRooms = new List<GeoTile>(AllGeo);
            availableRooms.Remove(PlayerSpawn);
            
            for (int i = 0; i < ghostCount; i++)
            {
                if (availableRooms.Count == 0) break;
                GeoTile randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                availableRooms.Remove(randomRoom);
                
                TextRoomOption textRoom = randomRoom.RoomType as TextRoomOption;
                if (textRoom != null && textRoom.MapText != null)
                {
                    List<Vector2Int> safeSpots = new List<Vector2Int>();
                    
                    for (int y = 1; y < textRoom.MapText.Length - 1; y++)
                    {
                        for (int x = 1; x < textRoom.MapText[y].Length - 1; x++)
                        {
                            if (textRoom.MapText[y][x] == ' ')
                            {
                                if (textRoom.MapText[y - 1][x] == ' ' &&
                                    textRoom.MapText[y + 1][x] == ' ' &&
                                    textRoom.MapText[y][x - 1] == ' ' &&
                                    textRoom.MapText[y][x + 1] == ' ')
                                {
                                    safeSpots.Add(new Vector2Int(x, y));
                                }
                            }
                        }
                    }
                    
                    if (safeSpots.Count == 0)
                    {
                        for (int y = 1; y < textRoom.MapText.Length - 1; y++)
                            for (int x = 1; x < textRoom.MapText[y].Length - 1; x++)
                                if (textRoom.MapText[y][x] == ' ') safeSpots.Add(new Vector2Int(x, y));
                    }
                    
                    if (safeSpots.Count > 0)
                    {
                        Vector2Int safeTile = safeSpots[Random.Range(0, safeSpots.Count)];
                        Vector3 safePos = randomRoom.Room.GetPos(safeTile.x, safeTile.y, textRoom.MapSize);
                        string[] possibleMonsters = new string[] { "Ghost", "Monster" };
                        string selectedMonster = possibleMonsters[Random.Range(0, possibleMonsters.Length)];
                        ThingOption monster = God.Library.GetThing(new SpawnRequest(selectedMonster));
                        if (monster != null)
                        {
                            SpawnPointsFixed.Add(new SpawnRequest(monster).SetPos(safePos));
                        }
                    }
                }
            }

            int currentLevel = Mathf.Max(1, God.Session.Level);
            if (currentLevel == 2)
            {
                ThingOption myWeapon = God.Library.GetThing(new SpawnRequest("Weapon"));
                ThingOption myKey = God.Library.GetThing(new SpawnRequest("Key"));
                if (myWeapon != null && SpawnPoints.Count > 0)
                {
                    SpawnRequest oneSpot = SpawnPoints[Random.Range(0, SpawnPoints.Count)];
                    SpawnPoints.Remove(oneSpot);
                    myWeapon.Create().Spawn(oneSpot);
                }
                else
                {
                    Debug.LogWarning("can't find weapon");
                }

                if (myKey != null && SpawnPoints.Count > 0)
                {
                    SpawnRequest twoSpot = SpawnPoints[Random.Range(0, SpawnPoints.Count)];
                    SpawnPoints.Remove(twoSpot);
                    myKey.Create().Spawn(twoSpot);
                }
                else
                {
                    Debug.LogWarning("can't find key");
                }
            }
        
        base.SpawnThings();
        GameObject spawnedExit = GameObject.Find("Exit");
        if (spawnedExit == null) spawnedExit = GameObject.Find("Exit(Clone)");
        GameObject monitor = new GameObject("ExitMonitor");
        ExitMonitor_SarahS manager = monitor.AddComponent<ExitMonitor_SarahS>();
        manager.levelExit = spawnedExit;
        manager.currentLevel = God.Session.Level;
    }
    
    public override void FindQuotas()
    {
        
        if (God.Session.Level == -1) return;
        int currentLevel = Mathf.Max(1, God.Session.Level);
        int targetDotCount = Mathf.RoundToInt(SpawnPoints.Count / 8f);
        while (SpawnPoints.Count > targetDotCount)
        {
            int randomIndex = Random.Range(0, SpawnPoints.Count);
            SpawnPoints.RemoveAt(randomIndex);
        }
        AddQuota(GameTags.ScoreThing, SpawnPoints.Count);
        
        FindThings();
    }
    
}
