import { cpfValido, criarMascaraCpf } from './cpf.js';
import { limparErroAoDigitar, limparErros, validarFormulario } from './formulario.js';

export function iniciarBeneficiarios() {
    const elementoModal = document.getElementById('modalBeneficiarios');
    const modal = new bootstrap.Modal(elementoModal);
    const formulario = document.getElementById('formBeneficiario');
    const campoCpf = formulario.elements.BenefCpf;
    const campoNome = formulario.elements.BenefNome;
    const botaoSalvar = document.getElementById('btnIncluirBenef');
    const corpoTabela = document.querySelector('#gridBeneficiarios tbody');
    const mascaraCpf = criarMascaraCpf(campoCpf);

    let beneficiarios = [];
    let indiceEdicao = -1;

    const regras = {
        BenefCpf: valor => {
            if (!cpfValido(valor))
                return 'CPF inválido.';

            return cpfDuplicado(valor) ? 'CPF já incluído para este cliente.' : null;
        }
    };

    function cpfDuplicado(cpf) {
        return beneficiarios.some((b, indice) => indice !== indiceEdicao && b.Cpf === cpf);
    }

    function limparCampos() {
        indiceEdicao = -1;
        formulario.reset();
        mascaraCpf.value = '';
        limparErros(formulario);
        botaoSalvar.textContent = 'Incluir';
    }

    function criarBotao(texto, acao, indice) {
        const botao = document.createElement('button');
        botao.type = 'button';
        botao.className = 'btn btn-primary btn-sm me-1';
        botao.dataset.acao = acao;
        botao.dataset.indice = indice;
        botao.textContent = texto;

        return botao;
    }

    function mostrarBeneficiarios() {
        const linhas = beneficiarios.map((beneficiario, indice) => {
            const linha = document.createElement('tr');
            const celulaCpf = document.createElement('td');
            const celulaNome = document.createElement('td');
            const celulaAcoes = document.createElement('td');

            celulaCpf.textContent = beneficiario.Cpf;
            celulaNome.textContent = beneficiario.Nome;
            celulaAcoes.append(criarBotao('Alterar', 'alterar', indice), criarBotao('Excluir', 'excluir', indice));
            linha.append(celulaCpf, celulaNome, celulaAcoes);

            return linha;
        });

        corpoTabela.replaceChildren(...linhas);
    }

    limparErroAoDigitar(formulario);

    document.getElementById('btnBeneficiarios').addEventListener('click', () => modal.show());
    elementoModal.addEventListener('hidden.bs.modal', limparCampos);

    formulario.addEventListener('submit', evento => {
        evento.preventDefault();

        if (!validarFormulario(formulario, regras))
            return;

        const beneficiario = { Id: 0, Cpf: campoCpf.value, Nome: campoNome.value.trim() };

        if (indiceEdicao >= 0) {
            beneficiario.Id = beneficiarios[indiceEdicao].Id;
            beneficiarios[indiceEdicao] = beneficiario;
        } else {
            beneficiarios.push(beneficiario);
        }

        limparCampos();
        mostrarBeneficiarios();
    });

    corpoTabela.addEventListener('click', evento => {
        const botao = evento.target.closest('button[data-acao]');
        if (!botao)
            return;

        const indice = Number(botao.dataset.indice);

        if (botao.dataset.acao === 'excluir') {
            beneficiarios.splice(indice, 1);
            limparCampos();
            mostrarBeneficiarios();
            return;
        }

        limparCampos();
        indiceEdicao = indice;
        mascaraCpf.value = beneficiarios[indice].Cpf;
        campoNome.value = beneficiarios[indice].Nome;
        botaoSalvar.textContent = 'Atualizar';
    });

    return {
        obter: () => beneficiarios.map(b => ({ ...b })),
        carregar: lista => {
            beneficiarios = lista.map(b => ({ Id: b.Id, Cpf: b.Cpf, Nome: b.Nome }));
            mostrarBeneficiarios();
        },
        limpar: () => {
            beneficiarios = [];
            mostrarBeneficiarios();
        }
    };
}
