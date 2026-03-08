using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace FarmingOverhaul.assets.farmingoverhaul.blocks
{
    internal class BlockTrampoline : Block
    {
        public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact)
        {
            if (isImpact && facing.IsVertical)
            {
                entity.Pos.Motion.Y *= -0.8f;
            }
        }
    }
}