using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.Geometry;
internal class Transform
{
	Vector3 _position;
	Quaternion _rotation;
	Vector3 _scale;
	List<MeshFilter> _meshFilters;
	bool _changed;
	public Vector3 Position
	{
		get => _position;
		set
		{
			_changed |= _position == value;
			_position = value;
		}
	}
	public Quaternion Rotation
	{
		get => _rotation;
		set
		{
			_changed |= _rotation == value;
			_rotation = value;
		}
	}
	public Vector3 Scale 
	{
        get => _scale; 
		set
        {
            _changed |= _scale == value;
            _scale = value;
        }
    }
	public void CheckChange()
	{
		if(_changed)
		{
			_meshFilters.ForEach(filter => filter.Mesh.ApplyTransform(this));
			_changed = false;
		}
	}
	public void ApplyTo(Mesh mesh)
	{
		mesh.ApplyTransform(this);
	}
}
