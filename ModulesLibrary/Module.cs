using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace ModulesLibrary
{
    // Module class
    public class Module
    {
        // Module attributes
        public string code { get; set; }
        public string name { get; set; }
        public double credits { get; set; }
        public double hours { get; set; }
        public double selfstudyHours { get; set; }
        public double selfHoursLeft { get; set; }

        // Constructor for creating module
        public Module(string codeIn, string nameIn, double creditsIn, double hoursIn)
        {
            // Assign attributes
            code = codeIn;
            name = nameIn;
            credits = creditsIn;
            hours = hoursIn;
        }

        public void calculateSelfstudy(int weeks)
        {
            // Calculates total hours of self study for this module
            selfstudyHours = ((credits * 10) / (double)weeks) - hours;
            selfHoursLeft = selfstudyHours;
        }
    }

    public class RsaEncryption
    {
        CspParameters cp = new CspParameters();
        private static RSACryptoServiceProvider csp;
        private RSAParameters _privateKey;
        private RSAParameters _publicKey;

        public RsaEncryption()
        {
            cp.KeyContainerName = "PasswordKeyContainer";
            csp = new RSACryptoServiceProvider(2048, cp);
            _privateKey = csp.ExportParameters(true);
            _publicKey = csp.ExportParameters(false);
        }

        public string GetPublicKey()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, _publicKey);
            return sw.ToString();
        }

        public string Encrypt(string plainText)
        {
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(_publicKey);
            var data = Encoding.Unicode.GetBytes(plainText);
            var cypher = csp.Encrypt(data, false);
            return Convert.ToBase64String(cypher);
        }

        public string Decrypt(string cypherText)
        {
            Debug.WriteLine("Decrypting: " + cypherText);
            var dataBytes = Convert.FromBase64String(cypherText);
            Debug.WriteLine("Converted to bytes...");
            csp.ImportParameters(_privateKey);
            Debug.WriteLine("Imported private key...");
            var plainText = csp.Decrypt(dataBytes, false);
            Debug.WriteLine("Decrypted text: " + Encoding.Unicode.GetString(plainText));
            return Encoding.Unicode.GetString(plainText);
        }
    }
}