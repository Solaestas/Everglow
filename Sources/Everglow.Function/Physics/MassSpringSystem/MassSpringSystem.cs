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

	public Vector2 OldPos;
	public Vector2 DeltaPos;
	public float HessianDiag;
	public _Mass(float mass, Vector2 position, bool isStatic)
	{
		this.Mass = mass;
		this.Velocity = Vector2.Zero;
		this.Force = Vector2.Zero;
		this.Position = position;
		this.IsStatic = isStatic;

		this.OldPos = Vector2.Zero;
		this.DeltaPos = Vector2.Zero;
		this.HessianDiag = 0;
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
		LambdaPrev = 0;
	}
}

public class MassSpringSystem
{
	public List<_Mass> Masses { get; } = new List<_Mass>();

	public List<ElasticConstrain> Springs { get; } = new List<ElasticConstrain>();

	public float Damping { get; set; }

	public MassSpringSystem()
	{
	}

	public void AddMassSpringMesh(IMassSpringMesh mesh)
	{
		Masses.AddRange(mesh.GetMasses());
		Springs.AddRange(mesh.GetElasticConstrains());
	}
}