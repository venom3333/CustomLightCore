// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderMailViewModel.cs" company="CustomLight">
//   CustomLight
// </copyright>
// <summary>
//   The order mail view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace CustomLightCore.ViewModels.Mail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The order mail view model.
    /// </summary>
    public class OrderMailViewModel
    {
        /// <summary>
        /// Gets or sets the delivery type.
        /// </summary>
        [Required(ErrorMessage = "Укажите тип доставки!")]
        [DisplayName("Тип доставки")]
        [DataType(DataType.Text)]
        public string DeliveryType { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required(ErrorMessage = "Укажите ФИО!")]
        [DisplayName("ФИО")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        [Required(ErrorMessage = "Укажите номер телефона!")]
        [DisplayName("Телефон")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        [DisplayName("Адрес доставки")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the misc.
        /// </summary>
        [DisplayName("Примечания")]
        [DataType(DataType.MultilineText)]
        public string Misc { get; set; }
    }
}
