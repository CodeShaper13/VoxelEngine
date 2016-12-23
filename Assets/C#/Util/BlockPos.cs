﻿using UnityEngine;
using System;

//Struct for a position in the world, with several helper methods.
[Serializable]
public struct BlockPos {
    public static BlockPos zero = new BlockPos(0, 0, 0);
    public static BlockPos one = new BlockPos(1, 1, 1);
    public static BlockPos north = new BlockPos(0, 0, 1);
    public static BlockPos east = new BlockPos(1, 0, 0);
    public static BlockPos south = new BlockPos(0, 0, -1);
    public static BlockPos west = new BlockPos(-1, 0, 0);
    public static BlockPos up = new BlockPos(0, 1, 0);
    public static BlockPos down = new BlockPos(0, -1, 0);

    public int x;
    public int y;
    public int z;

    public BlockPos(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public ChunkPos toChunkPos() {
        return new ChunkPos(this.x / Chunk.SIZE, this.y / Chunk.SIZE, this.z / Chunk.SIZE);
    }

    public BlockPos move(Direction dir) {
        return new BlockPos(this.x + dir.direction.x, this.y + dir.direction.y, this.z + dir.direction.z);
    }

    public override string ToString() {
        return "(" + this.x + ", " + this.y + ", " + this.z + ")";
    }

    public override bool Equals(object obj) {
        return this.GetHashCode() == obj.GetHashCode();
    }

    public override int GetHashCode() {
        unchecked {
            int hash = 47;
            hash = hash * 227 + x.GetHashCode();
            hash = hash * 227 + y.GetHashCode();
            hash = hash * 227 + z.GetHashCode();
            return hash;
        }
    }

    public static BlockPos operator +(BlockPos b, BlockPos b1) {
        return new BlockPos(b.x + b1.x, b.y + b1.y, b.z + b1.z);
    }

    public static BlockPos operator -(BlockPos b, BlockPos b1) {
        return new BlockPos(b.x - b1.x, b.y - b1.y, b.z - b1.z);
    }

    public static BlockPos operator *(BlockPos b, int i) {
        return new BlockPos(b.x * i, b.y * i, b.z * i);
    }

    public static BlockPos operator /(BlockPos b, int i) {
        return new BlockPos(b.x / i, b.y / i, b.z / i);
    }

    public Vector3 toVector() {
        return new Vector3(this.x, this.y, this.z);
    }

    public static BlockPos fromVec(Vector3 vector) {
        return new BlockPos(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
    }

    public static BlockPos fromRaycast(RaycastHit hit, bool adjacent = false) {
        //Debug.Log(hit.point.x + ", " +  hit.point.y + ", " + hit.point.z);
        return BlockPos.fromVec(new Vector3(
            Util.moveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            Util.moveWithinBlock(hit.point.y, hit.normal.y, adjacent),
            Util.moveWithinBlock(hit.point.z, hit.normal.z, adjacent)));
    }
}