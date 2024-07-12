using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project_ArqueoList.Models
{
    public class Validacao
    {
        [Key]
        public int ID_Validacao { get; set; }

        [StringLength(100)]
        public string Estado { get; set; }

        [StringLength(2000)]
        public string Motivo { get; set; }

        [DataType(DataType.Date)] // informa a View de como deve tratar este atributo
        [DisplayFormat(ApplyFormatInEditMode = true,
                DataFormatString = "{0:dd-MM-yyyy}")]
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
        public DateOnly data_validacao { get; set; }

        [ForeignKey(nameof(ValidacaoArtigo))]
        public int ID_Artigo { get; set; }
        public Artigo ValidacaoArtigo { get; set; }


        [ForeignKey(nameof(AdminValidacao))]
        public int ID_Administrador { get; set; }
        public Administrador AdminValidacao { get; set; }
    }
}
