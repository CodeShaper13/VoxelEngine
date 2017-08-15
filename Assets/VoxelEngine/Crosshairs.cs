using UnityEngine;

namespace VoxelEngine {

    public class Crosshairs : MonoBehaviour {

        public Material material;

        private void OnPostRender() {
            GL.PushMatrix();
            this.material.SetPass(0);
            if (Main.isDeveloperMode) {
                this.drawDebugAxisLine(Color.red, Vector3.right);
                this.drawDebugAxisLine(Color.green, Vector3.up);
                this.drawDebugAxisLine(Color.blue, Vector3.forward);
            } else {
                this.drawCrosshairsLine(this.transform.up);
                this.drawCrosshairsLine(this.transform.right);
            }
            GL.PopMatrix();
        }

        private void drawDebugAxisLine(Color color, Vector3 direction) {
            Vector3 vertex = this.transform.position + (this.transform.forward * 0.1f);
            GL.Begin(GL.LINES);
            GL.Color(color);
            GL.Vertex(vertex);
            GL.Vertex(vertex + (direction * 0.01f));
            GL.End();
        }

        private void drawCrosshairsLine(Vector3 dir) {
            dir *= 0.0025f;
            Vector3 vertex = this.transform.position + (this.transform.forward * 0.1f);
            GL.Begin(GL.LINES);
            GL.Color(Color.black);
            GL.Vertex(vertex + dir);
            GL.Vertex(vertex - dir);
            GL.End();
        }
    }
}
