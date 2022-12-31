﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Function.NetUtils;

public static class NetUtils
{
    //没有自动补全的const int比较写起来很麻烦，直接写成一个属性便于阅读
    public static bool IsSingle => Main.netMode == NetmodeID.SinglePlayer;
    public static bool IsServer => Main.netMode == NetmodeID.Server;
    public static bool IsClient => Main.netMode == NetmodeID.MultiplayerClient;
    public static bool NotServer => Main.netMode != NetmodeID.Server;
    public static bool NotSingle => Main.netMode != NetmodeID.SinglePlayer;
    public static bool NotClient => Main.netMode != NetmodeID.MultiplayerClient;
}
