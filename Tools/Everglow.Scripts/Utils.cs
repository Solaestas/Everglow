using System.Text;

namespace Everglow.Scripts;

public static class Utils
{
	public static void ChangeColor(ConsoleColor foregroudColor = ConsoleColor.White, ConsoleColor backgroudColor = ConsoleColor.Black)
	{
		Console.ForegroundColor = foregroudColor;
		Console.BackgroundColor = backgroudColor;
	}

	public static void Clear(int top)
	{
		if(top == Console.WindowHeight)
		{
			return;
		}

		ChangeColor();
		StringBuilder cls = new StringBuilder();
		for (int i = top; i < Console.WindowHeight; i++)
		{
			cls.AppendLine();
			cls.Append(' ', Console.WindowWidth);
		}
		Console.SetCursorPosition(0, top - 1);
		Console.Write(cls.ToString());
		Console.SetCursorPosition(0, top);
	}

	public static void SetCursorPosition(int left, bool newline = false)
	{
		if (newline)
		{
			Console.WriteLine();
		}
		Console.SetCursorPosition(left, Console.GetCursorPosition().Top);
	}

	public static void SetCursorPosition(int left, int top)
	{
		var currentTop = Console.GetCursorPosition().Top;
		if (top <= currentTop)
		{
			Console.SetCursorPosition(left, top);
		}
		else
		{
			Console.Write(new string('\n', top - currentTop));
			Console.SetCursorPosition(left, Console.GetCursorPosition().Top);
		}
	}
}