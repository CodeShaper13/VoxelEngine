//Helper methods that don't fit in any other class.
public class Util {

    public static float moveWithinBlock(float point, float normal, bool adjacent = false) {
        if ((point - (int)point) == 0.5f || (point - (int)point) == -0.5f) {
            if (adjacent) {
                point += (normal / 2);
            }
            else {
                point -= (normal / 2);
            }
        }
        return point;
    }

    public static bool inChunkBounds(int i) {
        if (i < 0 || i >= Chunk.SIZE) {
            return false;
        }
        return true;
    }
}
