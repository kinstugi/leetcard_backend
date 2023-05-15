namespace SharpCardAPI.Utility;

public class SM2Algorithm{
    public static void CalculateNewInterval(int repititions, DateTime previousInterval,double previousEaseFactor, int quality, out DateTime newInterval, out double newEaseFactor, out int newRepitition){
        if (quality >= 3){
            // correct response
            if (repititions == 0){
                newInterval = DateTime.Now.AddDays(1);
            }else if(repititions == 1){
                newInterval = previousInterval.AddDays(6);
            }else{
                var ticks = (long) Math.Round(previousInterval.Ticks * previousEaseFactor);
                newInterval = new DateTime(ticks);
            }
            newEaseFactor = (0.1 - (5 - quality) * (0.08 + (5 - quality ) * 0.02)) + previousEaseFactor;
            newRepitition = repititions + 1;
        }else{
            // incorrect response
            newInterval = DateTime.Now.AddSeconds(30);
            newRepitition = 0;
            newEaseFactor = previousEaseFactor;
        }
        if (newEaseFactor < 1.3)
            newEaseFactor = 1.3;
    }
}