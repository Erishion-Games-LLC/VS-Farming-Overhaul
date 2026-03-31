using System;
using System.Linq;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using static FarmingOverhaul.src.HelperFunctions;

namespace FarmingOverhaul.src.Behaviors
{
    public class BreedingBehavior(Entity entity) : BaseBehavior(entity)
    {
        public const string BreedingBehaviorKey = "fobreeding";
        public override string PropertyNameKey => BreedingBehaviorKey;
        public override string PropertyName() => PropertyNameKey;
        public override string TreeKey => PropertyNameKey;

        private AnimalState animalState;
        private AnimalConstants constants;
        private WeightBehavior weightBehavior;

        private long updateReproductionTickListenerId;
        private int updateReproductionTimerMs;

        public bool IsPregnant
        {
            get { return GetBoolFromTree(nameof(IsPregnant)); }
            set { SetBoolInTree(nameof(IsPregnant), value); }
        }
        public int FetusAmount
        {
            get { return GetIntFromTree(nameof(FetusAmount)); }
            set { SetIntInTree(nameof(FetusAmount), value); }
        }
        public double PregnancyLengthDays
        {
            get { return GetDoubleFromTree(nameof(PregnancyLengthDays)); }
            set { SetDoubleInTree(nameof(PregnancyLengthDays), value); }
        }
        private double PregnancyStartTotalDays
        {
            get { return GetDoubleFromTree(nameof(PregnancyStartTotalDays)); }
            set { SetDoubleInTree(nameof(PregnancyStartTotalDays), value); }
        }
        private double BeforeCanBePregnantAgainTotalDays
        {
            get { return GetDoubleFromTree(nameof(BeforeCanBePregnantAgainTotalDays)); }
            set { SetDoubleInTree(nameof(BeforeCanBePregnantAgainTotalDays), value); }
        }
        public bool LateGestation
        {
            get { return GetBoolFromTree(nameof(LateGestation)); }
            set { SetBoolInTree(nameof(LateGestation), value); }
        }               
        public bool CompletedFirstEstrus
        {
            get { return GetBoolFromTree(nameof(CompletedFirstEstrus)); }
            set { SetBoolInTree(nameof(CompletedFirstEstrus), value); }
        }
        public double EstrusCycleTotalStartDays
        {
            get { return GetDoubleFromTree(nameof(EstrusCycleTotalStartDays)); }
            set { SetDoubleInTree(nameof(EstrusCycleTotalStartDays), value); }
        }
        public double EstrusCycleLengthDays
        {
            get { return GetDoubleFromTree(nameof(EstrusCycleLengthDays)); }
            set { SetDoubleInTree(nameof(EstrusCycleLengthDays), value); }
        }
        public double EstrusCycleTimeBeforeHeatHours
        {
            get { return GetDoubleFromTree(nameof(EstrusCycleTimeBeforeHeatHours)); }
            set { SetDoubleInTree(nameof(EstrusCycleTimeBeforeHeatHours), value); }
        }
        public double EstrusCycleHeatDurationHours
        {
            get { return GetDoubleFromTree(nameof(EstrusCycleHeatDurationHours)); }
            set { SetDoubleInTree(nameof(EstrusCycleHeatDurationHours), value); }
        }
        public double EstrusCycleHoursUntilPeakFertility
        {
            get { return GetDoubleFromTree(nameof(EstrusCycleHoursUntilPeakFertility)); }
            set { SetDoubleInTree(nameof(EstrusCycleHoursUntilPeakFertility), value); }
        }
        public double EstrusCyclePeakFertilityHours
        {
            get { return GetDoubleFromTree(nameof(EstrusCyclePeakFertilityHours)); }
            set { SetDoubleInTree(nameof(EstrusCyclePeakFertilityHours), value); }
        }
        public double TotalDaysWhenPeakFertilityStarts
        {
            get { return GetDoubleFromTree(nameof(TotalDaysWhenPeakFertilityStarts)); }
            set { SetDoubleInTree(nameof(TotalDaysWhenPeakFertilityStarts), value); }
        }
        public double TotalDaysWhenPeakFertilityEnds
        {
            get { return GetDoubleFromTree(nameof(TotalDaysWhenPeakFertilityEnds)); }
            set { SetDoubleInTree(nameof(TotalDaysWhenPeakFertilityEnds), value); }
        }


        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);
            animalState = entity.GetBehavior<AnimalState>();
            weightBehavior = entity.GetBehavior<WeightBehavior>();

            if (animalState == null || weightBehavior == null)
            {
                Logger.Error("FARMING OVERHAUL missing required behaviors for Breeding Behavior to function: " + GetSpeciesStringLowerFromEntity(entity));
                return;
            }
            constants = animalState.Constants;

            /*Calculate how often the reproduction function should be called based on the game speed.
             * We want to call every 30 in game minutes, so we divide 1800 seconds by the game speed to get how many real life seconds should be between each call,
             * and then multiply by 1000 to convert to milliseconds for the timer.
             */
            //updateReproductionTimerMs = (int)(1800 / (Calendar.SpeedOfTime * Calendar.CalendarSpeedMul)) * 1000;
            updateReproductionTimerMs = 15000;

            if (ApiSide == EnumAppSide.Server) { EnableTickListeners(); }            
        }

        public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage)
        {
            base.OnEntityReceiveDamage(damageSource, ref damage);
            GetPregnant();
        }
        public override void OnEntityDeath(DamageSource damageSourceForDeath)
        {
            base.OnEntityDeath(damageSourceForDeath);
            DieOrDespawn();
        }
        public override void OnEntityDespawn(EntityDespawnData despawn)
        {
            base.OnEntityDespawn(despawn);
            DieOrDespawn();
        }

        public void DieOrDespawn()
        {
            DisableTickListeners();
        }

        /*Entity Lifecycle for female animals -
         * DONE Naturally generated animals should (If generated during their breeding season) already be or have a chance to be, in their estrus cycle.
         * DONE We constantly check to see if the animal can start its estrus cycle. 
         * DONE If it can, we stop repeatedly checking and actually start the cycle. 
         * DONE The animal is now fully ready to breed.
         * TODO Have female animals exhibit signs of heat. 
         * TODO Implement the Ram Effect
         * TODO Have the male animal impregnate the female with an actual action that THEY decide to do, not just by being close.
         * DONE Check if the impregnation is successful
         * 
         * 
         * Check fertibility level based on when after the current heat started the ewe was inseminated and current weight
         * Combine fertibility level with something else
         * if check passes, ewe gets pregnant. Roll fetus count
        */
        public override void EnableTickListeners()
        {
            updateReproductionTickListenerId = Api.Event.RegisterGameTickListener(UpdateReproduction, updateReproductionTimerMs);
        }
        
        public override void DisableTickListeners()
        {
            Api.Event.UnregisterGameTickListener(updateReproductionTickListenerId);
        }

        protected void UpdateReproduction(float deltaTime)
        {
            Logger.Error("Start UpdateReproduction");
            //If animal is dead, stop all ticking.
            if (!entity.Alive) { DisableTickListeners(); Logger.Error("entity is dead");
                return; }

            //If animal is pregnant, check if it can give birth. If it can, give birth and end pregnancy.
            if (IsPregnant)
            {
                Logger.Error("Is Pregnant");

                if (TryGiveBirth())
                {
                    EndPregnancy();
                }
                return;
            }

            //If animal is not pregnant, check if it can start estrus cycle.
            if (TryStartEstrusCycle()) return;

            //If animals estrus cycle has already started, check if it should end.
            if (Calendar.TotalDays - EstrusCycleTotalStartDays >= EstrusCycleLengthDays)
            {
                EndEstrusCycle();
            }
        }

        //Attempts to initiate the estrus cycle for the female animal if all required conditions are met.
        protected bool TryStartEstrusCycle()
        {
            //If the animal is pregnant or if the cycle is already started, end the repeating function
            if (EstrusCycleTotalStartDays != -1 || IsPregnant) { return false; }

            //If it is not the breeding season or if the animal hasn't recovered from a previous pregnancy yet, repeat the function to check again later
            if (!IsBreedingSeason() || IsBreedingCooldownActive()) { return false; }

            StartEstrusCycle(); return true;
        }

        //DONE
        //Initializes the estrus cycle for the animal, setting the duration and timing parameters for the current cycle.
        protected void StartEstrusCycle()
        {
            EstrusCycleLengthDays = SampleNormalDistributionInRange(Rand, constants.EstrusCycleMinDays, constants.EstrusCycleMaxDays);
            EstrusCycleTimeBeforeHeatHours = SampleNormalDistributionInRange(Rand, constants.EstrusCycleTimeBeforeHeatMinHours, constants.EstrusCycleTimeBeforeHeatMaxHours);
            EstrusCycleHeatDurationHours = SampleNormalDistributionInRange(Rand, constants.EstrusCycleHeatDurationMinHours, constants.EstrusCycleHeatDurationMaxHours);
            EstrusCycleHoursUntilPeakFertility = SampleNormalDistributionInRange(Rand, constants.EstrusCycleTimeBeforePeakFertilityMinHours, constants.EstrusCycleTimeBeforePeakFertilityMaxHours);
            EstrusCyclePeakFertilityHours = SampleNormalDistributionInRange(Rand, constants.EstrusCyclePeakFertilityMinHours, constants.EstrusCyclePeakFertilityMaxHours);

            double totalDays = Calendar.TotalDays;

            //If the animal is naturally generated AND it is has not completed its first Estrus, then it could have been generated at any point in the cycle. 
            if (entity.Attributes.GetString("origin") != "reproduction" && !CompletedFirstEstrus)
            {
                EstrusCycleTotalStartDays = GenerateRandomDouble(Rand, totalDays - EstrusCycleLengthDays, totalDays);
            }
            //If the animal is not naturally generated, then it starts the cycle at the normal time.
            else
            {
                EstrusCycleTotalStartDays = totalDays;
            }

            TotalDaysWhenPeakFertilityStarts = EstrusCycleTotalStartDays + (EstrusCycleHoursUntilPeakFertility / 24);
            TotalDaysWhenPeakFertilityEnds = TotalDaysWhenPeakFertilityStarts + (EstrusCyclePeakFertilityHours / 24);
        }

        //DONE
        //End the animals estrus cycle. Set the variables needed during this cycle to -1.
        protected void EndEstrusCycle()
        {
            CompletedFirstEstrus = true;

            EstrusCycleLengthDays = -1;
            EstrusCycleTimeBeforeHeatHours = -1;
            EstrusCycleHeatDurationHours = -1;
            EstrusCycleHoursUntilPeakFertility = -1;
            EstrusCyclePeakFertilityHours = -1;

            EstrusCycleTotalStartDays = -1;
            TotalDaysWhenPeakFertilityStarts = -1;
            TotalDaysWhenPeakFertilityEnds = -1;
        }

        //TODO Eventually check based on species specific needs, like shorter daylight hours for sheep.
        //Is the current month in the animals breeding season?
        protected bool IsBreedingSeason()
        {
            if (constants.BreedingSeason == null) { return false; }
            if (constants.BreedingSeason.Contains(Calendar.MonthName)) return true;
            else return false;
        }

        //DONE
        //Has the required time passed for the animal to breed again?
        protected bool IsBreedingCooldownActive()
        {
            if (Calendar.TotalDays >= BeforeCanBePregnantAgainTotalDays) return false;
            else return true;
        }

        //DONE
        /*If the current amount of time passed is inbetween the min and max amount of time passed for this heat, then the animal is in heat*/
        protected bool IsInHeat()
        {
            double heatBeginsTotalDays = EstrusCycleTotalStartDays + (EstrusCycleTimeBeforeHeatHours / 24);
            double heatEndsTotalDays = heatBeginsTotalDays + (EstrusCycleHeatDurationHours / 24);
            double totalDays = Calendar.TotalDays;

            if (totalDays >= heatBeginsTotalDays && totalDays <= heatEndsTotalDays) return true;
            else return false;
        }
       
        //DONE
        protected void TryGetPregnant()
        {
            //If already pregnant, exit
            if (IsPregnant) return;

            //See if impregnation should fail based on the base impregnation fail chance and the modifiers to it.
            if (Rand.NextDouble() <= CalculateImpregnationFailChance()) return;

            //Impregnation was successful, so get pregnant.
            GetPregnant();
        }

        protected void GetPregnant()
        {
            //If already pregnant, don't do anything. Should never reach here
            if (IsPregnant) return;

            IsPregnant = true;
            FetusAmount = (int)SampleNormalDistributionInRange(Rand, constants.MinFetusAmount, constants.MaxFetusAmount);
            PregnancyLengthDays = SampleNormalDistributionInRange(Rand, constants.MinDaysPregnant, constants.MaxDaysPregnant);
            PregnancyStartTotalDays = Calendar.TotalDays;
            EndEstrusCycle();
        }

        //Set up the variables pertaining to the end of pregnancy, and then restart the looping function to try and begin the estrus cycle anew
        protected void EndPregnancy()
        {
            IsPregnant = false;
            PregnancyLengthDays = -1;
            PregnancyStartTotalDays = -1;
            BeforeCanBePregnantAgainTotalDays = Calendar.TotalDays + SampleNormalDistributionInRange(Rand, constants.MinDaysBeforeBreedAgainFemale, constants.MaxDaysBeforeBreedAgainFemale);
        }

        protected bool TryGiveBirth()
        {
            if (Calendar.TotalDays < PregnancyStartTotalDays + PregnancyLengthDays) return false;

            GiveBirth(); return true;
        }

        protected void GiveBirth()
        {
            while (FetusAmount > 0)
            {
                FetusAmount -= 1;
                SpawnChild(entity);
            }
        }

        //TODO
        //Need to scale fail chance with weight, temp, health, age, etc.
        //If the returned double is higher than a random double between 0 and 1, then impregnation fails. If it is lower, impregnation succeeds.
        protected double CalculateImpregnationFailChance()
        {
            double baseFailChance = constants.ImpregnationFailChance;

            if (TotalDaysWhenPeakFertilityStarts < 0 || TotalDaysWhenPeakFertilityEnds < 0) return baseFailChance;

            double fertilityStrength = GetGaussianWeight(Calendar.TotalDays, TotalDaysWhenPeakFertilityStarts, TotalDaysWhenPeakFertilityEnds);
            double failChance = baseFailChance * (1.0  - fertilityStrength);

            return Math.Clamp(failChance, 0.0, 1.0);
        }

        //TODO
        //need to add support for father entity when genetics are implemented
        //Spawn a child entity with the appropriate species and type, based on the mother.
        protected void SpawnChild(Entity mother)
        {
            if (ApiSide != EnumAppSide.Server)
            {
                return;
            }
            string gender = DetermineGender();
            var rand = World.Rand;

            string entityCode = $"{animalState.Species}-{animalState.Type}-baby-{gender}";
            AssetLocation assetLocation = new AssetLocation(entityCode);

            EntityProperties entityType = World.GetEntityType(assetLocation);

            Entity child = World.ClassRegistry.CreateEntity(entityType);

            child.ServerPos.SetFrom(mother.ServerPos);
            child.ServerPos.Motion.X += (rand.NextDouble() - 0.5) / 20.0;
            child.ServerPos.Motion.Z += (rand.NextDouble() - 0.5) / 20.0;
            child.Pos.SetFrom(child.ServerPos);

            child.Attributes.SetString("origin", "reproduction");
            child.WatchedAttributes.SetInt("generation", animalState.Generation + 1);

            World.SpawnEntity(child);
        }


        //TODO
        //need to eventually add support for uneven sex distribution
        protected string DetermineGender()
        {     
            //Roll a random integer between 0 and 1. If 0, gender is female. If 1, gender is male.
            if (GenerateRandomIntFromMaxInclusive(Rand, 1) == 0) return "female";
            else return "male";
        }

        public override void GetInfoText(StringBuilder sb)
        {
            base.GetInfoText(sb);
            sb.AppendLine($"Is Pregnant: {IsPregnant}");
            if (IsPregnant)
            {
                sb.AppendLine($"Fetus Amount: {FetusAmount}");
                sb.AppendLine($"Days Pregnant: {Calendar.TotalDays - PregnancyStartTotalDays} / {PregnancyLengthDays}");
                sb.AppendLine($"Late Gestation: {LateGestation}");
            }
            else
            {
                sb.AppendLine($"In Heat: {IsInHeat()}");
                sb.AppendLine($"Breeding Cooldown Active: {IsBreedingCooldownActive()}");
            }
        }

        ////Will be removed once animals need to mate themselves
        //protected bool IsRequiredMaleNearby()
        //{
        //    float range = constants.RadiusMaleForHeat;
        //    Entity[] entities = World.GetEntitiesAround(entity.Pos.XYZ, range, range);

        //    //Search all nearby entities in range for a male matching species
        //    foreach (Entity e in entities)
        //    {
        //        AnimalState aState = e.GetBehavior<AnimalState>();

        //        if (aState == null) continue;

        //        if (aState.Species != animalState.Species) continue;

        //        if (aState.Gender == "male") return true;
        //    }

        //    //No male matching species was found
        //    return false;
        //}

        //private bool InLateGestation(Entity animal)
        //{
        //    if (DaysPregnant / pregnancyLength < Constants.LateGestationPercent)
        //    {
        //        return false;
        //    }
        //    else return true;
        //}      
    }
}