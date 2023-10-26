using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyFloorTemplate : FloorRoomTemplate {
    public override string ShortChar => "#";
}

public class ShopFloorTemplate : FloorRoomTemplate {
    public override string ShortChar => "$";
}

public class TreasureFloorTemplate : FloorRoomTemplate {
    public override string ShortChar => "*";
}

public class BossFloorTemplate : FloorRoomTemplate {
    public override string ShortChar => "!";
}


public class FloorRoomTemplate {
    public virtual string ShortChar => "?";
    public virtual string Char()
    {
        return ShortChar;
    }
}

public class FloorTemplate
{
    private readonly Dictionary<char, FloorRoomTemplate?> _basicTiles = new() {
        {'.', null},
        {'#', new EnemyFloorTemplate()},
        {'$', new ShopFloorTemplate()},
        {'*', new TreasureFloorTemplate()},
        {'!', new BossFloorTemplate()},
    };

    public int Height => Rooms.GetLength(0);
    public int Width => Rooms.GetLength(1);
    public FloorRoomTemplate?[,] Rooms { get; }
    public int SpawnX { get; }
    public int SpawnY { get; }

    public static FloorTemplate Generate(int width, int height) {

        var rooms = new FloorRoomTemplate?[width,height];
        var startX = Random.Range(1, width - 1);
        var startY = Random.Range(1, height - 1);
        rooms[startX, startY] = new FloorRoomTemplate();
        var result = new FloorTemplate(rooms);

        return result;
    }

    public FloorTemplate(FloorRoomTemplate[,] rooms) {
        Rooms = rooms;
    }

    public FloorTemplate(string preset) {
        var lines = preset.Split("\n");
        Rooms = new FloorRoomTemplate?[lines[0].Length,lines.Length];
        for (int i = 0; i < lines.Length; i++) {
            var line = lines[i];
            for (int ii = 0; ii < line.Length; ii++) {
                var c = line[ii];
                if (c == '@') {
                    SpawnX = ii;
                    SpawnY = i;
                    c = '#';
                }
                var tile = _basicTiles[c];
                Rooms[i, ii] = tile;
            }
        }
    }

    public override string ToString()
    {
        var result = "";
        
        for (int i = 0; i < Height; i++) {
            for (int ii = 0; ii < Width; ii++) {
                result += Rooms[i, ii] is not null ? Rooms[i, ii].Char() : ".";
                result += " ";
            }
            result += "\n";
        }    

        return result;
    }
}

[System.Serializable]
public class RoomChance {
    public GameObject room;
    public int chance; 
}

[CreateAssetMenu(fileName = "Floor")]
public class Floor : ScriptableObject {
    public new string name;
    public List<RoomChance> roomLayouts;

    public GameObject RandomRoom() {
        var count = 0;
        foreach (var pair in roomLayouts)
            count += pair.chance;
        var v = Random.Range(1, count + 1);
        foreach (var pair in roomLayouts) {
            if (v > pair.chance) {
                v -= pair.chance;
                continue;
            }

            return pair.room;
        }

        return null;
    }
}