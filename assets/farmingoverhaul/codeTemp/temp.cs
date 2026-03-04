public enum SpeciesList
{
    Hare, Chicken, Pig, Sheep, Goat
}

public class Animal
{
    SpeciesList Species = SpeciesList.Sheep;
    double maintenanceEnergyReqConst = 0.056;
    double bodyWeightExponent = 0.75;
    double growthCoefficient = 0.276;
    double lactationEnergyReqConst = 5;
    int maxDaysPregnant = 150;
    double baseFetusEnergyModifier = 0.1;
    double additionalFetusEnergyModifier = 0.05;
    double earlyGestationFetusEnergyModifier = 0.025;
    double tempModifier = 1;
    double kCalPerKg = 0;

    //Animal State
    double kgMilkProduced = 1;
    bool lateGestation = false;
    int fetusAmount = 0;
    int daysPregnant = 0;
    bool belowTempThresh = false;

    public Animal(SpeciesList species)
    {
        Species = species;
    }
}

public class CalculateAnimalEnergyReq
{
    public static CalculateAnimalEnergyReq()
    {
        metabolicBodyWeight = Math.Pow(bodyWeight, bodyWeightExponent);
        baseMaintenanceEnergyReq = maintenanceEnergyReqConst * metabolicBodyWeight;
        lactationEnergyReq = lactationEnergyReqConst * kgMilkProduced;

        switch (lateGestation)
        {
            case true:
            pregnancyEnergyReq = (baseFetusEnergyModifier + (additionalFetusEnergyModifier * (fetusAmount - 1))) * bodyWeight;
            case false:
            if (fetusAmount < 0)
                {
                    pregnancyEnergyReq = earlyGestationFetusEnergyModifier * fetusAmount * bodyWeight;
                }
            else
                {
                    pregnancyEnergyReq = 0;
                }
        }
        if (temp > upperTempRange)
        {
            //have it scale based on high much higher it is
            tempModifier = 1.25;
        }
        if (temp < lowerTempRange)
        {
            //have it scale based on high much lower it is
            tempModifier = 1.25;
        }
        totalMaintenanceEnergyReq = (baseMaintenanceEnergyReq + lactationEnergyReq + pregnancyEnergyReq) * tempModifier;
        double excessCalories = caloriesConsumed - totalMaintenanceEnergyReq;

        if (excessCalories != 0)
        {
            weightChangeKg = excessCalories / kCalPerKg;
            animal.weight = animal.weight + weightChangeKg;
        }
    }
}