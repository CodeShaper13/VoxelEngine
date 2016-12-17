using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class GenerationFeature {

    public abstract void generate(World world, int x, int y, int z);

    public abstract bool shouldGenerate(World world, int x, int y, int z);
}
