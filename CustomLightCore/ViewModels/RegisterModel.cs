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
		public string UserName { get; set; }

		[Required(ErrorMessage = "Не указан пароль!")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Пароли не совпадают")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "Не указан email!")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required(ErrorMessage = "Укажите полное имя!")]
		public string FullName { get; set; }

		[Required(ErrorMessage = "Укажите дату рождения!")]
		[DataType(DataType.Date)]
		public DateTime BirthDate { get; set; }
	}
}
