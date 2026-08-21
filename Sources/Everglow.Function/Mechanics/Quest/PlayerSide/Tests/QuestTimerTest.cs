using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Commons.Mechanics.Quest.PlayerSide.Abstractions;

namespace Everglow.Commons.Mechanics.Quest.PlayerSide.Tests;

public class QuestTimerTest : PlayerQuestBase
{
	public override string DisplayName => GetType().Name;

	public override int TimeLimit => 18000;
}
