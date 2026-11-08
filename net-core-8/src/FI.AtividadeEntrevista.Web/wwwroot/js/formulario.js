export function limparErro(campo) {
    campo.classList.remove('is-invalid');
    campo.parentElement.querySelector(':scope > .invalid-feedback')?.remove();
}

export function limparErros(formulario) {
    formulario.querySelectorAll('.is-invalid').forEach(limparErro);
}

export function limparErroAoDigitar(formulario) {
    formulario.addEventListener('input', evento => limparErro(evento.target));
    formulario.addEventListener('change', evento => limparErro(evento.target));
}

export function validarFormulario(formulario, regras = {}) {
    let valido = true;

    for (const campo of formulario.querySelectorAll('input[name], select[name]')) {
        const mensagem = mensagemDoCampo(campo, regras[campo.name]);

        if (mensagem) {
            mostrarErro(campo, mensagem);
            valido = false;
        } else {
            limparErro(campo);
        }
    }

    return valido;
}

function mostrarErro(campo, mensagem) {
    limparErro(campo);

    const aviso = document.createElement('div');
    aviso.className = 'invalid-feedback';
    aviso.textContent = mensagem;

    campo.classList.add('is-invalid');
    campo.after(aviso);
}

function mensagemDoCampo(campo, regra) {
    const valor = campo.value.trim();

    if (campo.required && !valor)
        return 'Campo obrigatório.';

    if (!valor)
        return null;

    if (campo.validity.typeMismatch)
        return 'Digite um e-mail válido.';

    if (campo.minLength > 0 && valor.length < campo.minLength)
        return `Digite no mínimo ${campo.minLength} letras.`;

    return regra ? regra(valor) : null;
}
