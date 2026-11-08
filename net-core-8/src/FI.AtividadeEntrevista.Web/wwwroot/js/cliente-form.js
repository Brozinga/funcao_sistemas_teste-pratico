import { cpfValido, criarMascaraCpf } from './cpf.js';
import { limparErroAoDigitar, validarFormulario } from './formulario.js';
import { iniciarBeneficiarios } from './beneficiarios.js';
import { mostrarModal } from './modal.js';

const formulario = document.getElementById('formCadastro');
const { urlPost, urlRetorno, id } = formulario.dataset;
const mascaraCpf = criarMascaraCpf(formulario.elements.Cpf);
const beneficiarios = iniciarBeneficiarios();

beneficiarios.carregar(JSON.parse(document.getElementById('dadosBeneficiarios').textContent));
limparErroAoDigitar(formulario);

const regras = {
    Cpf: valor => (cpfValido(valor) ? null : 'CPF inválido.')
};

function montarCliente() {
    return {
        ...Object.fromEntries(new FormData(formulario)),
        Id: Number(id),
        Beneficiarios: beneficiarios.obter()
    };
}

async function enviar() {
    const resposta = await fetch(urlPost, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(montarCliente())
    });

    const corpo = await resposta.json().catch(() => ({}));

    if (resposta.ok) {
        mostrarModal('Sucesso!', corpo.Mensagem, () => {
            if (urlRetorno)
                window.location.href = urlRetorno;
        });

        if (!urlRetorno) {
            formulario.reset();
            mascaraCpf.value = '';
            beneficiarios.limpar();
        }
        return;
    }

    const mensagem = resposta.status === 400 ? corpo.Mensagem : 'Ocorreu um erro interno no servidor.';
    mostrarModal('Ocorreu um erro', mensagem);
}

formulario.addEventListener('submit', async evento => {
    evento.preventDefault();

    if (!validarFormulario(formulario, regras))
        return;

    try {
        await enviar();
    } catch {
        mostrarModal('Ocorreu um erro', 'Não foi possível comunicar com o servidor.');
    }
});
