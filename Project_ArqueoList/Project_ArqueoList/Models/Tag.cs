using System.ComponentModel.DataAnnotations;

namespace Project_ArqueoList.Models
{
    public class Tag
    {
        public Tag()
        {
            ListaArtigoTags = new HashSet<Artigo_Tag>();
        }

        [Key]
        public int ID_Tag { get; set; }
        [StringLength(50)]
        public int Nome { get; set; }

        public ICollection<Artigo_Tag> ListaArtigoTags { get; set; }
    }
}
