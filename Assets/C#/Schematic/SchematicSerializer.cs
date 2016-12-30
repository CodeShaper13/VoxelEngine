using UnityEngine;

public class SchematicSerializer {

    public static string path = "gameData/schematics/";
    
    public static void saveSchematic(Schematic schematic) {
        SerializationHelper.serialize(schematic, schematic.name);
    }

    public static Schematic loadSchematic(string schematicName) {
        Schematic s = null;
        if(schematicName != s.name) {
            Debug.LogError("Schematic name and file name do not mathc, schematic may be corrupted!");
        }
        return s;
    }
}
