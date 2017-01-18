namespace VoxelEngine.Generation.CellularAutomaton {

    public class GeneratorOptions {
        public static GeneratorOptions DEFAULT_OPTIONS = new GeneratorOptions(54, 3, true, 15, true, 25, true, 1);

        public int fillPercent;
        public int smoothPasses;
        public bool enablePillarTearing;
        public int pillarTearSize;
        public bool enablePocketFilling;
        public int pocketFillSize;
        public bool joinRooms;
        public int hallwaySize;

        public GeneratorOptions(int fillPercent, int smoothPasses, bool flag, int pillerTearSize, bool flag1, int pocketFillSize, bool flag2, int m) {
            this.fillPercent = fillPercent;
            this.smoothPasses = smoothPasses;
            this.enablePillarTearing = flag;
            this.pillarTearSize = pillerTearSize;
            this.enablePocketFilling = flag1;
            this.pocketFillSize = pocketFillSize;
            this.joinRooms = flag2;
            this.hallwaySize = m;
        }
    }
}
