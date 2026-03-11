using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace FarmingOverhaul.src.Items
{
    internal class ItemThornsBlade : Item
    {
        public override void OnAttackingWith(IWorldAccessor world, Entity byEntity, Entity attackedEntity, ItemSlot itemslot)
        {
            DamageSource damage = new DamageSource()
            {
                Type = EnumDamageType.PiercingAttack,
                SourceEntity = byEntity,
                KnockbackStrength = 0,
            };
            if (!attackedEntity.Alive)
            {
                attackedEntity.Revive();
            }
            byEntity.ReceiveDamage(damage, 0.25f);
            base.OnAttackingWith(world, byEntity, attackedEntity, itemslot);
        }
    }
}
