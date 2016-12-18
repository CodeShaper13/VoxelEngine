using System;

[Serializable]
public class WorldData {

    public string worldName;
    public long seed;
    public int worldType = 0;

    public int spawnX;
    public int spawnY;
    public int spawnZ;

    public WorldData(string worldName) {
        this.worldName = worldName;
        this.seed = DateTime.Today.ToBinary();
    }
}
