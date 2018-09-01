using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExampleMod.Items
{
	public class DirtBlock : GlobalItem
	{
		public override void SetDefaults(Item item)
		{
			if (item.type == ItemID.DirtBlock)
			{
				item.ammo = ItemID.DirtBlock;
			}
		}
	}
}
