namespace ApiService2;

public class BeerRate
{         
    public string Name { get; set; } = default!;
    public int Rate { get;  set; } =  0;
    public int Rates { get;  set; } = 1;


    public void SetRate(int value)
    {
        Rate = (Rate + value) / 2;
        Rates++;
    }
}
