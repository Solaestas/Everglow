using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Commons.Mechanics.MissionSystem.Core;

namespace Everglow.Commons.Mechanics.MissionSystem.Tests;

public class MissionTimerTest : MissionBase
{
	public override string DisplayName => GetType().Name;

	public override long TimeMax => 18000;
}