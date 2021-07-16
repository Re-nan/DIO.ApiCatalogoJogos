using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Services
{
    public interface IJogoService
    {
        // Retorna uma lista de jogos com paginação
        Task<List<JogoViewModel>> Obter(int pagina, int quantidade);

        // Retorna um jogo só pelo seu id
        Task<JogoViewModel> Obter(Guid id);

        // Inseri um jogo
        Task<JogoViewModel> Inserir(JogoInputModel jogo);

        // Atualiza todas as propriedades de um jogo
        Task Atualizar(Guid id, JogoInputModel jogo);

        // Atualiza somente o preço de um jogo
        Task Atualizar(Guid id, double preco);

        // Remove um jogo pelo seu id
        Task Remover(Guid id);
    }
}
