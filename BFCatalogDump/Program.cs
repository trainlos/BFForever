using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BFForever.Riff;

namespace BFCatalogDump;

public static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.Error.WriteLine("Usage: BFCatalogDump <catalog2.rif>");
            return 1;
        }

        var rif = RiffFile.FromFile(args[0]);
        var catalog = rif.Objects.OfType<Catalog2>().FirstOrDefault();
        if (catalog == null)
        {
            Console.Error.WriteLine("No Catalog2 zobject found in " + args[0]);
            return 2;
        }

        var entries = catalog.Entries.Select(EntryToDict).ToList();
        var json = JsonSerializer.Serialize(entries, new JsonSerializerOptions
        {
            WriteIndented = true,
        });
        Console.WriteLine(json);
        return 0;
    }

    private static Dictionary<string, object?> EntryToDict(Catalog2Entry e)
    {
        return new Dictionary<string, object?>
        {
            ["identifier"]        = (string?)e.Identifier,
            ["song_type"]         = e.SongType,
            ["title"]             = (string?)e.Title,
            ["artist"]            = (string?)e.Artist,
            ["album"]             = (string?)e.Album,
            ["description"]       = (string?)e.Description,
            ["legend_tag"]        = (string?)e.LegendTag,
            ["era_tag"]           = (string?)e.EraTag,
            ["year"]              = e.Year,
            ["song_length"]       = e.SongLength,
            ["song_path"]         = (string?)e.SongPath,
            ["texture_path"]      = (string?)e.TexturePath,
            ["preview_path"]      = (string?)e.PreviewPath,
            ["labels"]            = e.Labels.Select(l => (string?)l).ToList(),
            ["metadata_tags"]     = e.MetadataTags.Select(t => (string?)t).ToList(),
            ["genre_tags"]        = e.GenreTags.Select(t => (string?)t).ToList(),
            ["guitar_intensity"]  = e.GuitarIntensity,
            ["bass_intensity"]    = e.BassIntensity,
            ["vox_intensity"]     = e.VoxIntensity,
            ["lead_guitar"]       = TuningToList(e.LeadGuitarTuning, 6),
            ["rhythm_guitar"]     = TuningToList(e.RhythmGuitarTuning, 6),
            ["bass"]              = TuningToList(e.BassTuning, 4),
            ["unknown1"]          = e.Unknown1,
            ["unknown2"]          = e.Unknown2,
            ["unknown3"]          = e.Unknown3,
            ["unknown4"]          = e.Unknown4,
            ["unknown5"]          = e.Unknown5,
        };
    }

    private static List<int> TuningToList(InstrumentTuning t, int n)
    {
        var pitches = new[] {
            (int)t.String1, (int)t.String2, (int)t.String3,
            (int)t.String4, (int)t.String5, (int)t.String6,
        };
        return pitches.Take(n).ToList();
    }
}
