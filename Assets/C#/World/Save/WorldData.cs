using System;
using System.IO;
using UnityEngine;

[Serializable]
public class WorldData {
    public string worldName;
    public long seed;
    public int worldType = 0;
    [NonSerialized]
    public Texture2D worldImage;

    public WorldData(string worldName) {
        this.worldName = worldName;
        this.seed = DateTime.Today.ToBinary();
    }

    public bool loadWorldImage() {
        string name = "saves/" + this.worldName + "/worldImage.png";
        if (File.Exists(name)) {
            byte[] fileData = File.ReadAllBytes(name);
            this.worldImage = new Texture2D(2, 2);
            this.worldImage.LoadImage(fileData);
            return true;
        }
        return false;
    }
}
