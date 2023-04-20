using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons;
public class ShackInfo
{
	internal Vector2 center;
	internal Vector2 dir;
	internal float strength;
	internal float period;
	internal float propagationSpeed;
	internal float ac_vibration;
	internal float ac_propagation;
	internal int tickTimer;
	internal int maxTick;
	internal int propagationDelayTimer;
	internal int maxPropagationTime;
	public virtual bool Update()
	{
		tickTimer++;
		if (strength * Math.Pow(ac_vibration, tickTimer) < 0.01f || (maxTick > 0 && tickTimer > maxTick))
		{
			propagationDelayTimer++;
			if (propagationDelayTimer >= maxPropagationTime)
			{
				return false;
			}
		}
		return true;
	}
	public virtual void ApplyTo(Vector2 pos,ref Vector2 effect)
	{
		int tracetime = (int)(Vector2.Distance(center, pos) / propagationSpeed);
		if (tickTimer < tracetime)
		{
			return;
		}
		int time = tickTimer - tracetime;
		if (maxTick != 1 && time > maxTick)
		{
			return;
		}
		float amplitude = (float)Math.Sin(period * time);
		Vector2 vdir = Vector2.Normalize(dir);
		float vStrength = strength * amplitude * (float)Math.Pow(ac_vibration, time);
		float attenuation = (float)Math.Pow(ac_propagation, tracetime);
		float attenuatedStrength = vStrength * attenuation;
		effect += attenuatedStrength * vdir;
	}
}
public class UndirectedShackInfo : ShackInfo
{
	public static UndirectedShackInfo Create(Vector2 center,float strength = 1, float period = 1, float speed = 1, float acv = 0.9f, float acp = 0.9f, int maxtick = -1)
	{
		if (period < 0)
		{
			throw new ArgumentException("不合法的物理参数.振动周期不可能为负数.");
		}
		if (speed <= 0)
		{
			throw new ArgumentException("不合法的物理参数.传播速度不可能为负数或0.这是一个标量速度!");
		}
		if (acv <= 0)
		{
			throw new ArgumentException("不合法的物理参数.强度衰减不可能为负数或0.这样的震动不可能存在!");
		}
		if (acp <= 0)
		{
			throw new ArgumentException("不合法的物理参数.传播衰减不可能为负数或0.这样的震动不可能存在!");
		}
		return new UndirectedShackInfo()
		{
			center = center,
			strength = strength,
			period = period,
			propagationSpeed = speed,
			ac_vibration = acv,
			ac_propagation = acp,
			maxPropagationTime = (int)Math.Floor(1 / acp / speed) + 1,
			maxTick = maxtick
		};
	}
	public override void ApplyTo(Vector2 pos, ref Vector2 effect)
	{
		int tracetime = (int)(Vector2.Distance(center, pos) / propagationSpeed);
		if (tickTimer < tracetime)
		{
			return;
		}
		int time = tickTimer - tracetime;
		if (maxTick != 1 && time > maxTick)
		{
			return;
		}
		float amplitude = (float)Math.Sin(period * time);
		Vector2 vdir = Vector2.Normalize(pos - center);
		float vStrength = strength * amplitude * (float)Math.Pow(ac_vibration, time);
		float attenuation = (float)Math.Pow(ac_propagation, tracetime);
		float attenuatedStrength = vStrength * attenuation;
		effect += attenuatedStrength * vdir;
	}
}
public class ShackManager : ModSystem
{
	static List<ShackInfo> shacks;
	static List<ShackInfo> waitremove;
	public override void Load()
	{
		shacks = new();
		waitremove = new();
	}
	public override void Unload()
	{
		shacks.Clear();
		shacks = null;
		waitremove.Clear();
		waitremove = null;
	}
	public override void PostUpdateEverything() => Update();
	public override void ModifyScreenPosition()
	{
		Main.screenPosition += GetEffectOn(Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2);
	}
	public static void AddShack(Vector2 center, Vector2 dir, float strength = 1, float period = 1, float speed = 1, float acv = 0.9f, float acp = 0.9f, int maxtick = -1)
	{
		if (period < 0)
		{
			throw new ArgumentException("不合法的物理参数.振动周期不可能为负数.");
		}
		if (speed <= 0)
		{
			throw new ArgumentException("不合法的物理参数.传播速度不可能为负数或0.这是一个标量速度!");
		}
		if (acv <= 0)
		{
			throw new ArgumentException("不合法的物理参数.强度衰减不可能为负数或0.这样的震动不可能存在!");
		}
		if (acp <= 0)
		{
			throw new ArgumentException("不合法的物理参数.传播衰减不可能为负数或0.这样的震动不可能存在!");
		}
		shacks.Add(new()
		{
			center = center,
			dir = dir,
			strength = strength,
			period = period,
			propagationSpeed = speed,
			ac_vibration = acv,
			ac_propagation = acp,
			maxPropagationTime = (int)Math.Floor(1 / acp / speed) + 1,
			maxTick = maxtick
		});
	}
	public static void AddShack(ShackInfo info)
	{
		shacks.Add(info);
	}
	public static void Clear() => shacks.Clear();
	static void Update()
	{
		waitremove.Clear();
		foreach (ShackInfo info in shacks)
		{
			if (!info.Update())
			{
				waitremove.Add(info);
			}
		}
		shacks.RemoveAll(waitremove.Contains);
	}
	public static Vector2 GetEffectOn(Vector2 pos)
	{
		Vector2 effect = Vector2.Zero;
		foreach (ShackInfo info in shacks)
		{
			info.ApplyTo(pos, ref effect);
		}
		return effect;
	}
}