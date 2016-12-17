using System;
using System.Collections.Generic;

public class CrushChipsMeta
{
	public List<ChipMeta> crushedChips, fallingChips;
	public bool HasCrushedChips { get { return crushedChips.Count > 0; } }

	public CrushChipsMeta (List<ChipMeta> crushedChips, List<ChipMeta> fallingChips) 
	{
		this.crushedChips = crushedChips;
		this.fallingChips = fallingChips;
	}

	public CrushChipsMeta() : this(new List<ChipMeta>(), new List<ChipMeta>()) {
		
	}
}

