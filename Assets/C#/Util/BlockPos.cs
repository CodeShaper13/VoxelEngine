using UnityEngine;
using System.Collections;
using System;

[Serializable]
public struct BlockPos {
    public int x, y, z;

    public BlockPos(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public BlockPos toChunkPos() {
        return new BlockPos(this.x / Chunk.SIZE, this.y / Chunk.SIZE, this.z / Chunk.SIZE);
    }

    public BlockPos toWorldPos() {
        return new BlockPos(this.x * Chunk.SIZE, this.y * Chunk.SIZE, this.z * Chunk.SIZE);
    }

    public BlockPos move(Direction dir) {
        //Debug.Log(this + dir.direction);
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
        return BlockPos.fromVec(new Vector3(
            Util.MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            Util.MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
            Util.MoveWithinBlock(hit.point.z, hit.normal.z, adjacent)));
    }
}