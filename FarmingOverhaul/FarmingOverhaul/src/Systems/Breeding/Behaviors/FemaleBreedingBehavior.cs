using FarmingOverhaul.src.Systems.Breeding.States.Managers;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace FarmingOverhaul.src.Systems.Breeding.Behaviors
{
    public class FemaleBreedingBehavior(Entity entity) : BaseBreedingBehavior(entity)
    {
        public const string FemaleBreedingBehaviorKey = "fofemalebreeding";
        protected override string PropertyNameKey => FemaleBreedingBehaviorKey;

        private FemaleReproductionStateManager stateManager;


        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);

            stateManager = new FemaleReproductionStateManager(Rand, animalState, TreeAccessor, Logger, Calendar.DaysPerMonth);
            stateManager.OnBirth += GiveBirth;
        }

        public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage)
        {
            base.OnEntityReceiveDamage(damageSource, ref damage);
            Logger.Error($"{Calendar.TotalDays} : {Calendar.TotalHours} : {Calendar.Month}");

            //ChildSpawner.SpawnChild(entity, animalState, 99999);
        }

        protected override void UpdateReproduction(float deltaTime)
        {
            base.UpdateReproduction(deltaTime);
            stateManager.Update(Calendar.TotalDays, Calendar.MonthName);
        }

        protected void GiveBirth(int fetusAmount, double birthTotalDays)
        {
            while (fetusAmount > 0)
            {
                fetusAmount -= 1;
                ChildSpawner.SpawnChild(entity, animalState, birthTotalDays);
            }
        }
  
        public override void GetInfoText(StringBuilder infotext)
        {
            base.GetInfoText(infotext);
            infotext.AppendLine($"Reproduction State: {stateManager.CurrentState}");

        }

        //DONE
        /*If the current amount of time passed is inbetween the min and max amount of time passed for this heat, then the animal is in heat*/
        public static bool IsInHeat(double cycleStartTotalDays, double timeBeforeHeatHours, double heatDurationHours, double totalDays)
        {
            double heatBeginsTotalDays = cycleStartTotalDays + (timeBeforeHeatHours / 24);
            double heatEndsTotalDays = heatBeginsTotalDays + (heatDurationHours / 24);

            if (totalDays >= heatBeginsTotalDays && totalDays <= heatEndsTotalDays) return true;
            else return false;
        }
    }
}