using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TileDemoManager : MonoBehaviour
{
    public Authors Author;
    public int Level = 5;
    public GeoDemoScript GeoPrefab;
    public Camera Cam;
    public List<GeoDemoScript> Tiles = new List<GeoDemoScript>();
    
    void Start()
    {
        if(God.Session == null) God.Session = new GameSession(Author);
        God.Session.Level = Level;
        God.LB = Parser.GetLB(God.Session.Author);
        StartCoroutine(Build());
    }

    public IEnumerator Pause(float time=0)
    {
        if (Input.GetKey(KeyCode.Z)) time = 0.2f;
        foreach(GeoDemoScript g in Tiles) g.Setup();
        if (time == 0)
        {
            while (!Input.GetKeyDown(KeyCode.Space))
                yield return null;
        }
        else
            yield return new WaitForSeconds(Input.GetKey(KeyCode.LeftShift) ? 0.01f : time);
        yield return null;
    }
    
    public IEnumerator Build()
    {
        God.LB.Customize();
        Cam.transform.position = new Vector3(God.LB.Size.x / 2, God.LB.Size.y / 2, -10);
        if (Author == Authors.Demo)
        {
            //Two nested for loops lets us build a grid of room slots at our desired size
            for (int x = 0; x < God.LB.Size.x; x++)
            for (int y = 0; y < God.LB.Size.y; y++)
            {
                //Spawn a blank room slot into the position 
                GeoTile g = new GeoTile(x, y, God.LB);
                //If our dictionary doesn't have this X-row added, add it
                if (!God.LB.GeoMap.ContainsKey(x)) God.LB.GeoMap.Add(x, new Dictionary<int, GeoTile>());
                //Add it to the dictionary and list of slots
                God.LB.GeoMap[x].Add(y, g);
                God.LB.AllGeo.Add(g);
                GeoDemoScript t = Instantiate(GeoPrefab, new Vector3(g.X, g.Y), Quaternion.identity);
                t.Tile = g;
                t.Setup();
                Tiles.Add(t);
                yield return StartCoroutine(Pause(0.2f));
            }
        }
        else
        {
            God.LB.BuildGeoMap();
            foreach (GeoTile g in God.LB.AllGeo)
            {
                GeoDemoScript t = Instantiate(GeoPrefab, new Vector3(g.X, g.Y), Quaternion.identity);
                t.Tile = g;
                t.Setup();
                Tiles.Add(t);
            }
        }

        yield return StartCoroutine(Pause());
        if (Author == Authors.Demo)
        {
            //Make a safe path leading to the exit
            //Pick the column the player spawns in (at the bottom of the level)
            int start = Random.Range(0, God.LB.Size.x);
            //For each row. . .
            for (int y = 0; y < God.LB.Size.y; y++)
            {
                //Pick a slot to move the path towards before moving up a row
                int end = Random.Range(0, God.LB.Size.x);
                if (end == start) end = Random.Range(0, God.LB.Size.x);
                God.LB.GetGeo(end, y).DebugColor = "X";
                yield return StartCoroutine(Pause(0.5f));
                int x = start;
                //If this is the bottom row. . .
                if (y == 0)
                {
                    //Then our start column is the player start point
                    God.LB.PlayerSpawn = God.LB.GetGeo(x, 0);
                    God.LB.PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);
                }

                //While we haven't reached out destination. . .
                while (x != end)
                {
                    int old = x;
                    //Take a step towards the destination
                    x = (int)Mathf.MoveTowards(x, end, 1);
                    //Open up a connection between the tile we came from and the one we're at now
                    GeoTile a = God.LB.GetGeo(old, y);
                    GeoTile b = God.LB.GetGeo(x, y);
                    //And make sure it's marked as being on the main path (does nothing for now)
                    b.SetPath(GeoTile.GeoTileTypes.MainPath);
                    
                    yield return StartCoroutine(Pause(0.2f));
                    if (start > end)
                    {
                        a.Links.Add(Directions.Left);
                        yield return StartCoroutine(Pause(0.2f));
                        b.Links.Add(Directions.Right);
                        
                        yield return StartCoroutine(Pause(0.2f));
                    }
                    else
                    {
                        a.Links.Add(Directions.Right);
                        yield return StartCoroutine(Pause(0.2f));
                        b.Links.Add(Directions.Left);
                        yield return StartCoroutine(Pause(0.2f));
                    }
                    
                }

                //Mark our end point as the start of the path on the next row
                start = end;
                //Move up row up and repeat, linking to the slot below
                if (y < God.LB.Size.y - 1)
                {
                    GeoTile a = God.LB.GetGeo(start, y);
                    GeoTile b = God.LB.GetGeo(start, y + 1);
                    b.SetPath(GeoTile.GeoTileTypes.MainPath);
                    yield return StartCoroutine(Pause(0.2f));
                    a.Links.Add(Directions.Up);
                    yield return StartCoroutine(Pause(0.2f));
                    b.Links.Add(Directions.Down);
                    yield return StartCoroutine(Pause(0.2f));
                }
                else
                {
                    if (God.LB.Boss != null)
                    {
                        GeoTile g = new GeoTile(start, y + 1, God.LB);
                        if (!God.LB.GeoMap.ContainsKey(start)) God.LB.GeoMap.Add(start, new Dictionary<int, GeoTile>());
                        God.LB.GeoMap[start].Add(y + 1, g);
                        God.LB.AllGeo.Add(g);
                        God.LB.Exit = God.LB.GetGeo(start, y + 1);
                        God.LB.Exit.SetPath(GeoTile.GeoTileTypes.Exit);
                        GeoDemoScript t = Instantiate(GeoPrefab, new Vector3(g.X, g.Y), Quaternion.identity);
                        t.Tile = g;
                        t.Setup();
                        Tiles.Add(t);
                        yield return StartCoroutine(Pause(0.2f));

                        GeoTile boss = God.LB.GetGeo(start, y);
                        boss.SetPath(GeoTile.GeoTileTypes.Boss);
                        yield return StartCoroutine(Pause(0.2f));
                        boss.Links.Add(Directions.Up);
                        yield return StartCoroutine(Pause(0.2f));
                        God.LB.Exit.Links.Add(Directions.Down);
                        yield return StartCoroutine(Pause(0.2f));
                    }
                    else
                    {
                        //But if we're at the top of the map, mark our ultimate position as the exit spawn location
                        God.LB.Exit = God.LB.GetGeo(start, y);
                        God.LB.Exit.SetPath(GeoTile.GeoTileTypes.Exit);
                    }


                }
            }
        }
        else
            God.LB.BuildMainPath();
        yield return StartCoroutine(Pause());
        if (Author == Authors.Demo)
        {
            //Open up a bunch of random links between rooms
        //For each room slot that exists. . .
        foreach (GeoTile g in God.LB.AllGeo)
        {
            //Get a list of the slots above it and to its right
            List<Directions> maybe = g.PotentialLinks();
            //For each possible link. . .
            foreach (Directions d in maybe)
            {
                //Flip a (weighted) coin. If it comes up false, don't link the room slots
                if (!God.CoinFlip(God.LB.LinkOdds)) continue;
                //But if it came up true, and its neighbor actually exists, connect them!
                GeoTile other = g.Neighbor(d);
                if (other == null) continue;
                g.DebugColor = "X";
                other.DebugColor = "X";
                yield return StartCoroutine(Pause(0.2f));
                g.Links.Add(d);
                yield return StartCoroutine(Pause(0.2f));
                other.Links.Add(God.OppositeDir(d));
                yield return StartCoroutine(Pause(0.2f));
                g.DebugColor = "";
                other.DebugColor = "";
            }
        }

        //Do we have any isolated rooms you can't get to?
        //If so, open some more links
        yield return StartCoroutine(FakeUnconnectedTest());
        List<GeoTile> uncon = God.LB.UnconnectedTest();
        foreach (GeoTile u in uncon) u.DebugColor = "X";
        yield return StartCoroutine(Pause());
        foreach (GeoTile u in uncon) u.DebugColor = "";
        int safety = 99;
        //For as long as we have unconnected tiles, keep connecting them
        //But only do this while loop 99 times (more than we need but not infinite), in case we cause an infinite loop
        while (uncon.Count > 0 && safety > 0)
        {
            List<GeoTile> unconTemp = new List<GeoTile>();
            unconTemp.AddRange(uncon);
            safety--;
            //For each tile that's not connected to anyone. . .
            while(unconTemp.Count > 0)
            {
                GeoTile g = unconTemp[Random.Range(0, unconTemp.Count)];
                unconTemp.Remove(g);
                g.DebugColor = "X";
                yield return StartCoroutine(Pause(0.2f));
                g.DebugColor = "";
                //Get a list of all four of its neighbors
                List<Directions> maybe = g.PotentialLinks(false);
                //If it has no neighbors, something's wrong and we should throw an error
                if (maybe.Count == 0)
                {
                    God.LogError("TILE BOTH LINKLESS AND UNCONNECTABLE: " + g);
                    continue;
                }
                //Up/down links are more level-design-fun than left/right ones, and up/down are returned first in the list
                //So roll twice and take the smaller to bias towards up/down links
                int roll = Mathf.Min(Random.Range(0, maybe.Count), Random.Range(0, maybe.Count));
                Directions d = maybe[roll];
                //Find the neighbor in that direction and make sure it exists
                GeoTile other = g.Neighbor(d);
                if (other == null)
                {
                    God.LogWarning("POTENTIAL LINK NULL: " + g + " / " + d + " / " + other);
                    continue;
                }
                //If the tile we're connecting to is reachable, connect to it and mark yourself as reachable
                if (other.Path != GeoTile.GeoTileTypes.Unreachable)
                {
                    g.DebugColor = "X";
                    other.DebugColor = "X";
                    yield return StartCoroutine(Pause());
                    g.Links.Add(d);
                    other.Links.Add(God.OppositeDir(d));
                    g.SetPath(GeoTile.GeoTileTypes.Connected);
                    //Mark how far the slot is from the main path (one further than the one it connected to)
                    g.Depth = other.Depth + 1;
                    g.DebugColor = "";
                    other.DebugColor = "";
                }
            }

            yield return StartCoroutine(FakeUnconnectedTest());
            //Alright, maybe we got them all! Do another flood to see if we still have unconnected tiles
            uncon = God.LB.UnconnectedTest();
        }
        }
        else 
            God.LB.ConnectAllGeos();
        foreach(GeoDemoScript g in Tiles) g.Setup();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("TileDemo");
    }
    
    public IEnumerator FakeUnconnectedTest()
    {
        //Make a flood to see if all the rooms are connected
        //Start with a list of all the tiles
        List<GeoTile> unc = new List<GeoTile>();
        unc.AddRange(God.LB.AllGeo);
        List<GeoTile> done = new List<GeoTile>();
        List<GeoTile> pend = new List<GeoTile>(){God.LB.PlayerSpawn};
        while (pend.Count > 0)
        {
            GeoTile chosen = God.Random(pend);
            chosen.DebugColor = "A";
            yield return StartCoroutine(Pause(0.1f));
            pend.Remove(chosen);
            done.Add(chosen);
            unc.Remove(chosen);
            // chosen.SetPath(GeoTile.GeoTileTypes.Connected);
            List<GeoTile> neigh = chosen.Neighbors(true);
            foreach (GeoTile n in neigh)
            {
                if (done.Contains(n)) continue;
                if (pend.Contains(n)) continue;
                pend.Add(n);
                n.DebugColor = "B";
                yield return StartCoroutine(Pause(0.1f));
            }
            yield return StartCoroutine(Pause(0.2f));
        }
        yield return StartCoroutine(Pause());
        foreach (GeoTile g in God.LB.AllGeo) g.DebugColor = "";
    }
}
