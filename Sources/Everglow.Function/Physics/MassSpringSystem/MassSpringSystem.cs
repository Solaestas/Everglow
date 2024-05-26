using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Commons.VFX.Visuals;

namespace Everglow.Commons.Physics.MassSpringSystem;

public class _Mass
{
	public bool IsStatic;
	public float Mass;
	public Vector2 Position;
	public Vector2 Velocity;
	public Vector2 Force;
	public Vector2 PrevPosition;

	public _Mass()
	{
		this.Mass = 1f;
		this.Velocity = Vector2.Zero;
		this.Force = Vector2.Zero;
		this.Position = Vector2.Zero;
		this.PrevPosition = Vector2.Zero;
		this.IsStatic = false;
	}
}

public class ElasticConstrain
{
	public _Mass A;
	public _Mass B;
	public float RestLength;
	public float Stiffness;
	public float LambdaPrev;

	public ElasticConstrain(_Mass mass1, _Mass mass2, float restLength, float stiffness)
	{
		A = mass1;
		B = mass2;
		RestLength = restLength;
		Stiffness = stiffness;
		LambdaPrev = 0.0f;
	}
}

public class MassSpringSystem
{
	public List<_Mass> Masses 
	{
		get;
	}

	public List<ElasticConstrain> Springs
	{
		get;
	}


	public MassSpringSystem()
	{
	}


}
