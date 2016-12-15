using System.Collections;

public class StackMetaPair 
{
	public CourseMeta courseMetaSource;
	public StackMeta start;
	public StackMeta target;
	public int nFlips;

	public StackMetaPair(CourseMeta courseMetaSource, StackMeta start, StackMeta target, int nFlips) {
		this.courseMetaSource = courseMetaSource;
		this.start = start;
		this.target = target;
		this.nFlips = nFlips;
	}
}

