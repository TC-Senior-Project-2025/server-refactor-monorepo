using Server.Modules.Game.Core;

namespace Server.Modules.Game.Services;

public static class PersonService
{
    public static bool DeathTick(int age, Random rng)
    {
        double probability;

        if (age < 20)
            probability = 0.0001;
        else if (age < 25)
            probability = 0.001418;
        else if (age < 30)
            probability = 0.000374;
        else if (age < 35)
            probability = 0.002;
        else if (age < 40)
            probability = 0.000877;
        else if (age < 45)
            probability = 0.001905;
        else if (age < 50)
            probability = 0.004617;
        else if (age < 55)
            probability = 0.001389;
        else if (age < 60)
            probability = 0.002327;
        else if (age < 65)
            probability = 0.005927;
        else if (age < 70)
            probability = 0.004;
        else if (age < 80)
            probability = 0.005;
        else
            probability = 0.03;

        return rng.NextDouble() < probability;
    }
}