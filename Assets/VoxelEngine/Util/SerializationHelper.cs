using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace VoxelEngine.Util {

    public static class SerializationHelper {

        public static void serialize(object obj, string path) {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, obj);
            stream.Close();
        }

        public static object deserialize(string path) {
            if (File.Exists(path)) {
                IFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                object obj = formatter.Deserialize(stream);
                stream.Close();
                return obj;
            }
            else {
                return null;
            }
        }
    }
}
