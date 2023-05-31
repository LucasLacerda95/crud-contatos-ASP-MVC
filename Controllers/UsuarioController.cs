using ControleDeContatos.Filters;
using ControleDeContatos.Models;
using ControleDeContatos.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers {

    [PaginaRestritaSomenteAdmin]//Filtro, acessa a controler apenas com sessao logada e também ter permissão de admin
    public class UsuarioController : Controller {

        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository) {
            _usuarioRepository = usuarioRepository;
        }


        public IActionResult Criar() {
            return View();
        }

        public IActionResult Editar(int id) {

            UsuarioModel usuario = _usuarioRepository.ListarPorId(id);
            return View(usuario);
        }

        public IActionResult Index() {
            List<UsuarioModel> usuarios = _usuarioRepository.BuscarTodos();

            return View(usuarios);
        }

        public IActionResult ApagarConfirmacao(int id) {

            UsuarioModel usuario = _usuarioRepository.ListarPorId(id);

            return View(usuario);
        }


        public IActionResult Apagar(int id) {

            try {

                bool apagado = _usuarioRepository.Apagar(id);
                if (apagado) {
                    TempData["MensagemSucesso"] = "Usuario apagado com sucesso !";
                } else {
                    TempData["MensagemErro"] = "Ops, não conseguimos apagar o seu usuario !";
                }
                return RedirectToAction("Index");

            } catch (Exception erro) {

                TempData["MensagemErro"] = $"Ops, não conseguimos apagar o seu contato !, detalhes do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario) {

            try {

                if (ModelState.IsValid) {//valida o Data notations que colocamos no model
                    _usuarioRepository.Adicionar(usuario);
                    TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso";
                    return RedirectToAction("Index");
                }

                return View(usuario);

            } catch (Exception erro) {

                TempData["MensagemErro"] = $"Ops, não conseguimos cadastrar seu usuário, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Alterar(UsuarioSemSenhaModel usuarioSemSenhaModel) {

            try
            {
                UsuarioModel usuario = null;

                if (ModelState.IsValid) {

                    usuario = new UsuarioModel() 
                    {
                        Id = usuarioSemSenhaModel.Id,
                        Nome = usuarioSemSenhaModel.Nome,
                        Login = usuarioSemSenhaModel.Login,
                        Email = usuarioSemSenhaModel.Email,
                        Perfil = usuarioSemSenhaModel.Perfil
                    };

                    usuario = _usuarioRepository.Atualizar(usuario);
                    TempData["MensagemSucesso"] = "Usuário foi alterado com sucesso";
                    return RedirectToAction("Index");
                }

                return View("Editar", usuario);

            } catch (Exception erro) {

                TempData["MensagemErro"] = $"Ops, não conseguimos atualizar o seu usuário, tente novamente, detalhe do erro: {erro.Message}";
                return RedirectToAction("Index");
            }
        }

    }
}
