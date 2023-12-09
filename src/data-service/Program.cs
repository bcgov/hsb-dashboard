namespace HSB.DataService;

class Program
{
    static Task<int> Main(string[] args)
    {
        var program = new ServiceManager(args);
        return program.RunAsync();
    }
}
