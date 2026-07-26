# 1

## 143

```C#
// Main.tile[val9.X, val9.Y] = default(Tile);
IL_0e4c: ldsflda valuetype Terraria.Tilemap Terraria.Main::tile
IL_0e51: ldloc.s 43
IL_0e53: ldfld int32 [FNA]Microsoft.Xna.Framework.Point::X
IL_0e58: ldloc.s 43
IL_0e5a: ldfld int32 [FNA]Microsoft.Xna.Framework.Point::Y
IL_0e5f: ldloca.s 9
IL_0e61: initobj Terraria.Tile
IL_0e67: ldloc.s 9

// bool flag = true;
IL_0e69: call instance void Terraria.Tilemap::set_Item(int32, int32, valuetype Terraria.Tile)

IL_0e6e: ldc.i4.1
IL_0e6f: stloc.s 44
// Cursor Here

// if (Main.tile[val9.X, val9.Y].nactive() &&
// AI_007_GrapplingHooks_CanTileBeLatchedOnTo(Main.tile[val9.X, val9.Y]))
IL_0e71: ldsflda valuetype Terraria.Tilemap Terraria.Main::tile
IL_0e76: ldloc.s 43
IL_0e78: ldfld int32 [FNA]Microsoft.Xna.Framework.Point::X
IL_0e7d: ldloc.s 43
IL_0e7f: ldfld int32 [FNA]Microsoft.Xna.Framework.Point::Y
IL_0e84: call instance valuetype Terraria.Tile Terraria.Tilemap::get_Item(int32, int32)
IL_0e89: stloc.s 9

// (no C# code)
IL_0e8b: ldloca.s 9
IL_0e8d: call instance bool Terraria.Tile::nactive()
IL_0e92: brfalse.s IL_0eb8
```

```C#
cursor.TryGotoNext(
	MoveType.After, 
	ins => (ins.Previous?.MatchLdcI4(1) ?? false) &&
			ins.MatchStloc(44) &&
			(ins.Next?.MatchLdsflda<Main>(nameof(Main.tile)) ?? false))
```

## 144

```c#
// Main.tile[point5.X, point4.Y] = default(Tile);
IL_0e42: ldsflda valuetype Terraria.Tilemap Terraria.Main::tile
IL_0e47: ldloc.s 43
IL_0e49: ldfld int32 [FNA]Microsoft.Xna.Framework.Point::X
IL_0e4e: ldloc.s 43
IL_0e50: ldfld int32 [FNA]Microsoft.Xna.Framework.Point::Y
IL_0e55: ldloca.s 9
IL_0e57: initobj Terraria.Tile
IL_0e5d: ldloc.s 9
// bool flag = true;
IL_0e5f: call instance void Terraria.Tilemap::set_Item(int32, int32, valuetype Terraria.Tile)

IL_0e64: ldc.i4.1
IL_0e65: stloc.s 44
// Cursor Here

// if (AI_007_GrapplingHooks_CanTileBeLatchedOnTo(point4.X, point4.Y))
IL_0e67: ldarg.0
IL_0e68: ldloc.s 43
IL_0e6a: ldfld int32 [FNA]Microsoft.Xna.Framework.Point::X
IL_0e6f: ldloc.s 43
IL_0e71: ldfld int32 [FNA]Microsoft.Xna.Framework.Point::Y
IL_0e76: call instance bool Terraria.Projectile::AI_007_GrapplingHooks_CanTileBeLatchedOnTo(int32, int32)
IL_0e7b: brfalse.s IL_0e80

// flag = false;
IL_0e7d: ldc.i4.0
IL_0e7e: stloc.s 45
```

```C#
cursor.TryGotoNext(MoveType.After, ins => ins.Offset == 0x0e65)
```

# 2

## 143

```c#
// if (flag)
IL_0eb7: ldloc.s 44
IL_0eb9: brfalse.s IL_0ec9

// ai[0] = 1f;
IL_0ebb: ldarg.0
IL_0ebc: ldfld float32[] Terraria.Projectile::ai
IL_0ec1: ldc.i4.0
IL_0ec2: ldc.r4 1
IL_0ec7: stelem.r4

// else if (Main.player[owner].grapCount < 10)
// Cursor Here
IL_0ec8: ret

IL_0ec9: ldsfld class Terraria.Player[] Terraria.Main::player
IL_0ece: ldarg.0
IL_0ecf: ldfld int32 Terraria.Projectile::owner
IL_0ed4: ldelem.ref
IL_0ed5: ldfld int32 Terraria.Player::grapCount
IL_0eda: ldc.i4.s 10
IL_0edc: bge.s IL_0f20
```

```c#
cursor.TryGotoNext(
    MoveType.Before, 
    ins => ins.MatchLdsfld<Main>(nameof(Main.player)) && ins.Previous.MatchRet())
```

## 144

```c#
// if (flag)
IL_0e80: ldloc.s 44
IL_0e82: brfalse.s IL_0e92

// ai[0] = 1f;
IL_0e84: ldarg.0
IL_0e85: ldfld float32[] Terraria.Projectile::ai
IL_0e8a: ldc.i4.0
IL_0e8b: ldc.r4 1
IL_0e90: stelem.r4
// else if (Main.player[owner].grapCount < 10)
// Cursor Here
IL_0e91: ret

IL_0e92: ldsfld class Terraria.Player[] Terraria.Main::player
IL_0e97: ldarg.0
IL_0e98: ldfld int32 Terraria.Projectile::owner
IL_0e9d: ldelem.ref
IL_0e9e: ldfld int32 Terraria.Player::grapCount
IL_0ea3: ldc.i4.s 10
IL_0ea5: bge.s IL_0ee9 
```

```C#
cursor.TryGotoNext(MoveType.Before, ins => ins.Offset == 0x0E91)
```




==========================

==========================

