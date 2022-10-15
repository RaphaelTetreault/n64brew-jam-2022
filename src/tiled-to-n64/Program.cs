using CommandLine;
using Manifold.Tiled;
using System;
using System.IO;

namespace TiledToN64
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions);
        }

        public static void RunOptions(Options options)
        {
            if (options.Verbose)
            {
                options.PrintState();
                Console.WriteLine();
            }

            bool fileExists = File.Exists(options.InputPath);
            if (!fileExists)
                throw new FileLoadException($"No file found at path '{options.InputPath}'");

            var map = Map.FromFile(options.InputPath);

            using (var fs = File.Create(options.OutputPath))
                using (var writer = new StreamWriter(fs))
                    WriteCFile(writer, map);
        }

        public static void WriteCFile(StreamWriter writer, Map map)
        {
            WriteCollisionLayer(writer, map);
            WriteActors(writer, map);
            writer.Flush();        
        }


        public static byte MapGidToTileLayer(int gid)
        {
            byte layer = gid switch
            {
                1 => (byte)TileID.walkable,
                2 => (byte)TileID.wall,
                3 => (byte)TileID.gap,
                4 => (byte)TileID.grate,
                _ => 0x00,
            };
            return layer;
        }
        public static byte ObjectToActorID(Manifold.Tiled.Object @object)
        {
            byte actorID = @object.Class switch
            {
                "player" => (byte)ActorID.player,
                "gate3" => (byte)ActorID.gate3,
                "button" => 0x80,
                "switch" => 0x80,
                "switch2" => 0x80,
                "obj" => 0x80,
                "gem" => 0x80,
                _ => throw new NotImplementedException(@object.Class),
            };
            return actorID;
        }


        public static void WriteCollisionLayer(StreamWriter writer, Map map)
        {
            // todo: get length tile gids
            // get layer collision
            // get layer height
            // build together

            foreach (var layer in map.Layers)
            {
                if (layer.Data == null)
                    continue;
                if (layer.Name != "collision")
                    continue;

                writer.WriteLine($"// {layer.Name}");
                writer.WriteLine($"s16 col_lev_{layer.Name}[] = {{");
                for (int gidIndex = 0; gidIndex < layer.Data.TileGIDs.Length; gidIndex++)
                {
                    int gid = layer.Data.TileGIDs[gidIndex];
                    
                    var tile = new TileCollider();
                    tile.layer = MapGidToTileLayer(gid);
                    tile.height = 0;

                    tile.Write(writer);
                    if ((gidIndex+1) % map.Width == 0)
                        writer.WriteLine();
                }
                writer.WriteLine("};\n");
            }
        }
        public static void WriteActors(StreamWriter writer, Map map)
        {
            foreach (var objectGroup in map.ObjectGroups)
            {
                if (!objectGroup.HasObjects)
                    continue;
                if (objectGroup.Objects == null)
                    continue;

                writer.WriteLine($"TLevelObject obj_lev_{objectGroup.Name}[] = {{");
                foreach (var tiledObject in objectGroup.Objects)
                {
                    if (tiledObject == null)
                        continue;

                    var gobj = new GameObject();
                    gobj.id = ObjectToActorID(tiledObject);
                    gobj.px = (float)tiledObject.X / map.TileWidth + 0.5f;
                    gobj.pz = (float)tiledObject.Y / map.TileHeight + 0.5f;
                    gobj.Write(writer);
                    writer.WriteLine($"\t// {tiledObject.Name}");
                }
                GameObject terminator = new GameObject() { id = (byte)ActorID.end_marker };
                terminator.Write(writer);
                writer.WriteLine("};\n");
            }
        }


    }
}