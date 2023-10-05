using System.Reflection;
using System.Text;
using Mono.Cecil;

namespace Everglow.Commons;

public abstract class Wrapper
{
	public static Wrapper Create(Type type) => new TypeWrapper(type);

	public static Wrapper Create(TypeReference type) => new TypeReferenceWrapper(type);

	public static Wrapper Create(ParameterInfo info) => new ParameterWrapper(info);

	public static Wrapper Create(MethodInfo info) => new MethodInfoWrapper(info);

	public static Wrapper Create(ConstructorInfo info) => new ConstructorInfoWrapper(info);

	public static Wrapper Create(MethodReference type) => new MethodReferenceWrapper(type);

	public abstract string Identity { get; }

	public override string ToString()
	{
		return Identity;
	}

	public static implicit operator string(Wrapper wrapper)
	{
		return wrapper.Identity;
	}

	protected string Escape(string val)
	{
		return val.Replace('<', '[').Replace('>', ']');
	}
}

internal class TypeWrapper : Wrapper
{
	public TypeWrapper(Type type)
	{
		if (type.IsGenericType)
		{
			_identity = $"{type.Name[..^2]}<{string.Join(',', type.GetGenericArguments().Select(Create))}>";
		}
		else
		{
			_identity = type.Name;
		}
	}

	private string _identity;

	public override string Identity => _identity;
}

internal class TypeReferenceWrapper : Wrapper
{
	public TypeReferenceWrapper(TypeReference type)
	{
		if (type.IsGenericInstance)
		{
			var info = type.GetType().GetProperty("GenericArguments")!;
			var args = (Mono.Collections.Generic.Collection<TypeReference>)info.GetValue(type)!;
			_identity = $"{type.Name[..^2]}<{string.Join(',', args.Select(Create))}>";
		}
		else
		{
			_identity = type.Name;
		}
	}

	private string _identity;

	public override string Identity => _identity;
}

internal class ParameterWrapper(ParameterInfo info) : Wrapper
{
	private string _identity = Create(info.ParameterType);

	public override string Identity => _identity;
}

internal class MethodInfoWrapper : Wrapper
{
	private string _identity;

	public MethodInfoWrapper(MethodInfo method)
	{
		var sb = new StringBuilder();
		sb.Append(Create(method.ReturnType!))
			.Append(' ')
			.Append(Create(method.DeclaringType!))
			.Append(".")
			.Append(method.Name);

		if (method.IsGenericMethod)
		{
			sb.Append('<')
				.AppendJoin(',', method.GetGenericArguments().Select(Create))
				.Append('>');
		}
		sb.Append('(')
			.AppendJoin(',', method.GetParameters().Select(Create))
			.Append(')');

		_identity = Escape(sb.ToString());
	}

	public override string Identity => _identity;
}

internal class ConstructorInfoWrapper : Wrapper
{
	private string _identity;

	public ConstructorInfoWrapper(ConstructorInfo method)
	{
		var sb = new StringBuilder();
		sb.Append(Create(method.DeclaringType!))
			.Append(".")
			.Append(method.Name);

		if (method.IsGenericMethod)
		{
			sb.Append('<')
				.AppendJoin(',', method.GetGenericArguments().Select(Create))
				.Append('>');
		}
		sb.Append('(')
			.AppendJoin(',', method.GetParameters().Select(Create))
			.Append(')');

		_identity = Escape(sb.ToString());
	}

	public override string Identity => _identity;
}

internal class MethodReferenceWrapper : Wrapper
{
	private string _identity;

	public MethodReferenceWrapper(MethodReference method)
	{
		var sb = new StringBuilder();
		sb.Append(Create(method.ReturnType!))
			.Append(' ')
			.Append(Create(method.DeclaringType!))
			.Append(".")
			.Append(method.Name);

		if (method.HasGenericParameters)
		{
			sb.Append('<')
				.AppendJoin(',', method.GenericParameters.Select(Create))
				.Append('>');
		}
		sb.Append('(')
			.AppendJoin(',', method.Parameters.Select(s => Create(s.ParameterType)))
			.Append(')');

		_identity = Escape(sb.ToString());
	}

	public override string Identity => _identity;
}