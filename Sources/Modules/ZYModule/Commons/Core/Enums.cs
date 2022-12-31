﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZYModule.Commons.Core;

public static class EnumUtils
{
    public static T[] GetEnums<T>() where T : Enum
    {
        return (from t in new object[] { typeof(T).GetEnumValues() } select (T)t).ToArray();
    }
}
public enum Direction : byte
{
    None = 0,
    Top = 1,
    Left = 2,
    Right = 4,
    Bottom = 8,
    TopLeft = Top | Left,
    TopRight = Top | Right,
    BottomLeft = Bottom | Left,
    BottomRight = Bottom | Right,
    Inside = 16
}
