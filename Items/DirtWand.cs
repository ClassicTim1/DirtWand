using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System;

namespace DirtWand.Items
{
	public class DirtWand : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dirt Wand");
			Tooltip.SetDefault("+20 range\nPlaces Dirt Blocks very quickly");
		}
		public override void SetDefaults()
		{
			item.useTurn = true;
			item.autoReuse = true;
            item.useAnimation = 5;
			item.useTime = 5;
			item.useStyle = 1;
			item.consumable = false;
            item.rare = 4;
			item.value = 500000;
			item.useAmmo = ItemID.DirtBlock;
		}
        
        public override bool CanUseItem(Player player){
            if(Math.Abs((player.position.X/16) - Player.tileTargetX) > Player.tileRangeX + 20 || Math.Abs((player.position.Y/16) - Player.tileTargetY) > Player.tileRangeY + 20){
                return false;
            }
            if(Main.tile[Player.tileTargetX, Player.tileTargetY].active()){
                if(!(Main.tileCut[Main.tile[Player.tileTargetX, Player.tileTargetY].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[Player.tileTargetX, Player.tileTargetY].type]))
                    return false;
            }
            for (int j = 0; j < player.inventory.Length; j++)
            {
                if (player.inventory[j].type == ItemID.DirtBlock && player.inventory[j].stack > 0)
                {
                    return true;
                }
            }
            return false;
        }
        
        public override bool UseItem(Player player)
		{
            int x = Player.tileTargetX;
            int y = Player.tileTargetY;
            int type = Main.tile[x, y].type;
            if(Math.Abs((player.position.X/16) - x) > Player.tileRangeX + 20 || Math.Abs((player.position.Y/16) - y) > Player.tileRangeY + 20){
                return false;
            }
            if(Main.tile[x, y].active()){
                if(Main.tileCut[type] || TileID.Sets.BreakableWhenPlacing[type])
                    WorldGen.KillTile(x, y);
                else
                    return false;
            }
            if(WorldGen.PlaceTile(x, y, TileID.Dirt)){
                for (int i = 0; i < player.inventory.Length; i++)
                    {
                        if (player.inventory[i].type == ItemID.DirtBlock)
                        {
                            player.inventory[i].stack--;
                        }
                    }
                if(Main.tile[x, y+1].slope() > 0 || Main.tile[x, y+1].halfBrick()){
                    Main.tile[x, y+1].slope(0);
                    Main.tile[x, y+1].halfBrick(false);
                }
                convertGrass(x, y+1);
                convertGrass(x+1, y);
                convertGrass(x-1, y);
            }
            return true;
		}

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtRod);
            recipe.AddIngredient(ItemID.IceRod);
			recipe.SetResult(this, 1);
            recipe.AddTile(TileID.WorkBenches);
			recipe.AddRecipe();
		}
        
        private void convertGrass(int x, int y){
            int type = Main.tile[x, y].type;
            if(!Main.tile[x,y-1].active())
                return;
            if(type == TileID.Grass || type == TileID.CorruptGrass|| type == TileID.HallowedGrass|| type == TileID.FleshGrass){
               Main.tile[x, y].type = TileID.Dirt;
               WorldGen.SquareTileFrame(x, y, true);
            }
        }
	}
}
