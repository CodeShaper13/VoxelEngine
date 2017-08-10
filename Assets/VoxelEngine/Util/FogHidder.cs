using UnityEngine;

namespace VoxelEngine.Util {

    /// <summary>
    /// When attached to a camera, this disables the fog for rendering.
    /// Used on the HUD camera.
    /// </summary>
    public class FogHidder : MonoBehaviour {

        private bool previouseFogState;

        private void OnPreRender() {
            previouseFogState = RenderSettings.fog;
            RenderSettings.fog = false;;
        }

        private void OnPostRender() {
            RenderSettings.fog = previouseFogState;
        }
    }
}
