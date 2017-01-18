using UnityEngine;
using VoxelEngine.Level;

namespace VoxelEngine.Generation.CellularAutomaton {
    public class CaveGenerator {// : MonoBehaviour {
        public int WIDTH = 128;
        public int HEIGHT = 128;

        public string seed = "null";
        public bool useRandomSeed = true;

        [Range(0, 100)]
        public int randomFillPercent = 56;
        [Header("Smoothing")]
        [Range(0, 10)]
        public int smoothPasses = 1; //5;
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
        public int hallwayRadius = 2;

        public int[][][] map;

        public void generateMap(bool newSeed) {
            this.map = new int[this.WIDTH][][];
            for (int i = 0; i < this.WIDTH; i++) {

                this.map[i] = new int[Chunk.SIZE][];

                for (int j = 0; j < Chunk.SIZE; j++) {
                    this.map[i][j] = new int[this.HEIGHT];
                }
            }

            this.randomlyFillMap();

            //Smooth the "floor"
            for (int i = 0; i < 3; i++) {
                this.smoothMap(2);
            }

            //Smooth the rest, working from the floor up
            for(int j = 0; j < 3; j++) {
                for (int x = 0; x < this.WIDTH; x++) {
                    for (int z = 0; z < this.HEIGHT; z++) {
                        for (int y = 2; y < Chunk.SIZE; y++) {
                            int neighbourWallTiles = this.func(x, y, z);
                            if (neighbourWallTiles > 9) { //14
                                this.map[x][y][z] = 1;
                            }
                            else if (neighbourWallTiles < 9) {
                                this.map[x][y][z] = 0;
                            }
                        }
                    }
                }
            }

            //List<Room> survivingRooms = this.fillAndDefineRegions();
            //if (this.connectRooms) {
            //    this.connectClosestRooms(survivingRooms);
            //}
        }

        //Returns the number of surrounding walls
        private int func(int gridX, int gridY, int gridZ) {
            int wallCount = 0;
            for (int x = gridX - 1; x <= gridX + 1; x++) {
                for (int z = gridZ - 1; z <= gridZ + 1; z++) {
                    for (int y = gridY-1; y <= gridY; y++) {
                        if (this.isInMapRange(x, y, z)) {
                            if (x != gridX || y != gridY || z != gridZ) {
                                wallCount += this.map[x][y][z];
                            }
                        }
                         else {
                             wallCount++;
                        }
                    }
                }
            }

            return wallCount;
        }

        //Checks if the passed position in is the map
        private bool isInMapRange(int x, int y, int z) {
            return x >= 0 && x < this.WIDTH && y >= 0 && y < Chunk.SIZE && z >= 0 && z < this.HEIGHT;
        }

        //Fills the map with random 1s and 0s
        private void randomlyFillMap() {
            this.seed = Time.time.ToString();

            System.Random pseudoRandom = new System.Random(seed.GetHashCode());

            for (int x = 0; x < this.WIDTH; x++) {
                for (int z = 0; z < this.WIDTH; z++) {
                    for (int y = 0; y < Chunk.SIZE; y++) {
                        if (x == 0 || x == this.WIDTH - 1 || y == 0 || y == this.HEIGHT - 1 || z == 0 || z == Chunk.SIZE - 1) {
                            this.map[x][y][z] = 1;
                        }
                        else {
                            this.map[x][y][z] = (pseudoRandom.Next(0, 100) < this.randomFillPercent) ? 1 : 0;
                        }
                    }
                }
            }
        }

        //Smooths out the map
        private void smoothMap(int endY) {
            for (int x = 0; x < this.WIDTH; x++) {
                for (int z = 0; z < this.HEIGHT; z++) {
                    for (int y = 0; y < endY; y++) {
                        int neighbourWallTiles = this.getSurroundingWallCount(x, y, z);
                        if (neighbourWallTiles > 4) { //14
                            this.map[x][y][z] = 1;
                        }
                        else if (neighbourWallTiles < 4) {
                            this.map[x][y][z] = 0;
                        }
                    }
                }
            }
        }

        //Returns the number of surrounding walls
        private int getSurroundingWallCount(int gridX, int gridY, int gridZ) {
            int wallCount = 0;
            for (int x = gridX-1; x <= gridX+1; x++) {
                for (int z = gridZ-1; z <= gridZ+1; z++) {
                    //for (int y = gridY-1; y <= gridY+1; y++) {
                        if (this.isInMapRange(x, gridY, z)) {
                            if (x != gridX || z != gridZ) {
                                wallCount += this.map[x][gridY][z];
                            }
                        } else {
                            wallCount++;
                        }
                    //}
                }
            }

            return wallCount;
        }

        //private List<Room> fillAndDefineRegions() {
        //    List<List<Coord>> wallRegions = this.getRegions(1);

        //    if (this.enableTearingDown) {
        //        foreach (List<Coord> wallRegion in wallRegions) {
        //            if (wallRegion.Count < this.pillarRemoveSize) {
        //                foreach (Coord tile in wallRegion) {
        //                    this.map[tile.tileX][tile.tileY] = 0;
        //                }
        //            }
        //        }
        //    }

        //    List<List<Coord>> roomRegions = this.getRegions(0);
        //    List<Room> survivingRooms = new List<Room>();

        //    foreach (List<Coord> roomRegion in roomRegions) {
        //        if (this.enableFilling && roomRegion.Count < this.roomFillSize) {
        //            foreach (Coord tile in roomRegion) {
        //                map[tile.tileX][tile.tileY] = 1;
        //            }
        //        }
        //        else {
        //            survivingRooms.Add(new Room(roomRegion, map));
        //        }
        //    }
        //    survivingRooms.Sort();
        //    survivingRooms[0].isMainRoom = true;
        //    survivingRooms[0].isAccessibleFromMainRoom = true;

        //    return survivingRooms;
        //}

        //private void connectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false) {
        //    List<Room> roomListA = new List<Room>();
        //    List<Room> roomListB = new List<Room>();

        //    if (forceAccessibilityFromMainRoom) {
        //        foreach (Room room in allRooms) {
        //            if (room.isAccessibleFromMainRoom) {
        //                roomListB.Add(room);
        //            }
        //            else {
        //                roomListA.Add(room);
        //            }
        //        }
        //    }
        //    else {
        //        roomListA = allRooms;
        //        roomListB = allRooms;
        //    }

        //    int bestDistance = 0;
        //    Coord bestTileA = new Coord();
        //    Coord bestTileB = new Coord();
        //    Room bestRoomA = new Room();
        //    Room bestRoomB = new Room();
        //    bool possibleConnectionFound = false;

        //    foreach (Room roomA in roomListA) {
        //        if (!forceAccessibilityFromMainRoom) {
        //            possibleConnectionFound = false;
        //            if (roomA.connectedRooms.Count > 0) {
        //                continue;
        //            }
        //        }

        //        foreach (Room roomB in roomListB) {
        //            if (roomA == roomB || roomA.IsConnected(roomB)) {
        //                continue;
        //            }

        //            for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++) {
        //                for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++) {
        //                    Coord tileA = roomA.edgeTiles[tileIndexA];
        //                    Coord tileB = roomB.edgeTiles[tileIndexB];
        //                    int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));

        //                    if (distanceBetweenRooms < bestDistance || !possibleConnectionFound) {
        //                        bestDistance = distanceBetweenRooms;
        //                        possibleConnectionFound = true;
        //                        bestTileA = tileA;
        //                        bestTileB = tileB;
        //                        bestRoomA = roomA;
        //                        bestRoomB = roomB;
        //                    }
        //                }
        //            }
        //        }
        //        if (possibleConnectionFound && !forceAccessibilityFromMainRoom) {
        //            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
        //        }
        //    }

        //    if (possibleConnectionFound && forceAccessibilityFromMainRoom) {
        //        CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
        //        this.connectClosestRooms(allRooms, true);
        //    }

        //    if (!forceAccessibilityFromMainRoom) {
        //        this.connectClosestRooms(allRooms, true);
        //    }
        //}

        //void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB) {
        //    Room.ConnectRooms(roomA, roomB);
        //    //Debug.DrawLine (CoordToWorldPoint (tileA), CoordToWorldPoint (tileB), Color.red, 100);
        //    //List<Coord> line = GetLine(tileA, tileB);
        //    Coord[] line = this.GetLine(tileA, tileB);
        //    foreach (Coord c in line) {
        //        DrawCircle(c, this.hallwayRadius);
        //    }
        //}

        //void DrawCircle(Coord c, int r) {
        //    this.map[c.tileX][c.tileY] = 2;
        //    //return;
        //    for (int x = -r; x <= r; x++) {
        //        for (int y = -r; y <= r; y++) {
        //            if (x * x + y * y <= r * r) {
        //                int drawX = c.tileX + x;
        //                int drawY = c.tileY + y;
        //                if (isInMapRange(drawX, drawY)) {
        //                    map[drawX][drawY] = 0;
        //                }
        //            }
        //        }
        //    }
        //}

        //private Coord[] GetLine(Coord from, Coord to) {
        //    int x = from.tileX;
        //    int y = from.tileY;

        //    int dx = to.tileX - from.tileX;
        //    int dy = to.tileY - from.tileY;

        //    bool inverted = false;
        //    int step = Math.Sign(dx);
        //    int gradientStep = Math.Sign(dy);

        //    int longest = Mathf.Abs(dx);
        //    int shortest = Mathf.Abs(dy);

        //    int arrayIndex = 0;
        //    Coord[] line = new Coord[longest > shortest ? longest : shortest];

        //    if (longest < shortest) {
        //        inverted = true;
        //        longest = Mathf.Abs(dy);
        //        shortest = Mathf.Abs(dx);

        //        step = Math.Sign(dy);
        //        gradientStep = Math.Sign(dx);
        //    }

        //    int gradientAccumulation = longest / 2;
        //    for (int i = 0; i < longest; i++) {
        //        line[arrayIndex] = new Coord(x, y); //line.Add(new Coord(x, y));
        //        arrayIndex++;

        //        if (inverted) {
        //            y += step;
        //        }
        //        else {
        //            x += step;
        //        }

        //        gradientAccumulation += shortest;
        //        if (gradientAccumulation >= longest) {
        //            if (inverted) {
        //                x += gradientStep;
        //            }
        //            else {
        //                y += gradientStep;
        //            }
        //            gradientAccumulation -= longest;
        //        }
        //    }

        //    return line;
        //}

        //Vector3 CoordToWorldPoint(Coord tile) {
        //    return new Vector3(-width / 2 + .5f + tile.tileX, 2, -height / 2 + .5f + tile.tileY);
        //}

        //Returns a flood fill list of every region, a region being a list of coords
        //private List<List<Coord>> getRegions(int tileType) {
        //    List<List<Coord>> regions = new List<List<Coord>>();
        //    int[,] mapFlags = new int[width, height];

        //    for (int x = 0; x < width; x++) {
        //        for (int y = 0; y < height; y++) {
        //            if (mapFlags[x, y] == 0 && map[x][y] == tileType) {
        //                List<Coord> newRegion = GetRegionTiles(x, y);
        //                regions.Add(newRegion);

        //                foreach (Coord tile in newRegion) {
        //                    mapFlags[tile.tileX, tile.tileY] = 1;
        //                }
        //            }
        //        }
        //    }

        //    return regions;
        //}

        //List<Coord> GetRegionTiles(int startX, int startY) {
        //    List<Coord> tiles = new List<Coord>();
        //    int[,] mapFlags = new int[width, height];
        //    int tileType = map[startX][startY];

        //    Queue<Coord> queue = new Queue<Coord>();
        //    queue.Enqueue(new Coord(startX, startY));
        //    mapFlags[startX, startY] = 1;

        //    while (queue.Count > 0) {
        //        Coord tile = queue.Dequeue();
        //        tiles.Add(tile);

        //        for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
        //            for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
        //                if (isInMapRange(x, y) && (y == tile.tileY || x == tile.tileX)) {
        //                    if (mapFlags[x, y] == 0 && map[x][y] == tileType) {
        //                        mapFlags[x, y] = 1;
        //                        queue.Enqueue(new Coord(x, y));
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return tiles;
        //}
    }
}