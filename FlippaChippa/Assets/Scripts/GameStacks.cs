using System;

public struct GameStacks
{
	private int maxFlips;
	public int MaxFlips { get { return maxFlips; } }

	private Stack targetStack;
	public Stack Target { get { return targetStack; } }

	private Stack playerStack;
	public Stack Player { get { return playerStack; } }

	public GameStacks(Stack target, Stack player, int maxFlips) {
		targetStack = target;
		playerStack = player;
		this.maxFlips = maxFlips;
	}
}

