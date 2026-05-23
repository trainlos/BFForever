using System;
using System.IO;
using BFForever.Texture;

namespace Xpr2Img;

public static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.Error.WriteLine("Usage: Xpr2Img <input.xpr> <output.png>");
            return 1;
        }
        var input  = args[0];
        var output = args[1];

        if (!File.Exists(input))
        {
            Console.Error.WriteLine($"Input file not found: {input}");
            return 2;
        }

        try
        {
            var xpr = XPR2.FromFile(input);
            xpr.WriteToImage(output);
            Console.WriteLine($"Wrote {output}");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to decode {input}: {ex.Message}");
            return 3;
        }
    }
}
