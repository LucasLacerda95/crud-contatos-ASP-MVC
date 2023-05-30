using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using ControleDeContatos.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers {
    public class LoginController : Controller {

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ISessao _sessao;

        
        public LoginController(IUsuarioRepository usuarioRepository, ISessao sessao) {

            _usuarioRepository = usuarioRepository;
            _sessao = sessao;  
        }

        public IActionResult Index() {
            //Se o usuário já estiver logado, redirecionar para a home


            if (_sessao.BuscarSessaoDoUsuario() != null) return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult Sair() {

            _sessao.RemoverSessaoDoUsuario();

            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel) {

            try {

                if (ModelState.IsValid) {

                    UsuarioModel usuario = _usuarioRepository.BuscarPorLogin(loginModel.Login);

                    if (usuario != null)
                    {
                        if (usuario.SenhaValida(loginModel.Senha)) {

                            _sessao.CriarSessaoDoUsuario(usuario);
                            return RedirectToAction("Index", "Home");   
                        }

                        TempData["MensagemErro"] = $"A senha do usuário é inválida. Por favor, tente novamente.";
                    }

                    TempData["MensagemErro"] = $"Usuário e/ou senha inválido(s). Por favor, tente novamente.";
                }

                return View("Index");
            } catch (Exception erro) {

                TempData["MensagemErro"] = $"Ops, não conseguimos realizar seu login, tente novamente ! detalhes do erro: {erro.Message}";
                return RedirectToAction("Index");

                throw;
            }
        }
    }
}
