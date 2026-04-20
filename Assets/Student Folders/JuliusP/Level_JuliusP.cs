using UnityEngine;
using System.Collections.Generic;

public class Level_JuliusP : LevelBuilder
{
    public Vector2Int OriginalLvlSize;

    public float SpecialRoomChance = 0.3f;
    public Level_JuliusP()
    {
        Author = Authors.JuliusP;
    }




    //OVVERIDE CUSTOMIZE FUNCTION FROM LEVEL BUILDER//
   public override void Customize()
   {
    
    //As you go deeper the map gets bigger
    
    //CURRENT LEVEL NUMBER//
    int l = God.Session.Level;
   
    //WIDTH AND HEIGHT OF THE GRID//
    int w;
    int h;

    //EVERY 5 LEVELS SET WIDTH TO BE 4 AND HEIGHT TO BE 5//
    if (l % 5 == 0)
    {
        w = 4;
        h = 5;
    }

   
    
    //EVERY 3 LEVELS, A SPECIAL ROOM APPEARS//
    else if(l % 3 == 0 && Random.value < SpecialRoomChance)
    {


         w = 5;
         h = 4;       



    }    

    
   
    else
    {

        //WIDTH STARTS AT 5, AND EVERY THIRD LEVEL GROWS BY 1//
        w = 6 + Mathf.FloorToInt(l / 3);
       
        //HEIGHT STARTS AT 5, AND EVERY OTHER LEVEL GROWS BY 1//
        h = 5 + Mathf.FloorToInt(l / 2);
    }

    //VECTOR2INT HOLDS AN X && Y VALUE, WIDTH AND HEIGHT//
   
    //SET'S THE LEVEL'S SIZE TO BE W WIDE AND H TALL//
    Size = new Vector2Int(w, h);

   
    //If we haven't created a player yet for this playthrough, make one.
    if (God.Session.Player == null)
        God.Session.Player = God.Library.GetThing(new SpawnRequest(GameTags.Player)).Create();
  
   Boss = God.Library.GetThing(new SpawnRequest(GameTags.Boss), this, false);
   
   }


    ///Build out a zoomed-out map of the level, without specific rooms
    /// THIS CREATES THE MAP (NOT PHYSICALLY, BUT IN MEMORY)
   
    public virtual void BuildGeoMap()
    {
        //Two nested for loops lets us build a grid of room slots at our desired size
        for (int x = 0; x < Size.x; x++)for (int y = 0; y < Size.y; y++)
        {
            //Spawn a blank room slot into the position 
            GeoTile g = new GeoTile(x, y,this);
            //If our dictionary doesn't have this X-row added, add it
            if(!GeoMap.ContainsKey(x)) GeoMap.Add(x,new Dictionary<int, GeoTile>());
            //Add it to the dictionary and list of slots
            GeoMap[x].Add(y,g);
            AllGeo.Add(g);

        }
    }


    ///Connect geomorphs to each other to make sure there's a path from player spawn to exit
    public override void BuildMainPath()
    {
        //Make a safe path leading to the exit
        //Pick the column the player spawns in (at the bottom of the level)

        //ROW -> Y (ROWS)
        //COLUMN -> X (COLUMNS)

        //WHERE THE PATH BEGINS//

        //START COULD BE 0, 1, 2, 3, 4 DEPENDING ON BIG THE COLUMNS ARE//
        int start = Random.Range(0, Size.x);

        //THIS FOR LOOP IS FOR EACH ROW//
        //For each row. . .
        for (int y = 0; y < Size.y; y++)
        {
            //Pick a slot to move the path towards before moving up a row
            //WHERE THE END SHOULD BE FOR EACH COLUMN//
            int end = Random.Range(0, Size.x);

            //MAKES SURE THE END AND START ARE NOT THE SAME, IF THEY ARE RE-RANDOMIZE//
            if(end == start) end = Random.Range(0, Size.x);
            
            //SETS X TO BE START//
            int x = start;
            
            //Y == 0, 
            
            //If this is the bottom row. . .
            if (y == 0)
            {
                //Then our start column is the player start point

                //THE COORDINATES OF THE PLAYER SPAWN//

                //THIS RETRIVES AND USES THE STARTING VALUE AND SETS IT FOR THE PLAYERSPAWN//
                PlayerSpawn = GetGeo(x, 0);

                //VISUALLY MARKS THE TILE OF WHERE THE PLAYER STARTS//
                PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);
            }

            //THIS IS FOR MOVING TOWARD THE SELECTED END OF A ROW//
            //While we haven't reached out destination. . .
            while(x != end)
            {
               
                //SAVES PREVIOUS POSITION ON TILE//
                int old = x;
                
                //Take a step towards the destination
                
                //MOVE TOWARD WHERE YOU WANT TO GO//
                x = (int)Mathf.MoveTowards(x, end, 1);
               
                //Open up a connection between the tile we came from and the one we're at now
                GeoTile a = GetGeo(old, y);
                GeoTile b = GetGeo(x, y);
                //And make sure it's marked as being on the main path (does nothing for now)
                //RESPONSIBLE FOR DRAWING THE MAINPATH//
                b.SetPath(GeoTile.GeoTileTypes.MainPath);
                if (start > end)
                {
                    a.Links.Add(Directions.Left);
                    b.Links.Add(Directions.Right);
                }
                else
                {
                    a.Links.Add(Directions.Right);
                    b.Links.Add(Directions.Left);
                }
            }
            //Mark our end point as the start of the path on the next row
            start = end;

            //FOR MOVING UP TO THE NEXT ROW//
            //Move up row up and repeat, linking to the slot below
            if (y < Size.y - 1)
            {
                GeoTile a = GetGeo(start, y);
                GeoTile b = GetGeo(start, y+1);
                a.Links.Add(Directions.Up);
                b.Links.Add(Directions.Down);
                b.SetPath(GeoTile.GeoTileTypes.MainPath);
            }
            else
            {
                if(Boss != null){
                    GeoTile g = new GeoTile(start, y+1,this);
                    if(!GeoMap.ContainsKey(start)) GeoMap.Add(start,new Dictionary<int, GeoTile>());
                    GeoMap[start].Add(y+1,g);
                    AllGeo.Add(g);
                    Exit = GetGeo(start, y+1); 
                    Exit.SetPath(GeoTile.GeoTileTypes.Exit);
                    
                    GeoTile boss = GetGeo(start, y); 
                    boss.SetPath(GeoTile.GeoTileTypes.Boss);
                    boss.Links.Add(Directions.Up);
                    Exit.Links.Add(Directions.Down);
                }
                else
                {
                    //But if we're at the top of the map, mark our ultimate position as the exit spawn location
                    Exit = GetGeo(start, y); 
                    Exit.SetPath(GeoTile.GeoTileTypes.Exit);
                }
                
                
            }
        }
    }



     ///Open more connections between geomorphs to make sure all are accessable
    public virtual void ConnectAllGeos()
    {
        //Open up a bunch of random links between rooms
        //For each room slot that exists. . .
        foreach (GeoTile g in AllGeo)
        {
            //Get a list of the slots above it and to its right
            List<Directions> maybe = g.PotentialLinks();
            //For each possible link. . .
            foreach (Directions d in maybe)
            {
                //Flip a (weighted) coin. If it comes up false, don't link the room slots
                if (!God.CoinFlip(LinkOdds)) continue;
                //But if it came up true, and its neighbor actually exists, connect them!
                GeoTile other = g.Neighbor(d);
                if (other == null) continue;
                g.Links.Add(d);
                other.Links.Add(God.OppositeDir(d));
            }
        }

        //Do we have any isolated rooms you can't get to?
        //If so, open some more links
        List<GeoTile> uncon = UnconnectedTest();
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
                    g.Links.Add(d);
                    other.Links.Add(God.OppositeDir(d));
                    g.SetPath(GeoTile.GeoTileTypes.Connected);
                    //Mark how far the slot is from the main path (one further than the one it connected to)
                    g.Depth = other.Depth + 1;
                }
            }
            //Alright, maybe we got them all! Do another flood to see if we still have unconnected tiles
            uncon = UnconnectedTest();
        }
    }

}