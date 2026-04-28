using System.Collections.Generic;
using Vintagestory.API.Common;
using static FarmingOverhaul.src.HelperFunctions;

namespace FarmingOverhaul.src.Constants.AnimalConstants
{
    public class EstrusConstants : IConstants
    {
        //What months the animal is able to breed in. Defines when animal can start estrus cycle
        public EnumMonth[] BreedingSeason;

        //Estrus cycle is how long from start of heat to next start of heat
        public double EstrusCycleMinDays;
        public double EstrusCycleMaxDays;

        //Determines how long after the estrus cycle starts will heat begin
        public double TimeBeforeHeatMinHours;
        public double TimeBeforeHeatMaxHours;

        //Determines how long heat will actually last for
        public double HeatDurationMinHours;
        public double HeatDurationMaxHours;

        //Determines how long after heat begins will peak fertility be reached
        public double TimeBeforePeakFertilityMinHours;
        public double TimeBeforePeakFertilityMaxHours;

        //Determines how long peak fertility will last for
        public double PeakFertilityMinHours;
        public double PeakFertilityMaxHours;

        public List<string> ValidateVariableRange()
        {
            List<string> errors = [];

            AddIfNotNull(errors, ValidateRange(EstrusCycleMinDays, EstrusCycleMaxDays, nameof(EstrusCycleMinDays)));
            AddIfNotNull(errors, ValidateRange(TimeBeforeHeatMinHours, TimeBeforeHeatMaxHours, nameof(TimeBeforeHeatMinHours)));
            AddIfNotNull(errors, ValidateRange(HeatDurationMinHours, HeatDurationMaxHours, nameof(HeatDurationMinHours)));
            AddIfNotNull(errors, ValidateRange(TimeBeforePeakFertilityMinHours, TimeBeforePeakFertilityMaxHours, nameof(TimeBeforePeakFertilityMinHours)));
            AddIfNotNull(errors, ValidateRange(PeakFertilityMinHours, PeakFertilityMaxHours, nameof(PeakFertilityMinHours)));
            
            return errors;
        }
    }
}