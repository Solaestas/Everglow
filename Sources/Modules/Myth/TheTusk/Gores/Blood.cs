namespace Everglow.Myth.TheTusk.Gores
{
	public class Blood : LiquidGore
	{
		//TODO:这里我不太会翻译成VFX,yiyang做獠牙的时候需要的话可以翻译一下
		public override string EffectPath()
		{
			return "Effects/BloodDrop";
		}
		public override bool Update(Gore gore)
		{
			gore.velocity.Y += 0.2f;
			gore.velocity *= 0.95f;
			gore.rotation += Main.rand.NextFloat(-0.05f, 0.05f);
			gore.timeLeft -= 8;
			if (Collision.SolidCollision(gore.position, (int)gore.Width, (int)gore.Height))
				gore.timeLeft -= 150;
			return base.Update(gore);
		}
	}
}
