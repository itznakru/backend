using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItZnak.PatentsDtoLibrary.Types.Members
{
    /// Roles
    /// supervisor member user
    /// supervisor 
    /// member - участник - юридическая компания
    /// user - конечный пользователь системы

    public class Identity
    {
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";// логин
        public string Name { get; set; } = ""; // наименование организации
                                               //   public string FIO {get;set;} // ФИО представителя 
        public string Role { get; set; } = "";

    }
}