using UnityEngine;

namespace VoxelEngine {

    public class Options {

        private static Options currentOptions;

        public bool useSmoothLighting;
        /// <summary> Are EntityItem's 3d. </summary>
        public bool use3dModels;
        
        private Options() {
            this.useSmoothLighting = PlayerPrefs.GetInt("useSmoothLighting", 1) == 1;
            this.use3dModels = PlayerPrefs.GetInt("use3dModels", 1) == 1;
        }

        /// <summary>
        /// Creates a copy of the passed options.
        /// </summary>
        public Options(Options options) {
            this.useSmoothLighting = options.useSmoothLighting;
            this.use3dModels = options.use3dModels;
        }

        public static void loadInitialOptions() {
            Options.currentOptions = new Options();
        }

        public static Options get() {
            return Options.currentOptions;
        }

        /// <summary>
        /// Applies the option, updating and rebaking any needed things.
        /// </summary>
        public void apply(Options newOptions) {
            Options oldOptions = Options.get();

            // Apply changes
            if(newOptions.useSmoothLighting != oldOptions.useSmoothLighting) {
                Main.singleton.worldObj.rebakeWorld();
            }

            Options.currentOptions = this;

            // Save new options
            this.saveChanges();
        }

        private void saveChanges() {
            PlayerPrefs.SetInt("useSmoothLighting", this.useSmoothLighting ? 1 : 0);
            PlayerPrefs.SetInt("use3dModels", this.use3dModels ? 1 : 0);
        }
    }
}
