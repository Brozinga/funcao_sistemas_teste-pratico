const CPF_PROIBIDO = '12345678909';

export function cpfValido(valor) {
    const cpf = String(valor ?? '').trim();

    if (!/^(\d{3}\.\d{3}\.\d{3}-\d{2}|\d{11})$/.test(cpf))
        return false;

    const numeros = cpf.replace(/\D/g, '');

    if (numeros === CPF_PROIBIDO || /^(\d)\1{10}$/.test(numeros))
        return false;

    const primeiroDigito = calcularDigito(numeros.slice(0, 9), 10);
    const segundoDigito = calcularDigito(numeros.slice(0, 9) + primeiroDigito, 11);

    return numeros.endsWith(`${primeiroDigito}${segundoDigito}`);
}

export function criarMascaraCpf(campo) {
    return IMask(campo, { mask: '000.000.000-00' });
}

function calcularDigito(baseCalculo, pesoInicial) {
    let soma = 0;

    for (let i = 0; i < baseCalculo.length; i++)
        soma += Number(baseCalculo[i]) * (pesoInicial - i);

    const resto = soma % 11;

    return resto < 2 ? 0 : 11 - resto;
}
