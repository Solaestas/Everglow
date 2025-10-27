using System.Diagnostics;
using Everglow.Commons.Physics.MassSpringSystem;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader.IO;

namespace Everglow.UnitTests;

[TestClass]
public class UnitTest
{
	public TestContext TestContext { get; set; } = default!;

	[TestInitialize]
	public void Initialize()
	{
		// Prevent TypeInitializer throwing exception
		Program.SavePath = string.Empty;
		Main.player[0] = new Player();
	}

	[TestMethod]
	public void RopeTest()
	{
		var rope = Rope.Create(
			new Vector2(0, 0),
			new Vector2(10, 0),
			11,
			0.1f, 1f);
		Assert.AreEqual(rope.Masses.Length, 11);
		for (int i = 0; i < rope.Masses.Length; i++)
		{
			var mass = rope.Masses[i];
			Assert.AreEqual(i, mass.Position.X);
		}

		var system = new MassSpringSystem();
		system.AddMassSpringMesh(rope);

		var solver = new EulerSolver(10);
		Enumerable.Repeat(0, 60).ToList().ForEach(_ =>
		{
			rope.ApplyForce();
			solver.Step(system, 1f);
		});

		TestContext.WriteLine(string.Join(", ", from mass in rope.Masses select mass.Position.Y));
	}

	[TestMethod]
	public void TagCompoundExceptionTest()
	{
		var keyStringList = "testList";
		var keyTagList = "testList2";
		var baseTag = new TagCompound
		{
			{ keyStringList, new List<string> { "a", "b", "c" } },
			{ keyTagList, new List<TagCompound> { new(), new() } },
		};

		Assert.ThrowsExactly<IOException>(() =>
		{
			var list = baseTag.GetCompound(keyStringList);
		});

		Assert.ThrowsExactly<IOException>(() =>
		{
			var list = baseTag.GetCompound(keyTagList);
		});

		TagCompound? value1 = null;
		TagCompound? value2 = null;

		Assert.IsNull(value1);
		Assert.IsNull(value2);

		try
		{
			value1 = baseTag.GetCompound(keyStringList);
			value2 = baseTag.GetCompound(keyTagList);
		}
		catch (IOException)
		{
			TestContext.WriteLine("Caught IOException as expected.");
		}
		finally
		{
			value1 = new TagCompound();
			value2 = new TagCompound();
			TestContext.WriteLine("Test completed.");
		}

		Assert.IsNotNull(value1);
		Assert.IsNotNull(value2);
	}
}