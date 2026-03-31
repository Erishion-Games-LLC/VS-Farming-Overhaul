using System.Text;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace FarmingOverhaul.src.Behaviors
{
    //need to eventually overwrited and implement BaseBehavior
    public class LactationBehavior(Entity entity) : EntityBehaviorMilkable(entity)
    {
        public const string LactationKey = "folactation";


        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);
        }

        public override void OnEntityLoaded()
        {
            return;
        }

        public override void OnEntitySpawn()
        {
            return;
        }

        public override void GetInfoText(StringBuilder infotext)
        {
            return;
        }
    }
}