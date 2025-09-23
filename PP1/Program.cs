// See https://aka.ms/new-console-template for more information

class Program
{
    static int SumFor(int n)
    {
        return (n * (n + 1)) / 2;
    }

    static int SumIte(int n)
    {
        int sum = 0;
        for (int i = 1; i <= n; i++)
        {
            sum += i;
        }
        return sum;
    }

    static void Main()
    {

        int max = int.MaxValue;

        //ascendente
        int nfoor_asce = 0;
        int sumfoor_asce = 0;

        for (int n = 1; n <= max; n++)
        {
            int su = SumFor(n);

            if (su > 0)
            {
                nfoor_asce = n;
                sumfoor_asce = su;
            }
            else
            {
                break;
            }

        }
        //descendente
        int nfoor_desc = 0;
        int sumfoor_desc = 0;

        for (int n = max; n >= 1; n--)
        {
            int su = SumFor(n);

            if (su > 0)
            {
                nfoor_desc = n;
                sumfoor_desc = su;
            }
            else
            {
                break;
            }

        }


        //ascendente 

        int nite_asce = 0;
        int sumite_asce = 0;

        for (int n = 1; n <= max; n++)
        {
            int su = SumFor(n);

            if (su > 0)
            {
                nite_asce = n;
                sumite_asce = su;
            }
            else
            {
                break;
            }

        }

        //descendente
        int nite_desc = 0;
        int sumite_desc = 0;

        for (int n = max; n >= 1; n--)
        {
            int su = SumFor(n);

            if (su > 0)
            {
                nite_desc = n;
                sumite_desc = su;
            }
            else
            {
                break;
            }

        }

        Console.WriteLine("SumFor:");
        Console.WriteLine($"From 1 to Max: n={nfoor_asce}, sum={sumfoor_asce}");
        Console.WriteLine($"From Max to 1: n={nfoor_desc}, sum={sumfoor_desc}");

        Console.WriteLine("SumIte:");
        Console.WriteLine($"From 1 to Max: n={nite_asce}, sum={sumite_asce}");
        Console.WriteLine($"From Max to 1: n={nite_desc}, sum={sumite_desc}");

    }
    
}