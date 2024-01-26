using Fi.ApiBase;

namespace Fi.Template.Api
{
    public class Program : ProgramBase<Startup>
    {
        public new static void Main(string[] args)
        {
            ProgramBase<Startup>.Main(args);
        }
    }
}