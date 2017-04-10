using UnityEngine;
using System.Collections.Generic;
using System;
using System.Diagnostics;

public class MapGenerator : MonoBehaviour {

    [HideInInspector]
    public int width = 128;
    [HideInInspector]
    public int height = 128;
    [HideInInspector]
    public int borderSize = 0;

    public string seed = "null";
    public bool useRandomSeed = true;

    [Range(0, 100)]
    public int randomFillPercent = 54;
    [Header("Smoothing")]
    [Range(0, 10)]
    public int smoothPasses = 5;
    [HideInInspector]
    public int smoothTo1 = 4;
    [HideInInspector]
    public int smoothTo0 = 4;
    [HideInInspector]
    public int smoothSampleRadius = 1; //causes an error?
    [Header("Controlls the removing of floating islands")]
    public bool enableTearingDown = true;
    public int pillarRemoveSize = 50;
    [Header("Controlls the filling in of small pockets")]
    public bool enableFilling = true;
    public int roomFillSize = 50;
    [Header("The joining of rooms")]
    public bool connectRooms = true;
    public int hallwayRadius = 1;

    public int[][] map;

    public void Start() {
        this.generateMap(false);
    }

    public void Update() {
        if (Input.GetMouseButtonDown(0)) {
            this.generateMap(true);
        }
    }

    void OnValidate() {
        this.generateMap(false);
    }

    private void generateMap(bool newSeed) {
        Stopwatch s = new Stopwatch();
        s.Start();

        this.map = new int[this.width][];
        for (int i = 0; i < this.width; i++) {
            this.map[i] = new int[this.height];
        }

        this.randomlyFillMap(newSeed);

        for (int i = 0; i < this.smoothPasses; i++) {
            this.smoothMap();
        }

        List<Room> survivingRooms = this.fillAndDefineRegions();
        if(this.connectRooms) {
            this.connectClosestRooms(survivingRooms);
        }

        //Make a boarder, we probably wont need this
        //int[,] borderedMap = new int[width + this.borderSize * 2, height + this.borderSize * 2];

        //for (int x = 0; x < borderedMap.GetLength(0); x++) {
        //    for (int y = 0; y < borderedMap.GetLength(1); y++) {
        //        if (x >= this.borderSize && x < this.width + this.borderSize && y >= this.borderSize && y < this.height + this.borderSize) {
        //            borderedMap[x, y] = this.map[x - this.borderSize, y - this.borderSize];
        //        }
        ///        else {
        //            borderedMap[x, y] = 1;
        //        }
        //    }
        //}

        s.Stop();
        print(s.Elapsed);

        //Generate a mesh for this
        //MeshGenerator meshGen = GetComponent<MeshGenerator>();
        //meshGen.GenerateMesh(this.map, 1); //borderedMap, 1);

        //Draw the map to a texture
        Texture2D t = new Texture2D(this.width, this.height);
        Color[] colors = new Color[this.width * this.height];
        for(int i = 0; i < this.width; i++) {
            for(int j = 0; j < this.height; j++) {
                int k = this.map[i][j];
                colors[i * this.width + j] = k == 0 ? Color.black : k == 1 ? Color.white : Color.red;
            }
        }
        t.filterMode = FilterMode.Point;
        t.SetPixels(colors);
        t.Apply();
        this.GetComponent<MeshRenderer>().material.mainTexture = t;
    }

    private List<Room> fillAndDefineRegions() {
        List<List<Coord>> wallRegions = this.getRegions(1);

        if(this.enableTearingDown) {
            foreach (List<Coord> wallRegion in wallRegions) {
                if (wallRegion.Count < this.pillarRemoveSize) {
                    foreach (Coord tile in wallRegion) {
                        this.map[tile.tileX][tile.tileY] = 0;
                    }
                }
            }
        }

        List<List<Coord>> roomRegions = this.getRegions(0);
        List<Room> survivingRooms = new List<Room>();

        foreach (List<Coord> roomRegion in roomRegions) {
            if (this.enableFilling && roomRegion.Count < this.roomFillSize) {
                foreach (Coord tile in roomRegion) {
                    map[tile.tileX][tile.tileY] = 1;
                }
            }
            else {
                survivingRooms.Add(new Room(roomRegion, map));
            }
        }
        survivingRooms.Sort();
        survivingRooms[0].isMainRoom = true;
        survivingRooms[0].isAccessibleFromMainRoom = true;

        return survivingRooms;
    }

    private void connectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false) {
        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceAccessibilityFromMainRoom) {
            foreach (Room room in allRooms) {
                if (room.isAccessibleFromMainRoom) {
                    roomListB.Add(room);
                }
                else {
                    roomListA.Add(room);
                }
            }
        } else {
            roomListA = allRooms;
            roomListB = allRooms;
        }

        int bestDistance = 0;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnectionFound = false;

        foreach (Room roomA in roomListA) {
            if (!forceAccessibilityFromMainRoom) {
                possibleConnectionFound = false;
                if (roomA.connectedRooms.Count > 0) {
                    continue;
                }
            }

            foreach (Room roomB in roomListB) {
                if (roomA == roomB || roomA.IsConnected(roomB)) {
                    continue;
                }

                for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++) {
                    for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++) {
                        Coord tileA = roomA.edgeTiles[tileIndexA];
                        Coord tileB = roomB.edgeTiles[tileIndexB];
                        int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));

                        if (distanceBetweenRooms < bestDistance || !possibleConnectionFound) {
                            bestDistance = distanceBetweenRooms;
                            possibleConnectionFound = true;
                            bestTileA = tileA;
                            bestTileB = tileB;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                        }
                    }
                }
            }
            if (possibleConnectionFound && !forceAccessibilityFromMainRoom) {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }

        if (possibleConnectionFound && forceAccessibilityFromMainRoom) {
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            this.connectClosestRooms(allRooms, true);
        }

        if (!forceAccessibilityFromMainRoom) {
            this.connectClosestRooms(allRooms, true);
        }
    }

    void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB) {
        Room.ConnectRooms(roomA, roomB);
        //Debug.DrawLine (CoordToWorldPoint (tileA), CoordToWorldPoint (tileB), Color.red, 100);
        //List<Coord> line = GetLine(tileA, tileB);
        Coord[] line = this.GetLine(tileA, tileB);
        foreach (Coord c in line) {
            DrawCircle(c, this.hallwayRadius);
        }
    }

    void DrawCircle(Coord c, int r) {
        this.map[c.tileX][c.tileY] = 2;
        //return;
        for (int x = -r; x <= r; x++) {
            for (int y = -r; y <= r; y++) {
                if (x * x + y * y <= r * r) {
                    int drawX = c.tileX + x;
                    int drawY = c.tileY + y;
                    if (isInMapRange(drawX, drawY)) {
                        map[drawX][drawY] = 0;
                    }
                }
            }
        }
    }

    private Coord[] GetLine(Coord from, Coord to) {
        int x = from.tileX;
        int y = from.tileY;

        int dx = to.tileX - from.tileX;
        int dy = to.tileY - from.tileY;

        bool inverted = false;
        int step = Math.Sign(dx);
        int gradientStep = Math.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        int arrayIndex = 0;
        Coord[] line = new Coord[longest > shortest ? longest : shortest];

        if (longest < shortest) {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }

        int gradientAccumulation = longest / 2;
        for (int i = 0; i < longest; i++) {
            line[arrayIndex] = new Coord(x, y); //line.Add(new Coord(x, y));
            arrayIndex++;

            if (inverted) {
                y += step;
            }
            else {
                x += step;
            }

            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest) {
                if (inverted) {
                    x += gradientStep;
                }
                else {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }

        return line;
    }

    //Vector3 CoordToWorldPoint(Coord tile) {
    //    return new Vector3(-width / 2 + .5f + tile.tileX, 2, -height / 2 + .5f + tile.tileY);
    //}

    //Returns a flood fill list of every region, a region being a list of coords
    private List<List<Coord>> getRegions(int tileType) {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (mapFlags[x, y] == 0 && map[x][y] == tileType) {
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach (Coord tile in newRegion) {
                        mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }

        return regions;
    }

    List<Coord> GetRegionTiles(int startX, int startY) {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[width, height];
        int tileType = map[startX][startY];

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while (queue.Count > 0) {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
                    if (isInMapRange(x, y) && (y == tile.tileY || x == tile.tileX)) {
                        if (mapFlags[x, y] == 0 && map[x][y] == tileType) {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }
        return tiles;
    }

    //Checks if the passed position in is the map
    private bool isInMapRange(int x, int y) {
        return x >= 0 && x < this.width && y >= 0 && y < this.height;
    }

    //Fills the map with rnadom 1s and 0s
    void randomlyFillMap(bool flag) {
        if (this.useRandomSeed && flag) {
            this.seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < this.width; x++) {
            for (int y = 0; y < this.height; y++) {
                if (x == 0 || x == this.width - 1 || y == 0 || y == this.height - 1) {
                    this.map[x][y] = 1;
                }
                else {
                    this.map[x][y] = (pseudoRandom.Next(0, 100) < this.randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    //Smooths out the map
    private void smoothMap() {
        for (int x = 0; x < this.width; x++) {
            for (int y = 0; y < this.height; y++) {
                int neighbourWallTiles = this.getSurroundingWallCount(x, y);

                if (neighbourWallTiles > this.smoothTo1) {
                    this.map[x][y] = 1;
                } else if (neighbourWallTiles < this.smoothTo0) {
                    this.map[x][y] = 0;
                }
            }
        }
    }

    //Returns the number of surrounding walls
    private int getSurroundingWallCount(int gridX, int gridY) {
        int wallCount = 0;
        for (int neighborX = gridX - this.smoothSampleRadius; neighborX <= gridX + this.smoothSampleRadius; neighborX++) {
            for (int neighborY = gridY - this.smoothSampleRadius; neighborY <= gridY + this.smoothSampleRadius; neighborY++) {
                if (this.isInMapRange(neighborX, neighborY)) {
                    if (neighborX != gridX || neighborY != gridY) {
                        wallCount += this.map[neighborX][neighborY];
                    }
                }
                else {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    struct Coord {
        public int tileX;
        public int tileY;

        public Coord(int x, int y) {
            tileX = x;
            tileY = y;
        }
    }
    
    class Room : IComparable<Room> {
        public List<Coord> tiles;
        public List<Coord> edgeTiles;
        public List<Room> connectedRooms;
        public int roomSize;
        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;

        public Room() {
        }

        public Room(List<Coord> roomTiles, int[][] map) {
            tiles = roomTiles;
            roomSize = tiles.Count;
            connectedRooms = new List<Room>();

            edgeTiles = new List<Coord>();
            foreach (Coord tile in tiles) {
                for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
                    for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
                        if (x == tile.tileX || y == tile.tileY) {
                            if (map[x][y] == 1) {
                                edgeTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }

        public void SetAccessibleFromMainRoom() {
            if (!isAccessibleFromMainRoom) {
                isAccessibleFromMainRoom = true;
                foreach (Room connectedRoom in connectedRooms) {
                    connectedRoom.SetAccessibleFromMainRoom();
                }
            }
        }

        public static void ConnectRooms(Room roomA, Room roomB) {
            if (roomA.isAccessibleFromMainRoom) {
                roomB.SetAccessibleFromMainRoom();
            }
            else if (roomB.isAccessibleFromMainRoom) {
                roomA.SetAccessibleFromMainRoom();
            }
            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }

        public bool IsConnected(Room otherRoom) {
            return connectedRooms.Contains(otherRoom);
        }

        public int CompareTo(Room otherRoom) {
            return otherRoom.roomSize.CompareTo(roomSize);
        }
    }
}