using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Services;
using ApiCatalogoJogos.ViewModel;
using System.ComponentModel.DataAnnotations;
using ApiCatalogoJogos.Exceptions;

namespace ApiCatalogoJogos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        // propriedade somente leitura, pois a responsabilidade de dar uma instancia pra ela será do Aspnet
        private readonly IJogoService _jogoService;

        // Criando um contrutor que seta minha propriedade readonly
        public JogosController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }


        // Neste GET eu retorno uma lista de jogos (todos os jogos)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            // jogos é um Task de lista de jogos --> Task<List<JogoViewModel>>
            var jogos = await _jogoService.Obter(pagina, quantidade); // os dados vem da Query

            if(jogos.Count == 0)
            {
                //se lista for vazia ele irá retornar que não existe conteúdo
                return NoContent();
            }
            else
            {
                // Rotorna um status 200 OK
                return Ok(jogos);
            }
            
        }


        // Neste GET eu tetorno um jogo específico pelo seu ID
        [HttpGet("{idJogo:guid}")]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter([FromRoute] Guid idJogo)
        {
            var jogo = await _jogoService.Obter(idJogo); // ao invés de vir da Query irá vir da rota {idJogo:guid} na url

            if (jogo == null)
                return NoContent();

            // Rotorna um status 200 OK
            return Ok(jogo);
        }


        // No POST eu insiro um jogo
        [HttpPost]
        public async Task<ActionResult<object>> InserirJogo([FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                var jogo = await _jogoService.Inserir(jogoInputModel);

                // Rotorna um status 200 OK
                return Ok(jogo);
            }
            catch (JogoJaCadastradoException ex)
            {
                return UnprocessableEntity("Já existe um jogo com este nome para esta produtora");
            }
        }


        // No PUT eu atualizo o jogo inteiro
        [HttpPut]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid idJogo, [FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, jogoInputModel);
                // Rotorna um status 200 OK
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo");
            }
        }


        // No PATCH eu atualizo algo específico do jogo, atualo uma parte só do recurso e não ele inteiro
        [HttpPatch("{idJogo:guid}/preco/{preco:double}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid idJogo, [FromRoute] double preco)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, preco);

                // Rotorna um status 200 OK
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo");
            }
        }


        // No DELETE eu removo um jogo
        [HttpDelete]
        public async Task<ActionResult> ApagarJogo([FromRoute] Guid idJogo)
        {
            try
            {
                await _jogoService.Remover(idJogo);
         
                // Rotorna um status 200 OK
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo");
            }
        }
    }
}
