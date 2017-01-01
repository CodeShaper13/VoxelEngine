using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class SaveHandler {
    public string worldName;
    public string saveFolderName;
    public string chunkFolderName;
    public string worldDataFileName;

    public SaveHandler(string worldName) {
        this.worldName = worldName;
        this.saveFolderName = "saves/" + this.worldName + "/";
        this.chunkFolderName = this.saveFolderName + "chunks/";
        this.worldDataFileName = this.saveFolderName + this.worldName + ".bin";

        if (!Directory.Exists(this.saveFolderName)) {
            Directory.CreateDirectory(this.saveFolderName);
        }
        if (!Directory.Exists(this.chunkFolderName)) {
            Directory.CreateDirectory(this.chunkFolderName);
        }
    }

    public WorldData getWorldData() {
        object obj = SerializationHelper.deserialize(this.worldDataFileName);

        if (obj == null) {
            return new WorldData(this.worldName);
        } else {
            return (WorldData)obj;
        }
    }

    public void serializeWorldData(WorldData worldData) {
        SerializationHelper.serialize(worldData, this.worldDataFileName);
    }

    public bool deserializeChunk(Chunk chunk) {
        string saveFile = this.getChunkFileName(chunk.chunkPos);

        if (File.Exists(saveFile)) {
            IFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(saveFile, FileMode.Open);

            byte[] blockIds = (byte[])formatter.Deserialize(stream);
            for (int i = 0; i < Chunk.BLOCK_COUNT; i++) {
                chunk.blocks[i] = Block.getBlock(blockIds[i]);
            }

            stream.Close();
            return true;
        } else {
            return false;
        }
    }

    public void serializeChunk(Chunk chunk) {
        byte[] blockIds = new byte[Chunk.BLOCK_COUNT];
        for (int i = 0; i < Chunk.BLOCK_COUNT; i++) {
            blockIds[i] = chunk.blocks[i].id;
        }

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(this.getChunkFileName(chunk.chunkPos), FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, blockIds);
        stream.Close();
    }

    private string getChunkFileName(ChunkPos pos) {
        return this.saveFolderName + "chunks/" + pos.x + "," + pos.y + "," + pos.z + ".bin";
    }
}
