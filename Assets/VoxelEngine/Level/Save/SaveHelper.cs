using System.IO;
using VoxelEngine.Util;
using fNbt;
using VoxelEngine.Entities;

namespace VoxelEngine.Level.Save {

    public class SaveHelper {
        public string worldName;
        public string saveFolderName;
        public string chunkFolderName;
        public string worldDataFileName;
        public string playerFileName;

        public SaveHelper(string worldName) {
            this.worldName = worldName;
            this.saveFolderName = "saves/" + this.worldName + "/";
            this.chunkFolderName = this.saveFolderName + "chunks/";
            this.worldDataFileName = this.saveFolderName + "world.nbt";
            this.playerFileName = this.saveFolderName + "player.nbt";

            if (!Directory.Exists(this.saveFolderName)) {
                Directory.CreateDirectory(this.saveFolderName);
            }
            if (!Directory.Exists(this.chunkFolderName)) {
                Directory.CreateDirectory(this.chunkFolderName);
            }
        }

        public void writeWorldData(WorldData worldData) {
            NbtFile file = new NbtFile(worldData.writeToNbt());
            file.SaveToFile(this.worldDataFileName, NbtCompression.None);
        }

        public bool readChunk(Chunk chunk) {
            string saveFile = this.getChunkFileName2(chunk.chunkPos);

            if (File.Exists(saveFile)) {
                NbtFile file = new NbtFile();
                file.LoadFromFile(saveFile);
                chunk.readFromNbt(file.RootTag);
                return true;
            }
            else {
                return false;
            }
        }

        public void writeChunkToDisk(Chunk chunk, NbtCompound tag) {
            NbtFile file = new NbtFile(tag);
            file.SaveToFile(this.getChunkFileName2(chunk.chunkPos), NbtCompression.None);
        }

        public void writePlayer(EntityPlayer player) {
            NbtCompound tag = new NbtCompound("player");
            NbtFile file = new NbtFile(player.writeToNbt(tag));
            file.SaveToFile(this.playerFileName, NbtCompression.None);
        }

        //Returns true if we found a player file
        public bool readPlayer(EntityPlayer player) {
            if (File.Exists(this.playerFileName)) {
                NbtFile file = new NbtFile();
                file.LoadFromFile(this.playerFileName);
                player.readFromNbt(file.RootTag);
                return true;
            }
            else {
                return false;
            }
        }

        private string getChunkFileName(ChunkPos pos) {
            return this.saveFolderName + "chunks/" + pos.x + "," + pos.y + "," + pos.z + ".bin";
        }

        private string getChunkFileName2(ChunkPos pos) {
            return this.saveFolderName + "chunks/" + pos.x + "," + pos.y + "," + pos.z + ".nbt";
        }
    }
}
