//Helper methods that don't fit in any other class.
public class Util {
    public static float moveWithinBlock(float pos, float norm, bool adjacent = false) {
        if (pos - (int)pos == 0.5f || pos - (int)pos == -0.5f) {
            if (adjacent) {
                pos += (norm / 2);
            }
            else {
                pos -= (norm / 2);
            }
        }
        return pos;
    }

    public static bool inChunkBounds(int i) {
        if (i < 0 || i >= Chunk.SIZE) {
            return false;
        }
        return true;
    }
}
