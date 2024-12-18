using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPN1EnWeb.Entidades
{
    public class ApplicationUser : IdentityUser //Creo esta clase para agregarle campos a la tabla Users
    {
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        public int CountryId { get; set; }
        [Required]
        public int StateId { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        public string Zipcode { get; set; } = null!;
        public string? Phone { get; set; }
        public Country Country { get; set; }=null!;
        public State State { get; set; }= null!;
        public City City { get; set; } = null!;
        public List<OrderHeader> OrderHeaders { get; set; } = new List<OrderHeader>();
    }
}
