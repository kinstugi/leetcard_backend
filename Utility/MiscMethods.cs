using SharpCardAPI.Models;

namespace SharpCardAPI.Utility;

public class MiscMethods{
    // this function will shuffle an array
    public static void Shuffle(ref List<object> array){
        Random random = new Random();
        for(int i = 0; i < array.Count; i++){
            int randomIndex = random.Next(array.Count);
            object temp = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = temp;
        }
    }

    // this function will take a List of objects and int k as input and return a list of randomly selected objects of size k
    public static List<T> SelectRandomDistinct<T>(List<T> items, int k){
        List<T> result = new List<T>();
        Random random = new Random();
        for (int i = 0; i < k; ++i){
            int j = random.Next(i, items.Count);
            result.Add(items[j]);
            T temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }
        return result;
    }
}