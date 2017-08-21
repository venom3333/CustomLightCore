namespace CustomLightCore.ViewModels.Mail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The call back mail view model.
    /// </summary>
    public class CallBackMailViewModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required(ErrorMessage = "Введите Имя!")]
        [DisplayName("Имя")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        [Required(ErrorMessage = "Введите номер телефона!")]
        [DisplayName("Телефон")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the misc.
        /// </summary>
        [DisplayName("Примечания")]
        [DataType(DataType.MultilineText)]
        public string Misc { get; set; }
    }
}
