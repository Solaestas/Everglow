using System.Runtime.InteropServices;
using System.Text;
using Everglow.Scripts;

Console.Clear();
ChangeColor();
Console.WriteLine(
"""
==============================
Everglow Script
Type 'help' for help
Press <Tab> for auto-completion
==============================
""");

List<string> sentence = new();
StringBuilder word = new();
string Prompt = "> ";
bool enableCompletion = false;
Completer completer = new ScriptCompleter();

// Command Cycle
while (true)
{
	word.Clear();
	sentence.Clear();

	// Input Cycle
	int completionIndex = 0;
	ChangeColor();
	Console.Write(Prompt);
	while (true)
	{
		if (sentence.Count == 0)
		{
			if (completer is not ScriptCompleter)
			{
				completer = new ScriptCompleter();
			}
		}
		else if (sentence[0].Contains("Module"))
		{
			if (completer is not ModuleCompleter)
			{
				completer = new ModuleCompleter();
			}
		}
		else
		{
			if (completer is not NoCompleter)
			{
				completer = new NoCompleter();
			}
		}
		var key = Console.ReadKey(true);
		if (key.Key == ConsoleKey.Enter)
		{
			if (enableCompletion && completer.Count != 0 &&  0 <= completionIndex && completionIndex < completer.Count)
			{
				sentence.Add(completer.Get(completionIndex));
				word.Clear();
				enableCompletion = false;
			}
			else
			{
				sentence.Add(word.ToString());
				word.Clear();
				Console.WriteLine();
				Clear(Console.GetCursorPosition().Top);
				break;
			}
		}
		if (key.Key == ConsoleKey.Backspace)
		{
			if (word.Length != 0)
			{
				word.Remove(word.Length - 1, 1);
			}
			else if (sentence.Count != 0)
			{
				word.Append(sentence[^1]);
				sentence.RemoveAt(sentence.Count - 1);
			}
		}
		else if (key.Key == ConsoleKey.Tab)
		{
			if (enableCompletion && completer.Count != 0)
			{
				sentence.Add(0 <= completionIndex && completionIndex < completer.Count ?
					completer.Get(completionIndex) :
					completer.Get(0));
				word.Clear();
				enableCompletion = false;
			}
		}
		else if (char.IsLetter(key.KeyChar))
		{
			word.Append(key.KeyChar);
			completionIndex = 0;
			enableCompletion = true;
		}
		else if (key.Key == ConsoleKey.Spacebar)
		{
			sentence.Add(word.ToString());
			word.Clear();
		}
		else if (key.Key == ConsoleKey.UpArrow)
		{
			if (completionIndex == -1)
			{
				completionIndex = completer.Count;
			}
			completionIndex--;
		}
		else if (key.Key == ConsoleKey.DownArrow)
		{
			completionIndex++;
			if (completionIndex == completer.Count)
			{
				completionIndex = -1;
			}
		}
		completer.Evaluate(word.ToString());

		Clear(Console.GetCursorPosition().Top);
		Console.Write(Prompt);
		ChangeColor(ConsoleColor.Yellow);
		foreach (var item in sentence)
		{
			Console.Write(item);
			Console.Write(' ');
		}
		Console.Write(word);

		if (enableCompletion)
		{
			completer.Print(completionIndex);
		}
	}

	sentence.RemoveAll(s => s == string.Empty);

	if (sentence.Count == 0)
	{
		continue;
	}

	var s = ScriptCompleter.Scripts.FirstOrDefault(s => string.Equals(s.Name, sentence[0], StringComparison.OrdinalIgnoreCase));
	if (s is null)
	{
		ChangeColor(ConsoleColor.Red);
		Console.WriteLine("Script name that does not exist");
		continue;
	}

	s.Run(CollectionsMarshal.AsSpan(sentence)[1..].ToArray());

	if(s is Exit)
	{
		return;
	}
}