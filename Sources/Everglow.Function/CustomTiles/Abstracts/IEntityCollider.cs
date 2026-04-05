using Everglow.Commons.CustomTiles.Core;
using Everglow.Commons.Physics.DataStructures;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.CustomTiles.Abstracts;

public interface IEntityCollider<TEntity> : IBox
	where TEntity : Entity
{
	private const float Sqrt2Div2 = 0.707106781186f;

	public TEntity Entity { get; }

	public RigidEntity Ground { get; set; }

	public float OffsetY { get; set; }

	public Vector2 OldPosition { get; set; }

	public void OnCollision(CollisionResult result);

	public void OnLeave();

	public void Prepare()
	{
		// if(Entity is Player)
		// {
		// if (Ground is not null)
		// {
		// Main.NewText(Ground.ToString() + ": " + Ground.Position.Y);
		// }
		// else
		// {
		// Main.NewText("null");
		// }
		// }
		if (Ground != null && OffsetY != 0)
		{
			bool entityCollideTile = Collision.SolidCollision(Entity.position + new Vector2(0, OffsetY - 0.4f), Entity.width, Entity.height);
			Vector2 nextEntityPos = Entity.position + Entity.velocity + new Vector2(0, OffsetY - 0.4f);
			ColliderManager.EnableHook = true;
			bool entityCollideTile_Next = Collision.SolidCollision(nextEntityPos, Entity.width, Entity.height);
			ColliderManager.EnableHook = false;
			if (entityCollideTile_Next)
			{
				int collideCount = 0;
				bool collideGround = false;
				AABB entityBox = new AABB(nextEntityPos, Entity.Size);
				foreach (var customTile in ColliderManager.Instance.OfType<BoxEntity>())
				{
					if (customTile.Intersect(entityBox))
					{
						collideCount++;
						if (customTile == Ground)
						{
							collideGround = true;
						}
					}
				}
				if (collideCount == 1 && collideGround)
				{
					entityCollideTile_Next = false;
				}
			}
			if (!entityCollideTile && !entityCollideTile_Next)
			{
				// This code is for preventing player from sticking to the ground. Only when player stand on customtile and not on solid tile or try to step up to a solid tile, the player will get unstuck. This code will not cause player to get unstuck when player is standing on solid tile, which is for preventing player from getting unstuck when standing on solid tile and trying to step up to customtile.
				CancelAttachToSolidTile();
			}
		}
		OldPosition = Entity.position;
	}

	public void CancelAttachToSolidTile()
	{
		Entity.position.Y += OffsetY;
		OffsetY = 0;
	}

	public void Update()
	{
		var stride = Entity.position - OldPosition;

		// Apply standing behavior
		if (Ground != null)
		{
			var acc = Ground.StandAccelerate(this);
			if (stride.Y * Gravity < 0)
			{
				Entity.velocity += acc;
				OnLeave();
			}
			stride += acc;
			Ground = null;
		}
		Entity.position = OldPosition;
		foreach (var result in ColliderManager.Instance.Move(this, stride))
		{
			if (IsGround(result.Normal, Gravity))
			{
				Ground = result.Collider;
				Entity.velocity.Y = 0;
			}
			else if (result.Normal == -Vector2.UnitY * Gravity
				&& result.Collider.Velocity.Y * Gravity <= 0) // Added this condition for preventing player from sticking to box's bottom when box is moving downward.
			{
				Entity.velocity.Y = -CollisionUtils.Epsilon;
			}
			OnCollision(result);
		}
	}

	private static bool IsGround(Vector2 normal, float gravity)
	{
		return Vector2.Dot(normal, gravity * Vector2.UnitY) > Sqrt2Div2;
	}

	public void StepUp(ref Vector2 position, ref Vector2 velocity, int width, int height, ref float stepSpeed, ref float gfxOffY, int gravDir = 1, bool holdsMatching = false, int specialChecksMode = 0)
	{
		int dir = 0;
		if (velocity.X < 0f)
		{
			dir = -1;
		}
		if (velocity.X > 0f)
		{
			dir = 1;
		}
		Vector2 nextPos = position;
		nextPos.X += velocity.X;
		int tilePosFront = (int)((nextPos.X + width / 2 + (width / 2 + 1) * dir) / 16f);
		int tilePosY = (int)((nextPos.Y + 0.1) / 16.0);
		if (gravDir == 1)
		{
			tilePosY = (int)((nextPos.Y + height - 1f) / 16f);
		}
		int heightToTilePos = height / 16 + ((height % 16 != 0) ? 1 : 0);
		bool flag = true;
		bool flag2 = true;
		if (Main.tile[tilePosFront, tilePosY] == null)
		{
			return;
		}
		for (int i = 1; i < heightToTilePos + 2; i++)
		{
			if (!WorldGen.InWorld(tilePosFront, tilePosY - i * gravDir, 0) || Main.tile[tilePosFront, tilePosY - i * gravDir] == null)
			{
				return;
			}
		}
		if (!WorldGen.InWorld(tilePosFront - dir, tilePosY - heightToTilePos * gravDir, 0) || Main.tile[tilePosFront - dir, tilePosY - heightToTilePos * gravDir] == null)
		{
			return;
		}
		Tile tile;
		for (int j = 2; j < heightToTilePos + 1; j++)
		{
			if (!WorldGen.InWorld(tilePosFront, tilePosY - j * gravDir, 0) || Main.tile[tilePosFront, tilePosY - j * gravDir] == null)
			{
				return;
			}
			tile = Main.tile[tilePosFront, tilePosY - j * gravDir];

			// No tile or non-solid tile or solid top tile (platform) is ok
			flag = flag && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]);
		}
		tile = Main.tile[tilePosFront - dir, tilePosY - heightToTilePos * gravDir];

		// No tile or non-solid tile or solid top tile (platform) is ok
		flag2 = flag2 && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]);
		bool flag3 = true;
		bool flag4 = true;
		bool flag5 = true;
		Tile tile2;
		if (gravDir == 1)
		{
			if (Main.tile[tilePosFront, tilePosY - gravDir] == null || Main.tile[tilePosFront, tilePosY - (heightToTilePos + 1) * gravDir] == null)
			{
				return;
			}
			tile = Main.tile[tilePosFront, tilePosY - gravDir];
			tile2 = Main.tile[tilePosFront, tilePosY - (heightToTilePos + 1) * gravDir];

			// No tile or non-solid tile or solid top tile (platform) or sloped tile that doesn't block the path is ok. If it's a half brick, then the tile below it also needs to be non-solid or solid top tile (platform).
			flag3 = flag3 && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] || (tile.slope() == 1 && position.X + width / 2 > tilePosFront * 16) || (tile.slope() == 2 && position.X + width / 2 < tilePosFront * 16 + 16) || (tile.halfBrick() && (!tile2.nactive() || !Main.tileSolid[tile2.type] || Main.tileSolidTop[tile2.type])));
			tile = Main.tile[tilePosFront, tilePosY];
			tile2 = Main.tile[tilePosFront, tilePosY - 1];
			if (specialChecksMode == 1)
			{
				flag5 = !TileID.Sets.IgnoredByNpcStepUp[tile.type];
			}

			// Active tile that is solid and not a solid top tile (platform) is not ok. A sloped tile is not ok if the slope would block the path. If it's a solid top tile (platform) that can be stood on, then the tile below it needs to be non-solid or non-active. If it's a half brick, then it's not ok.
			flag4 = flag4 && ((tile.nactive() && (!tile.topSlope() || (tile.slope() == 1 && position.X + width / 2 < tilePosFront * 16) || (tile.slope() == 2 && position.X + width / 2 > tilePosFront * 16 + 16)) && (!tile.topSlope() || position.Y + height > tilePosY * 16) && ((Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type]) || (holdsMatching && ((Main.tileSolidTop[tile.type] && tile.frameY == 0) || TileID.Sets.Platforms[tile.type]) && (!Main.tileSolid[tile2.type] || !tile2.nactive()) && flag5))) || (tile2.halfBrick() && tile2.nactive()));
			flag4 &= !Main.tileSolidTop[tile.type] || !Main.tileSolidTop[tile2.type];
		}
		else
		{
			tile = Main.tile[tilePosFront, tilePosY - gravDir];
			tile2 = Main.tile[tilePosFront, tilePosY - (heightToTilePos + 1) * gravDir];
			flag3 = flag3 && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] || tile.slope() != 0 || (tile.halfBrick() && (!tile2.nactive() || !Main.tileSolid[tile2.type] || Main.tileSolidTop[tile2.type])));
			tile = Main.tile[tilePosFront, tilePosY];
			tile2 = Main.tile[tilePosFront, tilePosY + 1];
			flag4 = flag4 && ((tile.nactive() && ((Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type]) || (holdsMatching && Main.tileSolidTop[tile.type] && tile.frameY == 0 && (!Main.tileSolid[tile2.type] || !tile2.nactive())))) || (tile2.halfBrick() && tile2.nactive()));
		}
		if (tilePosFront * 16 >= nextPos.X + width || tilePosFront * 16 + 16 <= nextPos.X)
		{
			return;
		}
		if (gravDir == 1)
		{
			if (!flag4 || !flag3 || !flag || !flag2)
			{
				return;
			}
			float tilePosY_World = tilePosY * 16;
			if (Main.tile[tilePosFront, tilePosY - 1].halfBrick())
			{
				tilePosY_World -= 8f;
			}
			else if (Main.tile[tilePosFront, tilePosY].halfBrick())
			{
				tilePosY_World += 8f;
			}
			if (tilePosY_World >= nextPos.Y + height)
			{
				return;
			}
			float MoveUp = nextPos.Y + height - tilePosY_World;
			if ((double)MoveUp <= 16.1)
			{
				gfxOffY += position.Y + height - tilePosY_World;
				position.Y = tilePosY_World - height;
				if (MoveUp < 9f)
				{
					stepSpeed = 1f;
					return;
				}
				stepSpeed = 2f;
				return;
			}
		}
		else
		{
			if (!flag4 || !flag3 || !flag || !flag2 || Main.tile[tilePosFront, tilePosY].bottomSlope() || TileID.Sets.Platforms[tile2.type])
			{
				return;
			}
			float tilePosY_World_1_Below = tilePosY * 16 + 16;
			if (tilePosY_World_1_Below <= nextPos.Y)
			{
				return;
			}
			float MoveUp_Grav = tilePosY_World_1_Below - nextPos.Y;
			if ((double)MoveUp_Grav <= 16.1)
			{
				gfxOffY -= tilePosY_World_1_Below - position.Y;
				position.Y = tilePosY_World_1_Below;
				velocity.Y = 0f;
				if (MoveUp_Grav < 9f)
				{
					stepSpeed = 1f;
					return;
				}
				stepSpeed = 2f;
			}
		}
	}
}