using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;
using VoxelEngine.Level;

namespace Assets.VoxelEngine.Testing {

    public class LoopTest : MonoBehaviour {

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
        }

        private void func1() {
            int x, y, z;
            for (int index = 0; index < Chunk.BLOCK_COUNT; index++) {
                x = index % Chunk.SIZE;
                y = (index - x) / Chunk.SIZE % Chunk.SIZE;
                z = ((index - x) / Chunk.SIZE - y) / Chunk.SIZE;
            }
        }

        private void func2() {
            int x, y, z;
            for (x = 0; x < Chunk.SIZE; x++) {
                for (y = 0; y < Chunk.SIZE; y++) {
                    for (z = 0; z < Chunk.SIZE; z++) {
                    }
                }
            }
        }
    }
}
