using System.Collections;

public class StackMetaPair 
{
	public StackMeta start;
	public StackMeta target;
	public int nFlips;

	public StackMetaPair(StackMeta start, StackMeta target, int nFlips) {
		this.start = start;
		this.target = target;
		this.nFlips = nFlips;
	}
}

