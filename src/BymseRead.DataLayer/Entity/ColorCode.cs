using BymseRead.DataLayer.Helpers;

namespace BymseRead.DataLayer.Entity;

public enum ColorCode
{
    [AttachedValue<string>("FDFAF6")] White = 7,
    [AttachedValue<string>("F2E1E1")] Red = 1,
    [AttachedValue<string>("FFFECA")] Yellow = 3,
    [AttachedValue<string>("EEFFCA")] Green = 4,
}