using System.Collections;

public class GameStacksMeta 
{
	public StackMeta start;
	public StackMeta target;
	public int nFlips;

	public GameStacksMeta(StackMeta start, StackMeta target, int nFlips) {
		this.start = start;
		this.target = target;
		this.nFlips = nFlips;
	}
}

