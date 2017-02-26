using UnityEngine;

namespace VoxelEngine.Util {

    public class HudCamera : MonoBehaviour {

        public static Camera cam;

        /*
        // Will be called from camera after regular rendering is done.
        //public void OnPostRender() {
            //if (!mat) {
            //    // Unity has a built-in shader that is useful for drawing
            //    // simple colored things. In this case, we just want to use
            //    // a blend mode that inverts destination colors.			
            //    var shader = Shader.Find("Hidden/Internal-Colored");
            //    mat = new Material(shader);
            //    mat.hideFlags = HideFlags.HideAndDontSave;
            //    // Set blend mode to invert destination colors.
            //    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusDstColor);
            //    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            //    // Turn off backface culling, depth writes, depth test.
            //    mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            //    mat.SetInt("_ZWrite", 0);
            //    mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
            //}

            //GL.PushMatrix();
            //GL.LoadOrtho();

            //// activate the first shader pass (in this case we know it is the only pass)
            //mat.SetPass(0);
            //// draw a quad over whole screen
            //GL.Begin(GL.QUADS);
            //GL.Vertex3(0, 0, 0);
            //GL.Vertex3(.25f, 0, 0);
            //GL.Vertex3(.25f, .25f, 0);
            //GL.Vertex3(0, .25f, 0);
            //GL.End();

            //GL.PopMatrix();

            //ItemStack stack = new ItemStack(Block.stone);
            //Vector3 pos = Vector3.one / 2;
            //IRenderItem render = stack.item.itemRenderer;
            //Material m = References.getMaterial(stack.item.id, true);
            //Graphics.DrawMesh(render.renderItem(stack).toMesh(), render.getMatrix(pos), m, 8, null, 0, null, false, false);

            //Mesh mesh = render.renderItem(new ItemStack(Block.stone)).toMesh();
            //m.SetPass(0);

            //Graphics.DrawMeshNow(mesh, render.getMatrix(pos));
        //}
        */

        void Awake() {
            if (HudCamera.cam == null) {
                HudCamera.cam = this.GetComponent<Camera>();
            } else {
                Debug.Log("ERROR!  There are more than one game objects with HudCamera script!");
            }
        }

        public static void bind(Canvas canvas) {
            canvas.transform.SetParent(HudCamera.cam.transform);
            canvas.worldCamera = HudCamera.cam;
        }
    }
}
