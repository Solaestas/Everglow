namespace Everglow.Commons.MissionSystem;

public class MissionIconGroup
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MissionIconGroup"/> class.
	/// </summary>
	public MissionIconGroup(List<MissionIconBase> missionIcons)
	{
		_iconDetails.AddRange(missionIcons);
	}

	private readonly List<MissionIconBase> _iconDetails = [];
	private int _currentIndex;

	/// <summary>
	/// Gets the current icon.
	/// </summary>
	public MissionIconBase CurrentIcon
	{
		get
		{
			if (_iconDetails.Count == 0)
			{
				throw new InvalidOperationException("No icons are available in the group.");
			}

			if (_currentIndex < 0 || _currentIndex >= _iconDetails.Count)
			{
				throw new InvalidOperationException("Current index is out of bounds.");
			}

			return _iconDetails[_currentIndex];
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="MissionIconGroup"/> class with a list of icons.
	/// </summary>
	/// <param name="icons">The initial list of icons.</param>
	public MissionIconGroup(IEnumerable<MissionIconBase> icons)
	{
		ArgumentNullException.ThrowIfNull(icons);

		_iconDetails.AddRange(icons);
	}

	/// <summary>
	/// Adds an icon to the group.
	/// </summary>
	/// <param name="icon">The icon to add.</param>
	public void AddIcon(MissionIconBase icon)
	{
		ArgumentNullException.ThrowIfNull(icon);

		_iconDetails.Add(icon);
	}

	/// <summary>
	/// Removes an icon from the group.
	/// </summary>
	/// <param name="icon">The icon to remove.</param>
	/// <returns>True if the icon was removed; otherwise, false.</returns>
	public bool RemoveIcon(MissionIconBase icon)
	{
		ArgumentNullException.ThrowIfNull(icon);

		return _iconDetails.Remove(icon);
	}

	/// <summary>
	/// Draws the current icon at the specified position.
	/// </summary>
	/// <param name="position">The position to draw the icon.</param>
	public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
	{
		CurrentIcon.Draw(spriteBatch, destinationRectangle);
	}

	/// <summary>
	/// Moves to the previous icon in the group.
	/// </summary>
	public void Prev()
	{
		if (_iconDetails.Count == 0)
		{
			throw new InvalidOperationException("No icons are available in the group.");
		}

		_currentIndex = (_currentIndex - 1 + _iconDetails.Count) % _iconDetails.Count;
	}

	/// <summary>
	/// Moves to the next icon in the group.
	/// </summary>
	public void Next()
	{
		if (_iconDetails.Count == 0)
		{
			throw new InvalidOperationException("No icons are available in the group.");
		}

		_currentIndex = (_currentIndex + 1) % _iconDetails.Count;
	}

	/// <summary>
	/// Gets the number of icons in the group.
	/// </summary>
	public int IconCount => _iconDetails.Count;

	public bool IsLastIcon => _currentIndex == _iconDetails.Count - 1;

	public bool IsFirstIcon => _currentIndex == 0;
}