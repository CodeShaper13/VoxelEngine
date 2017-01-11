using System.IO;
using VoxelEngine.Util;
using fNbt;
using VoxelEngine.Entities;

namespace VoxelEngine.Level.Save {

    public class SaveHelper {
        public WorldData data;
        public string saveFolderName;
        public string chunkFolderName;
        public string worldDataFileName;
        public string playerFileName;

        public SaveHelper(WorldData data) {
            this.data = data;
            this.saveFolderName = "saves/" + this.data.worldName + "/";
            this.chunkFolderName = this.saveFolderName + "chunks/";
            this.worldDataFileName = this.saveFolderName + "world.nbt";
            this.playerFileName = this.saveFolderName + "player.nbt";

            if(this.data.dontWriteToDisk) {
                return; //dont make folders if this world is a debug generated one
            }

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
            if (this.data.dontWriteToDisk) {
                return;
            }
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

        private string getChunkFileName2(ChunkPos pos) {
            return this.saveFolderName + "chunks/" + pos.x + "," + pos.y + "," + pos.z + ".nbt";
        }
    }
}
