using System.Collections.Generic;

namespace SaasEcom.Core.Infrastructure.Helpers
{
    /// <summary>
    /// List of Tax rates for European countries.
    /// Ref: http://ec.europa.eu/taxation_customs/resources/documents/taxation/vat/how_vat_works/rates/vat_rates_en.pdf
    /// Last Updated: 12 Jan 2015
    /// </summary>
    public static class EuropeanVat
    {
        private static readonly Dictionary<string, int> CountriesTax = new Dictionary<string, int>
        {
            { "AT", 20 }, // Austria
            { "BE", 21 }, // Belgium
            { "BG", 20 }, // Bulgaria
            { "CY", 19 }, // Cyprus
            { "CZ", 21 }, // Czech Republic
            { "HR", 25 }, // Croatia
            { "DK", 25 }, // Denmark
            { "DE", 19 }, // Germany
            { "EE", 20 }, // Estonia
            { "EL", 23 }, // Greece
            { "ES", 21 }, // Spain
            { "FR", 20 }, // France
            { "IE", 23 }, // Ireland
            { "IT", 22 }, // Italy
            { "LV", 21 }, // Latvia 
            { "LT", 21 }, // Lithuania
            { "LU", 17 }, // Luxembourg
            { "HU", 27 }, // Hungary
            { "MT", 18 }, // Malta
            { "NL", 21 }, // Netherlands
            { "PL", 23 }, // Poland
            { "PT", 23 }, // Portugal
            { "RO", 24 }, // Romania
            { "SI", 22 }, // Slovenia
            { "SK", 20 }, // Slovakia
            { "FI", 24 }, // Finland
            { "SE", 25 }, // Sweden
            { "UK", 20 }, // United Kingdom
        };

        public static Dictionary<string, int> Countries
        {
            get { return CountriesTax; }
        } 
    }
}
