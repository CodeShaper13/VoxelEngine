using UnityEngine;
using UnityEngine.Profiling;

namespace VoxelEngine.Testing {

    public class ArrayVsBits : MonoBehaviour {

        public void Start() {
            for (int i = 0; i < 1000; i++) {
                this.func1();
            }
            for (int i = 0; i < 1000; i++) {
                this.func2();
            }
        }

        private void func1() {
            int x, y, z, i;
            int flags = 0;
            for (x = 0; x < 15; x++) {
                for (y = 0; y < 15; y++) {
                    for (z = 0; z < 15; z++) {
                        Profiler.BeginSample("Inner loop");
                        for (i = 0; i < 6; i++) {
                            flags = flags >> i;
                        }
                        Profiler.EndSample();
                    }
                }
            }
        }

        private void func2() {
            int x, y, z, i;
            bool[] flags = new bool[6];
            for(x = 0; x < 15; x++) {
                for (y = 0; y < 15; y++) {
                    for (z = 0; z < 15; z++) {
                        Profiler.BeginSample("Inner loop");
                        for(i = 0; i < 6; i++) {
                            flags[i] = !flags[i];
                        }
                        Profiler.EndSample();
                    }
                }
            }
        }
    }
}
