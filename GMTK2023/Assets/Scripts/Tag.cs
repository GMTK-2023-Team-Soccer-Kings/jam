using System.Collections.Generic;

[System.Flags]
public enum Tag
{
    None = 0,
    Adventure = 1,
    StoryRich = 2,
    Difficult = 4,
    Platformer = 8,
    Horror = 16,
    Charming = 32,
    Gore = 64,
    Roguelike = 128,
    Puzzle = 256,
    Simulator = 512,
    Survival = 1024,
    CardGame = 2048,
    RPG = 4096,
    Emoticon = 8192,
    End = 16384
}


public static class TagData
{
    public static readonly Dictionary<string, Tag> StringToTag = new Dictionary<string, Tag>()
    {
        { "none",        Tag.None         },
        { "adv",   Tag.Adventure    },
        { "stry",   Tag.StoryRich    },
        { "hrd",        Tag.Difficult    },
        { "jump",       Tag.Platformer   },
        { "hrr",      Tag.Horror       },
        { "chrm",     Tag.Charming     },
        { "gore",     Tag.Gore         },
        { "rl",      Tag.Roguelike    },
        { "pzl",      Tag.Puzzle       },
        { "sim",   Tag.Simulator    },
        { "srv",    Tag.Survival     },
        { "cg",    Tag.CardGame     },
        { "rpg",         Tag.RPG          },
        { ":)",         Tag.Emoticon          },
    };
}