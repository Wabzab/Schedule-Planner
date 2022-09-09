namespace ModulesLibrary
{
    // Module class
    public class Module
    {

        public string code { get; set; }
        public string name { get; set; }
        public double credits { get; set; }
        public double hours { get; set; }
        public double selfstudyHours { get; set; }
        public double selfHoursLeft { get; set; }

        public Module(string codeIn, string nameIn, double creditsIn, double hoursIn)
        {
            code = codeIn;
            name = nameIn;
            credits = creditsIn;
            hours = hoursIn;
        }

        public void calculateSelfstudy(int weeks)
        {
            // Calculates the total hours of self study for this module
            selfstudyHours = ((credits * 10) / (double)weeks) - hours;
            selfHoursLeft = selfstudyHours;
        }

    }
}