using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_ArqueoList.Models
{
    public class Artigo
    {
        public Artigo()
        {
            ListaValidacao = new HashSet<Validacao>();
            ListaArtigoTags = new HashSet<Artigo_Tag>();
        }

        [Key]
        public int ID { get; set; }

        [StringLength(70)]
        public string Titulo { get; set; }

        [StringLength(5000)]
        public string Conteudo { get; set; }

        [Display(Name = "Imagem")] // altera o nome do atributo no ecrã
        [StringLength(50)] // define o tamanho máximo como 50 caracteres
        public string Imagem { get; set; }

        [StringLength(50)]
        public string Nome_Autor { get; set; }

        public Boolean validado { get; set; }

        [DataType(DataType.Date)] // informa a View de como deve tratar este atributo
        [DisplayFormat(ApplyFormatInEditMode = true,
                        DataFormatString = "{0:dd-MM-yyyy}")]
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
        public DateTime data_validacao { get; set; }

        [StringLength(50)]
        public string tipo { get; set; }


        [ForeignKey(nameof(UtilArtigo))]
        public int ID_Utilizador { get; set; }
        public Utilizador UtilArtigo { get; set; }

        /*[ForeignKey(nameof(UtenteArtigo))]
        public int ID_Utente { get; set; }
        public Utente UtenteArtigo { get; set; }

        [ForeignKey(nameof(AdminArtigo))]
        public int ID_Administrador { get; set; }
        public Administrador AdminArtigo { get; set; }

        [ForeignKey(nameof(AutorArtigo))]
        public int ID_Autor { get; set; }
        public Autor AutorArtigo { get; set; }*/

        public ICollection<Validacao> ListaValidacao { get; set; }
        public ICollection<Artigo_Tag> ListaArtigoTags { get; set; }

    }
}
