namespace CodiceFiscaleLib.Helpers;

public static class CombinationsHelper {

    // Helper function for generating combinations
    public static IEnumerable<IEnumerable<T>> Combinations<T>(IEnumerable<T> list, int comboSize)
    {
        if (comboSize == 0)
        {
            yield return new T[0];
        }
        else
        {
            var enumerable = list.ToList();
            for (int i = 0; i < enumerable.Count; i++)
            {
                var head = enumerable.Skip(i).Take(1);
                var tail = enumerable.Skip(i + 1);
                foreach (var tailCombo in Combinations(tail, comboSize - 1))
                {
                    yield return head.Concat(tailCombo);
                }
            }
        }
    }
}