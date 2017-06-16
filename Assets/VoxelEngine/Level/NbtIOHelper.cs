using System.IO;
using VoxelEngine.Util;
using fNbt;
using VoxelEngine.Entities;
using VoxelEngine.Generation;

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
            this.dontWriteToDisk = !data.writeToDisk;
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

        /// <summary>
        /// Tries to read the passed chunk from the disk, returning true if it was found.
        /// </summary>
        public bool readChunkFromDisk(Chunk chunk) {
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

        /// <summary>
        /// Tries to read the player data from the disk, returning true if it was found.
        /// </summary>
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

        /// <summary>
        /// Tries to read the world data from the disk, returning true if it was found.
        /// </summary>
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
