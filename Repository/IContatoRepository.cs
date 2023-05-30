using ControleDeContatos.Models;

namespace ControleDeContatos.Repository {
    public interface IContatoRepository {  //Minha interface de Contato - Contrato 

        
        ContatoModel ListarPorId(int id);
        
        List<ContatoModel> BuscarTodos();

        ContatoModel Adicionar(ContatoModel contato);

        ContatoModel Atualizar(ContatoModel contato);

        bool Apagar(int id);
    }
}
