using System;

public struct GameStacks
{
	private Stack targetStack;
	public Stack Target { get { return targetStack; } }

	private Stack playerStack;
	public Stack Player { get { return playerStack; } }

	public GameStacks(Stack target, Stack player) {
		targetStack = target;
		playerStack = player;
	}
}

