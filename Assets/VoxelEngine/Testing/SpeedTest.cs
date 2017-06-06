using UnityEngine;
using UnityEngine.Profiling;

namespace Assets.VoxelEngine.Testing {

    public class SpeedTest : MonoBehaviour {

        public void Start() {
            Profiler.BeginSample("Func 1");
            for (int i = 0; i < 1000; i++) {
                this.func1();
            }
            Profiler.EndSample();
            Profiler.BeginSample("Func 2");
            for (int i = 0; i < 1000; i++) {
                this.func2();
            }
            Profiler.EndSample();
            Profiler.BeginSample("Func 3");
            for (int i = 0; i < 1000; i++) {
                this.func3();
            }
            Profiler.EndSample();
        }

        private void func1() {
            int x, y, z;
            for (x = 0; x < 15; x++) {
                for (y = 0; y < 15; y++) {
                    for (z = 0; z < 15; z++) {
                        int i = 0;
                    }
                }
            }
        }

        private void func2() {
            int x;
            for (x = 0; x < 3375; x++) {
                int i = 0;
            }
        }

        private void func3() {
            int x = 0;
            int y = 0;
            int z = 0;
            for (int i = 0; i < 3375; i++) {
                x++;

                if(x >= 15) {
                    x = 0;
                    y++;
                }
                if (y >= 15) {
                    y = 0;
                    z++;
                }
                if (z >= 15) {
                    z = 0;
                    x++;
                }
            }
        }
    }
}
