var beneficiarios = [];     
var indiceAlteracao = -1; 

$.validator.addMethod("cpfNovo", function (value) {
    for (var i = 0; i < beneficiarios.length; i++) {
        if (i != indiceAlteracao && beneficiarios[i].CPF == value)
            return false;
    }
    return true;
}, "CPF já incluído para este cliente.");

$(document).ready(function () {

    $("#BenefCPF").mask("000.000.000-00");

    $('#btnBeneficiarios').click(function () {
        $('#modalBeneficiarios').modal('show');
    });

    $('#modalBeneficiarios').on('hidden.bs.modal', function () {
        LimparCamposBeneficiario();
    });

    $('#formBeneficiario').validate({
        rules: {
            BenefCPF: { required: true, cpf: true, cpfNovo: true },
            BenefNome: {
                required: true,
                minlength: 3,
                normalizer: function (value) { return $.trim(value); }
            }
        },
        submitHandler: function () {
            var beneficiario = { Id: 0, CPF: $('#BenefCPF').val(), Nome: $.trim($('#BenefNome').val()) };

            if (indiceAlteracao >= 0) {
                beneficiario.Id = beneficiarios[indiceAlteracao].Id;
                beneficiarios[indiceAlteracao] = beneficiario;
            }
            else
                beneficiarios.push(beneficiario);

            LimparCamposBeneficiario();
            MostrarBeneficiarios();
        }
    });

    $('#gridBeneficiarios').on('click', '.btn-alterar', function () {
        indiceAlteracao = $(this).data('indice');
        $('#BenefCPF').val(beneficiarios[indiceAlteracao].CPF);
        $('#BenefNome').val(beneficiarios[indiceAlteracao].Nome);
        $('#btnIncluirBenef').text('Atualizar');
    });

    $('#gridBeneficiarios').on('click', '.btn-excluir', function () {
        beneficiarios.splice($(this).data('indice'), 1);
        LimparCamposBeneficiario();
        MostrarBeneficiarios();
    });
})

function CarregarBeneficiarios(lista) {
    lista = lista || [];
    beneficiarios = [];
    for (var i = 0; i < lista.length; i++)
        beneficiarios.push({ Id: lista[i].Id, CPF: lista[i].CPF, Nome: lista[i].Nome });

    MostrarBeneficiarios();
}

function LimparCamposBeneficiario() {
    indiceAlteracao = -1;
    $('#formBeneficiario')[0].reset();
    $('#formBeneficiario').validate().resetForm();
    $('#formBeneficiario .form-group').removeClass('has-error');
    $('#btnIncluirBenef').text('Incluir');
}

function MostrarBeneficiarios() {
    var linhas = '';

    for (var i = 0; i < beneficiarios.length; i++) {
        linhas += '<tr>' +
            '<td>' + $('<div>').text(beneficiarios[i].CPF).html() + '</td>' +
            '<td>' + $('<div>').text(beneficiarios[i].Nome).html() + '</td>' +
            '<td>' +
            '<button type="button" class="btn btn-primary btn-sm btn-alterar" data-indice="' + i + '">Alterar</button> ' +
            '<button type="button" class="btn btn-primary btn-sm btn-excluir" data-indice="' + i + '">Excluir</button>' +
            '</td>' +
            '</tr>';
    }

    $('#gridBeneficiarios tbody').html(linhas);
}
