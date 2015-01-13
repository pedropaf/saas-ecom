using System.ComponentModel.DataAnnotations;

namespace SaasEcom.Core.Models
{
    /// <summary>
    /// Class that represents a billing Address for a customer
    /// </summary>
    public class BillingAddress
    {
        /// <summary>
        /// Gets or sets the name of the person / company.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        [Display(Name = "Name or Company Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address line1.
        /// </summary>
        /// <value>
        /// The address line1.
        /// </value>
        [Required]
        [Display(Name="Address 1")]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line2.
        /// </summary>
        /// <value>
        /// The address line2.
        /// </value>
        [Display(Name = "Address 2")]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        [Required]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        [Required]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>
        /// The zip code.
        /// </value>
        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        [Required]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the vat.
        /// </summary>
        /// <value>
        /// The vat.
        /// </value>
        [Display(Name = "VAT")]
        public string Vat { get; set; }
    }
}
