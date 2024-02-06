using Godot;
using YAT.Classes;
using YAT.Enums;
using YAT.Helpers;

namespace YAT.Scenes.BaseTerminal;

public partial class SelectedNode : Node
{
	[Signal]
	public delegate void CurrentNodeChangedEventHandler(Node node);
	[Signal]
	public delegate void CurrentNodeChangeFailedEventHandler(RejectionReason reason);
	[Signal]
	public delegate void MethodCalledEventHandler(string method, Variant returnValue, MethodStatus status);

	public Node Current { get; private set; }

	public enum RejectionReason
	{
		InvalidNode,
		InvalidNodePath
	}

	public enum MethodStatus
	{
		Success,
		Failed
	}

	private BaseTerminal _terminal;

	public override void _Ready()
	{
		_terminal = GetNode<BaseTerminal>("../");
		Current = GetTree().Root;
	}

	/// <summary>
	/// Changes the selected node to the specified node path.
	/// </summary>
	/// <param name="node">The node path of the new selected node.</param>
	/// <returns>True if the selected node was successfully changed, false otherwise.</returns>
	public bool ChangeSelectedNode(NodePath node)
	{
		if (node is null || node.IsEmpty)
		{
			EmitSignal(SignalName.CurrentNodeChangeFailed, (ushort)RejectionReason.InvalidNodePath);
			return false;
		}

		Node newSelectedNode;
		if (IsInstanceValid(Current))
		{
			newSelectedNode = Current.GetNodeOrNull(node);
			newSelectedNode ??= GetNodeOrNull(node);
		}
		else newSelectedNode = GetNodeOrNull(node);

		if (!IsInstanceValid(newSelectedNode))
		{
			EmitSignal(SignalName.CurrentNodeChangeFailed, (ushort)RejectionReason.InvalidNode);
			return false;
		}

		Current = newSelectedNode;
		EmitSignal(SignalName.CurrentNodeChanged, Current);

		return true;
	}

	public bool CallMethods(string input)
	{
		string[] methods = Text.SplitClean(input, ".");

		if (methods.Length == 0) return false;

		if (methods.Length == 1)
		{
			var (name, args) = Parser.ParseMethod(methods[0]);

			if (args.Length == 0
				? !CallMethod(Current, name, out var result)
				: !CallMethod(Current, name, out result, args)) return false;

			_terminal.Print(result.ToString());
		}
		else if (!MethodChaining(methods)) return false;

		return true;
	}

	private bool CallMethod(Node node, string method, out Variant result, params Variant[] args)
	{
		result = new();
		var validationResult = node.ValidateMethod(method);

		switch (validationResult)
		{
			case MethodValidationResult.InvalidInstance:
				_terminal.Output.Error(Messages.DisposedNode);
				EmitSignal(SignalName.MethodCalled, method, result, (ushort)MethodStatus.Failed);
				return false;
			case MethodValidationResult.InvalidMethod:
				_terminal.Output.Error(Messages.InvalidMethod(method));
				EmitSignal(SignalName.MethodCalled, method, result, (ushort)MethodStatus.Failed);
				return false;
		}

		result = args.Length == 0
			? node.CallMethod(method)
			: node.CallMethod(method, args);

		EmitSignal(SignalName.MethodCalled, method, result, (ushort)MethodStatus.Success);

		return true;
	}

	private bool MethodChaining(string[] methods)
	{
		Variant result = new();

		foreach (string method in methods)
		{
			var (name, args) = Parser.ParseMethod(method);

			if (result.As<Node>() is { })
			{
				if (args.Length == 0
					? !CallMethod((Node)result, name, out result)
					: !CallMethod((Node)result, name, out result, args)) return false;
			}
			else
			{
				if (args.Length == 0
					? !CallMethod(Current, name, out result)
					: !CallMethod(Current, name, out result, args)) return false;
			}

			_terminal.Print(result.ToString());
		}

		return true;
	}
}
