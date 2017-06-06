using UnityEngine;
using UnityEngine.Profiling;
using VoxelEngine.Util;

namespace VoxelEngine.Testing {

    public class ArrayVsBits : MonoBehaviour {

        public void Start() {
            Profiler.BeginSample("Func inlined bit");
            for (int i = 0; i < 1000; i++) {
                this.func3();
            }
            Profiler.EndSample();


            Profiler.BeginSample("Func array");
            for (int i = 0; i < 1000; i++) {
                this.func2();
            }
            Profiler.EndSample();
        }

        private void func3() {
            int x, y, z, i, j;
            int flag = 0;
            for (x = 0; x < 15; x++) {
                for (y = 0; y < 15; y++) {
                    for (z = 0; z < 15; z++) {
                        for (i = 0; i < 6; i++) {
                            flag |= 1 << i;
                            j = ((flag >> i) & 1);
                        }
                        this.passFunc2(flag);
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
                        for(i = 0; i < 6; i++) {
                            flags[i] = !flags[i];
                        }
                        this.passFunc1(flags);
                    }
                }
            }
        }

        private void passFunc1(bool[] f) {

        }

        private void passFunc2(int flag) {

        }
    }
}
