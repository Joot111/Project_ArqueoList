namespace Project_ArqueoList.Models
{
    public class Administrador : Utilizador
    {
        public Administrador()
        {
            ListaValidacao = new HashSet<Validacao>();
        }

        public int idAdmin { get; set; }

        public ICollection<Validacao> ListaValidacao { get; set; }
    }
}
