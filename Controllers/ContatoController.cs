using ControleDeContatos.Filters;
using ControleDeContatos.Models;
using ControleDeContatos.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers {

    [PaginaParaUsuarioLogado]//Filtro, acessa a controler apenas com sessao logada
    public class ContatoController : Controller {

        private readonly IContatoRepository _contatoRepository;

        public ContatoController(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }


        public IActionResult Index() {
            List<ContatoModel> contatos = _contatoRepository.BuscarTodos();

            return View(contatos);
        }

        public IActionResult Criar() {
            return View();
        }

        public IActionResult Editar(int id) {

           ContatoModel contato = _contatoRepository.ListarPorId(id);

           return View(contato);
        }

        public IActionResult ApagarConfirmacao(int id) {

            ContatoModel contato = _contatoRepository.ListarPorId(id);

            return View(contato);
        }


        public IActionResult Apagar(int id) {

            try {

                bool apagado = _contatoRepository.Apagar(id);
                if (apagado) {
                    TempData["MensagemSucesso"] = "Contato apagado com sucesso !";
                } else {
                    TempData["MensagemErro"] = "Ops, não conseguimos apagar o seu contato !";
                }
                return RedirectToAction("Index");

            } catch (Exception erro) {

                TempData["MensagemErro"] = $"Ops, não conseguimos apagar o seu contato !, detalhes do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public IActionResult Criar(ContatoModel contato) {

            try {

                if (ModelState.IsValid) {//valida o Data notations que colocamos no model
                    _contatoRepository.Adicionar(contato);
                    TempData["MensagemSucesso"] = "Contato cadastrado com sucesso";
                    return RedirectToAction("Index");
                }

                return View(contato);

            } catch (Exception erro) {

                TempData["MensagemErro"] = $"Ops, não conseguimos cadastrar seu contato, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Alterar(ContatoModel contato) {

            try {

                if (ModelState.IsValid) {
                    _contatoRepository.Atualizar(contato);
                    TempData["MensagemSucesso"] = "Contato alterado com sucesso";
                    return RedirectToAction("Index");
                }

                return View("Editar", contato);

            } catch (Exception erro) {

                TempData["MensagemErro"] = $"Ops, não conseguimos atualizar o seu contato, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
