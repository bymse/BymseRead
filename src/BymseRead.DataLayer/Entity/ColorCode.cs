using BymseRead.DataLayer.Helpers;

namespace BymseRead.DataLayer.Entity;

public enum ColorCode
{
    [AttachedValue<string>("FFCACC")] Red = 1,
    [AttachedValue<string>("FFDCA9")] Orange = 2,

    [AttachedValue<string>("FBFACD")] Yellow = 3,
    [AttachedValue<string>("E8F3D6")] Green = 4,
    [AttachedValue<string>("CCF3EE")] Blue = 5,
    [AttachedValue<string>("F9CEEE")] Purple = 6,
    [AttachedValue<string>("FDFAF6")] White = 7,
}