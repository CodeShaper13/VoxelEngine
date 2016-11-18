using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public static class Serialization {
    public static string saveFolderName = "voxelGameSaves";

    public static string SaveLocation(string worldName) {
        string saveLocation = saveFolderName + "/" + worldName + "/";

        if (!Directory.Exists(saveLocation)) {
            Directory.CreateDirectory(saveLocation);
        }
        return saveLocation;
    }

    public static string getChunkFileName(BlockPos chunkLocation) {
        return chunkLocation.x + "," + chunkLocation.y + "," + chunkLocation.z + ".bin";
    }

    public static void saveChunk(Chunk chunk) {
        //Save save = new Save(chunk);
        //if (save.blocks.Count == 0)
        //    return;
        string saveFile = SaveLocation(chunk.world.worldName) + getChunkFileName(chunk.pos);

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(saveFile, FileMode.Create, FileAccess.Write, FileShare.None);

        byte[] blockIds = new byte[Chunk.BLOCK_COUNT];
        for(int i = 0; i < Chunk.SIZE; i++) {
            blockIds[i] = chunk.blocks[i].id;
        }

        formatter.Serialize(stream, blockIds);
        stream.Close();
    }

    //returns false if their was no file, meaning no chunk to load
    public static bool loadChunk(Chunk chunk) {
        string saveFile = SaveLocation(chunk.world.worldName) + getChunkFileName(chunk.pos);

        if (!File.Exists(saveFile)) {
            return false;
        }

        IFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(saveFile, FileMode.Open);

        byte[] blockIds = (byte[])formatter.Deserialize(stream);
        for(int i = 0; i < Chunk.BLOCK_COUNT; i++) {
            chunk.blocks[i] = Block.getBlock(blockIds[i]);
        }

        //OLD ^  Save save = (Save)formatter.Deserialize(stream);
        //foreach (var block in save.blocks) {
        //    chunk.blocks[block.Key.x, block.Key.y, block.Key.z] = block.Value;
        //}

        stream.Close();
        return true;
    }
}