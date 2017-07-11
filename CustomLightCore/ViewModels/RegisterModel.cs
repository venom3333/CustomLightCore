using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CustomLightCore.ViewModels
{
    public class RegisterModel
    {
		[Required(ErrorMessage = "Не указан логин!")]
		public string Login { get; set; }

		[Required(ErrorMessage = "Не указан пароль!")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Пароли не совпадают")]
		public string ConfirmPassword { get; set; }
	}
}
