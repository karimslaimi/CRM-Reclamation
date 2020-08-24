using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PFE_reclamation.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }





        [RegularExpression(@"^[A-Za-z ]+$",ErrorMessage = "Les caractères ne sont pas autorisés.")]
        [Required(ErrorMessage ="Le nom ne doit pas etre vide")]
        [MinLength(3, ErrorMessage = "doit etre plus long")]
        public string nom { get; set; }






        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Les caractères ne sont pas autorisés.")]
        [Required(ErrorMessage = "Le prenom ne doit pas etre vide")]
        [MinLength(3, ErrorMessage = "doit etre plus long")]
        public string prenom { get; set; }





        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Les caractères ne sont pas autorisés.")]
        [Required(ErrorMessage = "Le cin ne doit pas etre vide")]
        [StringLength(8,ErrorMessage ="Le cin doit etre 8 caractére")]
        [Remote("IscinExists", "Users", AdditionalFields = "id", ErrorMessage = "Cin existe")]
        public string cin { get; set; }





        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Les caractères ne sont pas autorisés.")]
        [Required(ErrorMessage = "Le numéro ne doit pas etre vide")]
        [MaxLength(8, ErrorMessage = "Le numéro doit etre 8 caractére")]
        public string tel { get; set; }






        [Required(ErrorMessage = "Le mail ne doit pas etre vide")]
        [StringLength(254,MinimumLength =3, ErrorMessage ="mail invalide")]
        [EmailAddress(ErrorMessage ="Format de l'adresse mail n'est pas valide")]
        [Remote("IsmailExists", "Users", AdditionalFields = "id", ErrorMessage = "Email existe")]
        public string mail { get; set; }





        [Required(ErrorMessage = "Le nom d'utilisateur ne doit pas etre vide")]
        [Remote("IsUsernameExists", "Users", AdditionalFields = "id", ErrorMessage = "Nom d'utilisateur existe")]
        public string username { get; set; }




        [Required(ErrorMessage = "Le mot de passe ne doit pas etre vide")]
        [StringLength(254,MinimumLength =8,ErrorMessage ="mot de passe ne doit pas etre inférieur à 8")]
        public string password { get; set; }







        [InverseProperty(nameof(Message.sentBy))]
        public virtual ICollection<Message> sentmessages { get; set; }

        [InverseProperty(nameof(Message.sentTo))]
        public virtual ICollection<Message> receivedmessages { get; set; }
    }
}