using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomLightCore.Models
{
	public partial class User : IdentityUser
	{
		public string FullName { get; set; }
		public DateTime BirthDate { get; set; }
	}
}
