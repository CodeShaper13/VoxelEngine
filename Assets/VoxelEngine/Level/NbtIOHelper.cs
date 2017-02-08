using System.IO;
using VoxelEngine.Util;
using fNbt;
using VoxelEngine.Entities;
using VoxelEngine.Generation;
using UnityEngine;

namespace VoxelEngine.Level {

    public class NbtIOHelper {

        private bool dontWriteToDisk;
        private string saveFolderName;
        private string chunkFolderName;
        private string worldDataFileName;
        private string playerFileName;
        private string generationDataFileName;
        private string saveImageFileName;

        public NbtIOHelper(WorldData data) {
            this.dontWriteToDisk = data.dontWriteToDisk;
            this.saveFolderName = "saves/" + data.worldName + "/";
            this.chunkFolderName = this.saveFolderName + "chunks/";
            this.worldDataFileName = this.saveFolderName + "world.nbt";
            this.playerFileName = this.saveFolderName + "player.nbt";
            this.generationDataFileName = this.saveFolderName + "generationData.nbt";
            this.saveImageFileName = this.saveFolderName + "worldImage.png";

            this.makeDirIfMissing(this.saveFolderName);
            this.makeDirIfMissing(this.chunkFolderName);
        }

        public void writeWorldDataToDisk(WorldData worldData) {
            if (this.dontWriteToDisk) {
                return;
            }

            NbtFile file = new NbtFile(worldData.writeToNbt());
            file.SaveToFile(this.worldDataFileName, NbtCompression.None);
        }

        // Reading of world data in done in GuiScreenWorldSelect.Awake()

        public void writeChunkToDisk(Chunk chunk, NbtCompound tag) {
            if (this.dontWriteToDisk) {
                return;
            }

            NbtFile file = new NbtFile(tag);
            file.SaveToFile(this.getChunkFileName(chunk.chunkPos), NbtCompression.None);
        }

        // Returns true if the chunk was found
        public bool readChunk(Chunk chunk) {
            string saveFile = this.getChunkFileName(chunk.chunkPos);

            if (File.Exists(saveFile)) {
                NbtFile file = new NbtFile();
                file.LoadFromFile(saveFile);
                chunk.readFromNbt(file.RootTag);
                return true;
            }
            return false;
        }

        public void writePlayerToDisk(EntityPlayer player) {
            if (this.dontWriteToDisk) {
                return;
            }

            NbtFile file = new NbtFile(player.writeToNbt(new NbtCompound("player")));
            file.SaveToFile(this.playerFileName, NbtCompression.None);
        }

        // Returns true if we found a player file
        public bool readPlayerFromDisk(EntityPlayer player) {
            if (File.Exists(this.playerFileName)) {
                NbtFile file = new NbtFile();
                file.LoadFromFile(this.playerFileName);
                player.readFromNbt(file.RootTag);
                return true;
            }
            return false;
        }

        public void writeGenerationData(WorldGeneratorBase generator) {
            if (this.dontWriteToDisk) {
                return;
            }

            NbtFile file = new NbtFile(generator.writeToNbt(new NbtCompound("generationData")));
            file.SaveToFile(this.generationDataFileName, NbtCompression.None);
        }

        // Returns true if we found a generation data file
        public bool readGenerationData(WorldGeneratorBase generator) {
            if (File.Exists(this.generationDataFileName)) {
                NbtFile file = new NbtFile();
                file.LoadFromFile(this.generationDataFileName);
                generator.readFromNbt(file.RootTag);
                return true;
            }
            return false;
        }

        public void writeWorldImageToDisk() {
            if(this.dontWriteToDisk) {
                return;
            }

            ScreenshotHelper.captureScreenshot(this.saveImageFileName);
        }

        private string getChunkFileName(ChunkPos pos) {
            return this.saveFolderName + "chunks/" + pos.x + "," + pos.y + "," + pos.z + ".nbt";
        }

        private void makeDirIfMissing(string name) {
            if (!this.dontWriteToDisk && !Directory.Exists(name)) {
                Directory.CreateDirectory(name);
            }
        }
    }
}
