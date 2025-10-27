namespace Everglow.Scripts;

public abstract class Completer
{
	public abstract IEnumerable<string> Candidates { get; }

	public abstract int Count { get; }

	public abstract void Evaluate(string part);

	public abstract void Print(int index);

	public abstract string Get(int index);
}

public class ScriptCompleter : Completer
{
	public static Script[] Scripts { get; private set; }

	private Script[] _candidates;

	public override IEnumerable<string> Candidates => _candidates.Select(s => s.Name);

	public override int Count => _candidates.Length;

	static ScriptCompleter()
	{
		Scripts = typeof(ScriptCompleter).Assembly.GetTypes().Where(s => s.IsAssignableTo(typeof(Script))).Where(s => !s.IsAbstract).Select(s => Activator.CreateInstance(s)).OfType<Script>().ToArray();
	}

	public ScriptCompleter()
	{
		_candidates = Array.Empty<Script>();
	}

	public override void Evaluate(string part)
	{
		_candidates = Scripts.Where(s => s.Name.StartsWith(part.ToString(), StringComparison.OrdinalIgnoreCase)).ToArray();
	}

	public override string Get(int index)
	{
		return _candidates[index].Name;
	}

	public override void Print(int index)
	{
		if (_candidates.Length == 0)
		{
			return;
		}
		var choose = _candidates.ElementAtOrDefault(index);

		var (Left, _) = Console.GetCursorPosition();
		Console.Write(new string('\n', _candidates.Length + 2));
		var (_, Top) = Console.GetCursorPosition();
		Top -= _candidates.Length + 2;
		SetCursorPosition(Left, Top + 1);
		int maxLength = _candidates.Max(s => s.Name.Length);
		ChangeColor();
		Console.Write('┌');
		Console.Write(new string('─', maxLength + 2));
		Console.Write('┐');

		for (int i = 0; i < _candidates.Length; i++)
		{
			Script? candidate = _candidates[i];
			SetCursorPosition(Left, true);
			ChangeColor();
			Console.Write('│');

			ChangeColor(ConsoleColor.Blue);

			if (i == index)
			{
				Console.Write('>');
				ChangeColor(ConsoleColor.Yellow, ConsoleColor.DarkGray);
			}
			else
			{
				Console.Write(' ');
				ChangeColor(ConsoleColor.Yellow);
			}
			Console.Write(candidate.Name);

			ChangeColor();
			Console.Write(new string(' ', maxLength - candidate.Name.Length + 1));
			Console.Write('│');
		}
		SetCursorPosition(Left, true);
		Console.Write('└');
		Console.Write(new string('─', maxLength + 2));
		Console.Write(_candidates.Length == 1 && index != -1 ? '┴' : '┘');

		if (choose != null)
		{
			SetCursorPosition(Left + maxLength + 3, Top + 1);
			Console.Write('┬');
			Console.Write(new string('─', choose.Description.Length));
			Console.Write('┐');

			SetCursorPosition(Left + maxLength + 3, true);
			Console.Write('│');
			Console.Write(choose.Description);
			Console.Write('│');

			SetCursorPosition(Left + maxLength + 3, true);
			Console.Write(_candidates.Length == 1 ? '┴' : '├');
			Console.Write(new string('─', choose.Description.Length));
			Console.Write('┘');
		}
		Console.SetCursorPosition(Left, Top);
	}
}

public class ModuleCompleter : Completer
{
	private string[] _candidates = Array.Empty<string>();

	public override IEnumerable<string> Candidates => _candidates;

	public override int Count => _candidates.Length;

	public override void Evaluate(string part)
	{
		_candidates = Module.Modules.Where(s => s.StartsWith(part.ToString(), StringComparison.OrdinalIgnoreCase)).ToArray();
	}

	public override string Get(int index)
	{
		return _candidates[index];
	}

	public override void Print(int index)
	{
		if (_candidates.Length == 0)
		{
			return;
		}
		var choose = _candidates.ElementAtOrDefault(index);

		var (Left, _) = Console.GetCursorPosition();
		Console.Write(new string('\n', _candidates.Length + 2));
		var (_, Top) = Console.GetCursorPosition();
		Top -= _candidates.Length + 2;
		SetCursorPosition(Left, Top + 1);
		int maxLength = _candidates.Max(s => s.Length);
		ChangeColor();
		Console.Write('┌');
		Console.Write(new string('─', maxLength + 2));
		Console.Write('┐');

		for (int i = 0; i < _candidates.Length; i++)
		{
			var candidate = _candidates[i];
			SetCursorPosition(Left, true);
			ChangeColor();
			Console.Write('│');

			ChangeColor(ConsoleColor.Blue);

			if (i == index)
			{
				Console.Write('>');
				ChangeColor(ConsoleColor.Yellow, ConsoleColor.DarkGray);
			}
			else
			{
				Console.Write(' ');
				ChangeColor(ConsoleColor.Yellow);
			}
			Console.Write(candidate);

			ChangeColor();
			Console.Write(new string(' ', maxLength - candidate.Length + 1));
			Console.Write('│');
		}
		SetCursorPosition(Left, true);
		Console.Write('└');
		Console.Write(new string('─', maxLength + 2));
		Console.Write('┘');

		Console.SetCursorPosition(Left, Top);
	}
}

public class NoCompleter : Completer
{
	public override IEnumerable<string> Candidates => Enumerable.Empty<string>();

	public override int Count => 0;

	public override void Evaluate(string part)
	{ }

	public override string Get(int index) => throw new Exception("No Completion here");

	public override void Print(int index)
	{ }
}