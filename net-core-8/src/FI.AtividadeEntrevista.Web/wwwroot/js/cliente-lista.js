const TAMANHO_PAGINA = 5;
const PAGINAS_VISIVEIS = 5;

const container = document.getElementById('listaClientes');
const { urlLista, urlAlteracao } = container.dataset;
const estado = { pagina: 1, campo: 'Nome', crescente: true };

function criar(tag, classe, texto) {
    const elemento = document.createElement(tag);
    if (classe)
        elemento.className = classe;
    if (texto !== undefined)
        elemento.textContent = texto;

    return elemento;
}

function criarCabecalho(titulo, campo) {
    const celula = criar('th', null, titulo);

    if (!campo)
        return celula;

    celula.dataset.campo = campo;
    if (estado.campo === campo)
        celula.textContent += estado.crescente ? ' ▲' : ' ▼';

    celula.addEventListener('click', () => {
        estado.crescente = estado.campo === campo ? !estado.crescente : true;
        estado.campo = campo;
        estado.pagina = 1;
        carregar();
    });

    return celula;
}

function criarLinha(cliente) {
    const linha = criar('tr');
    const celulaAcoes = criar('td', 'text-end');
    const botao = criar('a', 'btn btn-primary btn-sm', 'Alterar');
    botao.href = `${urlAlteracao}/${cliente.Id}`;
    celulaAcoes.append(botao);

    linha.append(criar('td', null, cliente.Nome), criar('td', null, cliente.Cpf), criar('td', null, cliente.Email), celulaAcoes);

    return linha;
}

function criarItemPagina(texto, pagina, desabilitado, ativo) {
    const item = criar('li', 'page-item');
    const link = criar('button', 'page-link', texto);
    link.type = 'button';

    if (desabilitado)
        item.classList.add('disabled');
    if (ativo)
        item.classList.add('active');

    link.addEventListener('click', () => {
        estado.pagina = pagina;
        carregar();
    });

    item.append(link);

    return item;
}

function criarPaginacao(total) {
    const totalPaginas = Math.ceil(total / TAMANHO_PAGINA);
    const inicio = Math.max(1, Math.min(estado.pagina - Math.floor(PAGINAS_VISIVEIS / 2), totalPaginas - PAGINAS_VISIVEIS + 1));
    const fim = Math.min(totalPaginas, inicio + PAGINAS_VISIVEIS - 1);

    const lista = criar('ul', 'pagination justify-content-center');
    lista.append(criarItemPagina('Anterior', estado.pagina - 1, estado.pagina === 1));

    for (let pagina = inicio; pagina <= fim; pagina++)
        lista.append(criarItemPagina(String(pagina), pagina, false, pagina === estado.pagina));

    lista.append(criarItemPagina('Próxima', estado.pagina + 1, estado.pagina === totalPaginas));

    const navegacao = criar('nav');
    navegacao.setAttribute('aria-label', 'Paginação');
    navegacao.append(lista);

    return navegacao;
}

function desenhar(dados) {
    if (dados.Total === 0) {
        container.replaceChildren(criar('div', 'alert alert-info', 'Nenhum cliente cadastrado.'));
        return;
    }

    const cabecalho = criar('tr');
    cabecalho.append(criarCabecalho('Nome', 'Nome'), criarCabecalho('CPF'), criarCabecalho('Email', 'Email'), criar('th'));

    const tabela = criar('table', 'table table-hover align-middle');
    tabela.append(criar('thead'), criar('tbody'));
    tabela.tHead.append(cabecalho);
    tabela.tBodies[0].append(...dados.Registros.map(criarLinha));

    container.replaceChildren(tabela, criarPaginacao(dados.Total));
}

async function carregar() {
    const parametros = new URLSearchParams({
        iniciarEm: (estado.pagina - 1) * TAMANHO_PAGINA,
        quantidade: TAMANHO_PAGINA,
        ordenacao: estado.campo,
        crescente: estado.crescente
    });

    try {
        const resposta = await fetch(`${urlLista}?${parametros}`);
        if (!resposta.ok)
            throw new Error();

        desenhar(await resposta.json());
    } catch {
        container.replaceChildren(criar('div', 'alert alert-danger', 'Não foi possível carregar os clientes.'));
    }
}

carregar();
