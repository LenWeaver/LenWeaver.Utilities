using System;
using System.IO;
using System.Xml;

namespace LenWeaver.Utilities {

    public class CountryCode : IComparable<CountryCode> {

        private const string ISO3166XmlFile     = "LenWeaver.Utilities.CountryCode.ISO3166.xml";


        public  string  Alpha2      { get; }
        public  string  Alpha3      { get; }
        public  string  Name        { get; }
        public  string  Numeric     { get; }


        internal CountryCode( string name, string alpha2, string alpha3, string numeric ) {

            Name    = name;
            Alpha2  = alpha2;
            Alpha3  = alpha3;
            Numeric = numeric;
        }


        public override string ToString() {

            return Name;
        }


        public int CompareTo( CountryCode? other ) {
            
            return Name.CompareTo( other?.Name );
        }


        #region Static Methods
        public static CountryCode[] Load() {

            string              alpha2;
            string              alpha3;
            string              name;
            string              numeric;

            Stream?             s           = null;
            XmlDocument         xml;
            XmlNodeList         nodes;

            CountryCode[]       result;

            #nullable disable

            try {
                s                   = typeof(CountryCode).Assembly.GetManifestResourceStream( ISO3166XmlFile );
                xml                 = new XmlDocument();
                xml.Load( s );

                nodes               = xml.SelectNodes( "//CountryCodes/Country" );
                result              = new CountryCode[nodes.Count];

                for( int index = 0; index < nodes.Count; index++ ) {
                    name            = nodes[index].Attributes["Name"].InnerText;
                    alpha2          = nodes[index].Attributes["ISO3166-1-Alpha2"].InnerText;
                    alpha3          = nodes[index].Attributes["ISO3166-1-Alpha3"].InnerText;
                    numeric         = nodes[index].Attributes["ISO3166-1-Numeric"].InnerText;

                    result[index]   = new CountryCode( name, alpha2, alpha3, numeric );
                }
            }
            catch( Exception ex ) {
                throw new ApplicationException( "Unable to load country code data.", ex );
            }
            finally {
                if( s != null ) s.Dispose();
            }

            return result;
        }
        #endregion
    }
}