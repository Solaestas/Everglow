using Everglow.Sources.Modules.MythModule.Common;
using Terraria.GameContent.Bestiary;
using Terraria.DataStructures;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.NPCs
{
    // 蜈蚣设定：起始时蜈蚣贴在物块上爬行。受到攻击后转换为钻地（蠕虫形态）

    internal class CentipedeHead : FireWormHead
    {
        public override int BodyType => ModContent.NPCType<CentipedeBody>();
        public override int TailType => ModContent.NPCType<CentipedeTail>();
        public float wormSpeed = 1.0f;
        public int checkHitWidth = 24;
        public Vector2 OldSpeedDirection = new Vector2(1.0f, 0.0f);
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("测试版蜈蚣");

            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = "TheFirefly/NPCs/ExampleCentipede_Bestiary",
                Position = new Vector2(40f, 24f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = 12f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }

        public override void SetDefaults()
        {
            // Head is 10 defence, body 20, tail 30.
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.behindTiles = true;
            NPC.scale = 0.9f;
            NPC.dontCountMe = true;

            NPC.damage = 26;
            NPC.width = 22;
            NPC.height = 22;
            NPC.defense = 30;
            NPC.lifeMax = 200;
            NPC.knockBackResist = 0f;
            NPC.value = 300f;
            NPC.aiStyle = -1;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // 我们可以使用AddRange，而不是多次调用Add，以便一次添加多个项目
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,

                new FlavorTextBestiaryInfoElement("看起来就是条蜈蚣")
            });
        }

        public override void Init()
        {
            MinSegmentLength = 4;
            MaxSegmentLength = 6;
            CommonWormInit(this);
        }

        internal static void CommonWormInit(FireWorm worm)
        {
            // 这两个属性处理蠕虫的运动
            worm.MoveSpeed = 5.5f;
            worm.Acceleration = 0.045f;
        }

        private int attackCounter;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackCounter);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackCounter = reader.ReadInt32();
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (attackCounter > 0)
                {
                    attackCounter--;
                }
                Player target = Main.player[NPC.target];
                attackCounter = 2000;  // 先不让其转成蠕虫AI
                if (attackCounter == 0){
                    // 如果攻击计数器为0，切换成蠕虫AI
                    bool collision = HeadAI_CheckCollisionForDustSpawns();
                    // 测量与目标的距离
                    HeadAI_CheckTargetDistance(ref collision);
                    HeadAI_Movement(collision);

                    NPC.noGravity = true;
                    NPC.noTileCollide = true;
                    attackCounter = 2000;
                }
                // 如果靠近玩家，且头部无碰撞，且计数器大于0.就切换成爬虫AI
                // if (attackCounter > 500 && Vector2.Distance(NPC.Center, target.Center) < 200 && !Collision.SolidCollision(NPC.Center, NPC.width / 2, NPC.height / 2))  //  && Collision.CanHit(NPC.Center, 1, 1, target.Center, 1, 1) 
                else{
                    NPC.noTileCollide = false;
                    // 先检测下右上左四个方向的碰撞
                    Vector2[] hitDirBase = { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0)};
                    int[] isHitDirList = { 0, 0, 0, 0, 0 };
                    for (int i = 0; i < hitDirBase.Length; i++)
                    {
                        Vector2 dirItem = hitDirBase[i];
                        Vector2 newPos = NPC.Center + dirItem * 25;  // TODO 调节碰撞大小
                        if (Collision.SolidCollision(newPos, 1, 1))
                        {
                            isHitDirList[i + 1] = 1;
                            isHitDirList[0] += 1;
                        }
                    }
                    // 检测离玩家多远
                    Player playerTarget = Main.player[NPC.target];
                    Vector2 relatPosit = playerTarget.position-NPC.position;  // 相对位置
                    bool xBigery = true;
                    if(Math.Abs(relatPosit.X) <= Math.Abs(relatPosit.Y)){
                        xBigery = false;
                    }

                    if (isHitDirList[0] == 4)
                    {  // 卡住了，切换蠕虫AI
                        attackCounter = 0;
                        Main.NewText("四个方向发生了碰撞");
                    }
                    else if (isHitDirList[0] == 3)
                    {  // 只有一个方向可走
                        Main.NewText("三个方向发生了碰撞");
                        for (int i = 1; i < isHitDirList.Length; i++)
                        {
                            if (isHitDirList[i] == 0)
                            {
                                NPC.velocity = new Vector2(hitDirBase[i - 1].X, hitDirBase[i - 1].Y) * wormSpeed;
                            }
                        }
                    }
                    else if (isHitDirList[0] == 2 && !(isHitDirList[1] == 1 && isHitDirList[3] == 1) && !(isHitDirList[2] == 1 && isHitDirList[4] == 1))
                    {
                        Main.NewText("两个方向发生了碰撞");
                        // 在两个拐角处发生了碰撞，先设置x和y速度的方向
                        for (int i = 1; i < isHitDirList.Length; i++)
                        {
                            if (isHitDirList[i] == 1)
                            {
                                if (hitDirBase[i - 1].X > 0) NPC.velocity.X = -wormSpeed;
                                else if (hitDirBase[i - 1].X < 0) NPC.velocity.X = wormSpeed;
                                if (hitDirBase[i - 1].Y > 0) NPC.velocity.Y = -wormSpeed;
                                else if (hitDirBase[i - 1].Y < 0) NPC.velocity.Y = wormSpeed;
                                Main.NewText("isHitDirList: " + (i - 1));
                            }
                        }
                        // 按照当前玩家方向为目标
                        if(relatPosit.X * NPC.velocity.X >= 0){
                            NPC.velocity.Y = 0f;
                        }
                        else{
                            if(relatPosit.Y * NPC.velocity.Y > 0 || xBigery) NPC.velocity.X = 0f;
                            else{
                                NPC.velocity.Y = 0f;
                            }
                        }
                        Main.NewText("finally, NPC.velocity, (" + NPC.velocity.X + ", " + NPC.velocity.Y + ")");
                    }
                    else if(isHitDirList[0] == 0){  // 上下左右都没发生碰撞，那检测四个角的碰撞(右下，右上，左上，左下)
                        Vector2[] hitDirAdv = { new Vector2(1, 1), new Vector2(1, -1), new Vector2(-1, -1), new Vector2(-1, 1)};
                        for (int i = 0; i < hitDirAdv.Length; i++)
                        {
                            Vector2 dirItem = hitDirAdv[i];
                            Vector2 newPos = NPC.Center + dirItem * (float)Math.Sqrt(10);  // TODO 调节碰撞大小
                            if (Collision.SolidCollision(newPos, 1, 1))
                            {
                                isHitDirList[i + 1] = 1;
                                isHitDirList[0] += 1;
                            }
                        }
                        if(isHitDirList[0] == 0){  // 真的在悬空
                            // 切换蠕虫AI
                            // attackCounter = 0;
                            // NPC.noGravity = false;
                            NPC.velocity.Y += 0.1f;  // 向下落
                            if(NPC.velocity.Y > 3f) NPC.velocity.Y = 3f;
                            Main.NewText("在悬空");
                        }
                        else if(isHitDirList[0] == 1){  // 只有两个方向可选
                            Main.NewText("有一个对角碰撞");
                            for (int i = 1; i < isHitDirList.Length; i++){
                                if(isHitDirList[i] == 1){  // 先确定可能运动的方向
                                    NPC.velocity.X = hitDirAdv[i-1].X * wormSpeed;
                                    NPC.velocity.Y = hitDirAdv[i-1].Y * wormSpeed;
                                    break;
                                }
                            }
                            // 若Y方向速度远离玩家，则将其设置为0
                            if((playerTarget.position.Y - NPC.position.Y) * NPC.velocity.Y < 0){
                                NPC.velocity.Y = 0;
                            }
                            else{
                                NPC.velocity.X = 0;
                            }
                            Main.NewText("速度：("+NPC.velocity.X+" ,"+NPC.velocity.Y+")");
                        }
                        else if(isHitDirList[0] == 2 && !(isHitDirList[1] == 1 && isHitDirList[3] == 1) && !(isHitDirList[2] == 1 && isHitDirList[4] == 1)){
                            Main.NewText("有两个对角碰撞");
                            // 只有两个碰撞，且两个位置不处于对角线,则有三个可能运动的方向
                            NPC.velocity = new Vector2(0f,0f);
                            for (int i = 1; i < isHitDirList.Length; i++){
                                if(isHitDirList[i] == 1){  // 先确定可能运动的方向
                                    NPC.velocity.X += hitDirAdv[i-1].X;
                                    NPC.velocity.Y += hitDirAdv[i-1].Y;
                                }
                            }
                            // X轴速度同向，可以接近玩家
                            if(relatPosit.X * NPC.velocity.X >= 0){
                                NPC.velocity.X = relatPosit.X / Math.Abs(relatPosit.X) * wormSpeed;
                                NPC.velocity.Y = 0f;
                            }
                            else{  // 如果Y轴可以接近玩家，或者X轴距离玩家距离大于Y轴，则设置Y速度。
                                if(relatPosit.Y * NPC.velocity.Y >= 0 || xBigery){
                                    NPC.velocity.Y = relatPosit.Y / Math.Abs(relatPosit.Y) * wormSpeed;
                                    NPC.velocity.X = 0f;
                                }
                                else{
                                    NPC.velocity.X = relatPosit.X / Math.Abs(relatPosit.X) * wormSpeed;
                                    NPC.velocity.Y = 0f;
                                }
                            }
                        }
                        else{  // 剩余情况相同，有四个可能的运动方向
                            Main.NewText("有多个对角碰撞");
                            if(relatPosit.Y * NPC.velocity.Y >= 0 || xBigery){
                                NPC.velocity.Y = relatPosit.Y / Math.Abs(relatPosit.Y) * wormSpeed;
                                NPC.velocity.X = 0f;
                            }
                            else{
                                NPC.velocity.X = relatPosit.X / Math.Abs(relatPosit.X) * wormSpeed;
                                NPC.velocity.Y = 0f;
                            }
                        }
                    }
                    else
                    {  // 只有一个方向发生了碰撞，则令蠕虫贴着走即可
                        for (int i = 1; i < isHitDirList.Length; i++)
                        {
                            if (isHitDirList[i] == 1)
                            {
                                if (hitDirBase[i - 1].X == 0)
                                {  // 在上或下碰撞，就将Y速度分量设置为0
                                    NPC.velocity.Y = 0;
                                    if (playerTarget.position.X > NPC.position.X) NPC.velocity.X = wormSpeed;
                                    else NPC.velocity.X = -wormSpeed;
                                }
                                else
                                {  // 在左或右发生碰撞，就将X速度分量设置为0
                                    NPC.velocity.X = 0;
                                    if (playerTarget.position.Y > NPC.position.Y) NPC.velocity.Y = wormSpeed;
                                    else NPC.velocity.Y = -wormSpeed;
                                }
                            }
                        }
                    }
                    // 调整头部方向
                    if (NPC.velocity.X != 0) NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + MathHelper.PiOver2;
                    else NPC.rotation = MathHelper.PiOver2 * (NPC.velocity.Y / Math.Abs(NPC.velocity.Y));
                    
                }
                NPC.netUpdate = true;
            }
        }
        private bool HeadAI_CheckCollisionForDustSpawns()
        {
            int minTilePosX = (int)(NPC.Left.X / 16) - 1;
            int maxTilePosX = (int)(NPC.Right.X / 16) + 2;
            int minTilePosY = (int)(NPC.Top.Y / 16) - 1;
            int maxTilePosY = (int)(NPC.Bottom.Y / 16) + 2;

            if (minTilePosX < 0)
                minTilePosX = 0;
            if (maxTilePosX > Main.maxTilesX)
                maxTilePosX = Main.maxTilesX;
            if (minTilePosY < 0)
                minTilePosY = 0;
            if (maxTilePosY > Main.maxTilesY)
                maxTilePosY = Main.maxTilesY;

            bool collision = false;

            // 这是对物体碰撞的初步检查。
            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY; j < maxTilePosY; ++j)
                {
                    Tile tile = Main.tile[i, j];

                    // 如果物体是实心的或被认为是一个平台，那么就有有效的碰撞。
                    if (tile.HasUnactuatedTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType] && tile.TileFrameY == 0) || tile.LiquidAmount > 64)
                    {
                        Vector2 tileWorld = new Point16(i, j).ToWorldCoordinates(0, 0);

                        if (NPC.Right.X > tileWorld.X && NPC.Left.X < tileWorld.X + 16 && NPC.Bottom.Y > tileWorld.Y && NPC.Top.Y < tileWorld.Y + 16)
                        {
                            // 发现碰撞
                            collision = true;

                            if (Main.rand.NextBool(100))
                                WorldGen.KillTile(i, j, fail: true, effectOnly: true, noItem: false);
                        }
                    }
                }
            }
            return collision;
        }

        private void HeadAI_CheckTargetDistance(ref bool collision)
        {
            // 如果没有与tiles发生碰撞，我们会检查这个NPC和它的目标之间的距离是否过大，这样我们仍然可以触发 "碰撞"。
            if (!collision)
            {
                Rectangle hitbox = NPC.Hitbox;

                int maxDistance = MaxDistanceForUsingTileCollision;

                bool tooFar = true;

                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Rectangle areaCheck;

                    Player player = Main.player[i];

                    if (ForcedTargetPosition is Vector2 target)
                        areaCheck = new Rectangle((int)target.X - maxDistance, (int)target.Y - maxDistance, maxDistance * 2, maxDistance * 2);
                    else if (player.active && !player.dead && !player.ghost)
                        areaCheck = new Rectangle((int)player.position.X - maxDistance, (int)player.position.Y - maxDistance, maxDistance * 2, maxDistance * 2);
                    else
                        continue;  // Not a valid player

                    if (hitbox.Intersects(areaCheck))
                    {
                        tooFar = false;
                        break;
                    }
                }

                if (tooFar)
                    collision = true;
            }
        }

        private void HeadAI_Movement(bool collision)
        {
            // 移动速度决定了这个NPC可以移动的最大速度。
            float speed = MoveSpeed;
            // 这个NPC加速的速度。
            float acceleration = Acceleration;

            float targetXPos, targetYPos;

            Player playerTarget = Main.player[NPC.target];

            Vector2 forcedTarget = ForcedTargetPosition ?? playerTarget.Center;
            // 像这样使用一个ValueTuple，可以方便地分配多个值
            (targetXPos, targetYPos) = (forcedTarget.X, forcedTarget.Y);

            // Copy the value, since it will be clobbered later
            Vector2 npcCenter = NPC.Center;

            float targetRoundedPosX = (float)((int)(targetXPos / 16f) * 16);
            float targetRoundedPosY = (float)((int)(targetYPos / 16f) * 16);
            npcCenter.X = (float)((int)(npcCenter.X / 16f) * 16);
            npcCenter.Y = (float)((int)(npcCenter.Y / 16f) * 16);
            // dirX和dirY分别是当前蠕虫到目标的x和y距离
            float dirX = targetRoundedPosX - npcCenter.X;
            float dirY = targetRoundedPosY - npcCenter.Y;
            // 当前蠕虫到目标的直线距离
            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);

            // 如果我们没有任何类型的碰撞，我们希望NPC向下并沿X轴减速。
            if (!collision && !CanFly)
                HeadAI_Movement_HandleFallingFromNoCollision(dirX, speed, acceleration);
            else
            {
                // 否则，我们要播放一些音频（soundDelay）并向我们的目标移动。
                HeadAI_Movement_PlayDigSounds(length);
                // 设置NPC的速度
                HeadAI_Movement_HandleMovement(dirX, dirY, length, speed, acceleration);
            }
            // 设置NPC的转向
            HeadAI_Movement_SetRotation(collision);
        }
        private void HeadAI_Movement_HandleFallingFromNoCollision(float dirX, float speed, float acceleration)
        {
            NPC.TargetClosest(true);
            // Constant gravity of 0.11 pixels/tick
            NPC.velocity.Y += 0.11f;

            // Ensure that the NPC does not fall too quickly
            if (NPC.velocity.Y > speed)
                NPC.velocity.Y = speed;

            // 以下行为模仿了香草虫的运动
            if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.4f)
            {
                // 速度足够快，但不能太快
                if (NPC.velocity.X < 0.0f)
                    NPC.velocity.X -= acceleration * 1.1f;
                else
                    NPC.velocity.X += acceleration * 1.1f;
            }
            else if (NPC.velocity.Y == speed)
            {
                // NPC has reached terminal velocity
                if (NPC.velocity.X < dirX)
                    NPC.velocity.X += acceleration;
                else if (NPC.velocity.X > dirX)
                    NPC.velocity.X -= acceleration;
            }
            else if (NPC.velocity.Y > 4)
            {
                if (NPC.velocity.X < 0)
                    NPC.velocity.X += acceleration * 0.9f;
                else
                    NPC.velocity.X -= acceleration * 0.9f;
            }
        }

        private void HeadAI_Movement_PlayDigSounds(float length)
        {
            if (NPC.soundDelay == 0)
            {
                // NPC离目标地点越近，播放声音越快
                float num1 = length / 40f;

                if (num1 < 10)
                    num1 = 10f;

                if (num1 > 20)
                    num1 = 20f;

                NPC.soundDelay = (int)num1;

                SoundEngine.PlaySound(SoundID.WormDig, NPC.position);
            }
        }

        private void HeadAI_Movement_HandleMovement(float dirX, float dirY, float length, float speed, float acceleration)
        {
            float absDirX = Math.Abs(dirX);
            float absDirY = Math.Abs(dirY);
            float newSpeed = speed / length;  // 距离越近，速度越快
            dirX *= newSpeed;
            dirY *= newSpeed;
            // 蠕虫速度方向和目标方向有相同方向的
            if ((NPC.velocity.X > 0 && dirX > 0) || (NPC.velocity.X < 0 && dirX < 0) || (NPC.velocity.Y > 0 && dirY > 0) || (NPC.velocity.Y < 0 && dirY < 0))
            {
                // 该NPC正在向目标地点移动
                if (NPC.velocity.X < dirX)
                    NPC.velocity.X += acceleration;
                else if (NPC.velocity.X > dirX)
                    NPC.velocity.X -= acceleration;

                if (NPC.velocity.Y < dirY)
                    NPC.velocity.Y += acceleration;
                else if (NPC.velocity.Y > dirY)
                    NPC.velocity.Y -= acceleration;

                // 预定的Y-速度很小，而且NPC正在向左移动，目标在NPC的右边，反之亦然。
                if (Math.Abs(dirY) < speed * 0.2 && ((NPC.velocity.X > 0 && dirX < 0) || (NPC.velocity.X < 0 && dirX > 0)))
                {
                    if (NPC.velocity.Y > 0)
                        NPC.velocity.Y += acceleration * 2f;
                    else
                        NPC.velocity.Y -= acceleration * 2f;
                }
                // 预定的X-速度很小，而且NPC正在向上/向下移动，目标在NPC的下方/上方。
                if (Math.Abs(dirX) < speed * 0.2 && ((NPC.velocity.Y > 0 && dirY < 0) || (NPC.velocity.Y < 0 && dirY > 0)))
                {
                    if (NPC.velocity.X > 0)
                        NPC.velocity.X = NPC.velocity.X + acceleration * 2f;
                    else
                        NPC.velocity.X = NPC.velocity.X - acceleration * 2f;
                }
            }
            else if (absDirX > absDirY)
            {
                // X距离比Y距离大。 迫使沿X轴的运动更强烈
                if (NPC.velocity.X < dirX)
                    NPC.velocity.X += acceleration * 1.1f;
                else if (NPC.velocity.X > dirX)
                    NPC.velocity.X -= acceleration * 1.1f;

                if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.5)
                {
                    if (NPC.velocity.Y > 0)
                        NPC.velocity.Y += acceleration;
                    else
                        NPC.velocity.Y -= acceleration;
                }
            }
            else
            {
                if (NPC.velocity.Y < dirY)
                    NPC.velocity.Y += acceleration * 1.1f;
                else if (NPC.velocity.Y > dirY)
                    NPC.velocity.Y -= acceleration * 1.1f;

                if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.5)
                {
                    if (NPC.velocity.X > 0)
                        NPC.velocity.X += acceleration;
                    else
                        NPC.velocity.X -= acceleration;
                }
            }
        }

        private void HeadAI_Movement_SetRotation(bool collision)
        {
            // Set the correct rotation for this NPC.
            // 假设NPC的贴图是朝上的。 你可能需要修改这一行，以适当考虑到你的NPC的方向。
            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;

            // 一些netupdate的东西（多人游戏兼容性）。
            if (collision)
            {
                if (NPC.localAI[0] != 1)  // 碰撞检测同步
                    NPC.netUpdate = true;

                NPC.localAI[0] = 1f;
            }
            else
            {
                if (NPC.localAI[0] != 0)
                    NPC.netUpdate = true;

                NPC.localAI[0] = 0f;
            }

            // 如果NPC的速度发生变化，并且没有被玩家 "击中"，则强制进行网络更新。
            if (((NPC.velocity.X > 0 && NPC.oldVelocity.X < 0) || (NPC.velocity.X < 0 && NPC.oldVelocity.X > 0) || (NPC.velocity.Y > 0 && NPC.oldVelocity.Y < 0) || (NPC.velocity.Y < 0 && NPC.oldVelocity.Y > 0)) && !NPC.justHit)
                NPC.netUpdate = true;
        }

    }

    internal class CentipedeBody : FireWormBody
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("测试版蜈蚣");

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                // 将此NPC从Bestiary中隐藏起来，对于你只想要一个条目的多部分NPC很有用。
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.behindTiles = true;
            NPC.scale = 0.9f;
            NPC.dontCountMe = true;

            NPC.damage = 26;
            NPC.width = 24;
            NPC.height = 24;
            NPC.defense = 30;
            NPC.lifeMax = 200;
            NPC.knockBackResist = 0f;
            NPC.value = 300f;
            NPC.aiStyle = -1;
        }

        public override void Init()
        {
            CentipedeHead.CommonWormInit(this);
        }
    }

    internal class CentipedeTail : FireWormTail
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("测试版蜈蚣");

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.behindTiles = true;
            NPC.scale = 0.9f;
            NPC.dontCountMe = true;

            NPC.damage = 26;
            NPC.width = 24;
            NPC.height = 24;
            NPC.defense = 30;
            NPC.lifeMax = 200;
            NPC.knockBackResist = 0f;
            NPC.value = 300f;
            NPC.aiStyle = -1;
        }

        public override void Init()
        {
            CentipedeHead.CommonWormInit(this);
        }
    }
}