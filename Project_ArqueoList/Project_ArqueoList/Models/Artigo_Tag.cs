using System.ComponentModel.DataAnnotations.Schema;

namespace Project_ArqueoList.Models
{
    public class Artigo_Tag
    {
        [ForeignKey(nameof(ArtigoTag))]
        public int ID { get; set; }
        public Artigo ArtigoTag { get; set; }

        [ForeignKey(nameof(TagArtigo))]
        public int ID_Tag { get; set; }
        public Tag TagArtigo { get; set; }
    }
}
