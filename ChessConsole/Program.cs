var a =1;
var d = new M();
Console.WriteLine(d.Get());

class M
{
    public event Func<int,int> Ev;

    public int F(int i)
    {
        return i;
    }
    public int G(int i)
    {
        return i*2;
    }
    public M()
    {
        
        Ev+=F;
        Ev += G;
    }

    public int? Get()
    {
        return Ev?.Invoke(2);
    }
}