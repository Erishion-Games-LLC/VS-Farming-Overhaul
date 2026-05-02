using FarmingOverhaul.src.Helpers.Validation;
using System.Collections.Generic;
using Vintagestory.API.Common;
using static FarmingOverhaul.src.Helpers.HelperFunctions;

namespace FarmingOverhaul.src.Constants.AnimalsConstants
{
    public class EstrusConstants : IConstants
    {
        //What months the animal is able to breed in. Defines when animal can start estrus cycle
        public EnumMonth[] BreedingSeason;

        //Estrus cycle is how long from start of heat to next start of heat
        public double EstrusCycleMinDays;
        public double EstrusCycleMaxDays;

        //Determines how long after the estrus cycle starts will heat begin
        public double TimeBeforeHeatMinDays;
        public double TimeBeforeHeatMaxDays;

        //Determines how long heat will actually last for
        public double HeatDurationMinDays;
        public double HeatDurationMaxDays;

        public double PeakFertilityStartsXPercentAfterHeatBegins;
        public double PearkFertilityEndsXPercentAfterHeatBegins;

        //Determines how long after heat begins will peak fertility be reached
        public double TimeBeforePeakFertilityMinDays;
        public double TimeBeforePeakFertilityMaxDays;

        //Determines how long peak fertility will last for
        public double PeakFertilityMinDays;
        public double PeakFertilityMaxDays;

        public List<ValidationResult> ValidateVariableRange()
        {
            List<ValidationResult> errors = [];

            AddIfNotNull(errors, ValidateRange(EstrusCycleMinDays, EstrusCycleMaxDays, nameof(EstrusCycleMinDays)));
            AddIfNotNull(errors, ValidateRange(TimeBeforeHeatMinDays, TimeBeforeHeatMaxDays, nameof(TimeBeforeHeatMinDays)));
            AddIfNotNull(errors, ValidateRange(HeatDurationMinDays, HeatDurationMaxDays, nameof(HeatDurationMinDays)));
            AddIfNotNull(errors, ValidateRange(TimeBeforePeakFertilityMinDays, TimeBeforePeakFertilityMaxDays, nameof(TimeBeforePeakFertilityMinDays)));
            AddIfNotNull(errors, ValidateRange(PeakFertilityMinDays, PeakFertilityMaxDays, nameof(PeakFertilityMinDays)));
            AddIfNotNull(errors, ValidateRange(PeakFertilityStartsXPercentAfterHeatBegins, PearkFertilityEndsXPercentAfterHeatBegins, nameof(PeakFertilityStartsXPercentAfterHeatBegins)));
            
            return errors;
        }
    }
}