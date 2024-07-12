using System.ComponentModel.DataAnnotations;

namespace Project_ArqueoList.Models
{
    public class Utilizador
    {
        public Utilizador()
        {
            ListaArtigo = new HashSet<Artigo>();
        }

        [Key]
        public int idUtilizador { get; set; }

        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        [StringLength(30, ErrorMessage = "Atingiu o limite de characters.")]
        public string Username { get; set; }

        //[Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
        //[StringLength(20, ErrorMessage = "Atingiu o limite de characters.")]
        //public string Password { get; set; }

        //public string Role { get; set; }

        [DataType(DataType.Date)] // informa a View de como deve tratar este atributo
        [DisplayFormat(ApplyFormatInEditMode = true,
               DataFormatString = "{0:dd-MM-yyyy}")]
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
        public DateTime Data_Nascimento { get; set; }

        [StringLength(40)]
        public string UserId { get; set; }

        public ICollection<Artigo> ListaArtigo { get; set; }
    }
}
