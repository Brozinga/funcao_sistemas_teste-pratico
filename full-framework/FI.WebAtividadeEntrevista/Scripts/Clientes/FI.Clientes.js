$(document).ready(function () {
    $('#formCadastro').validate({
        rules: {
            CPF: { cpf: true }
        },
        submitHandler: function (form) {
            $.ajax({
                url: urlPost,
                method: "POST",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({
                    "Nome": $(form).find("#Nome").val(),
                    "CPF": $(form).find("#CPF").val(),
                    "CEP": $(form).find("#CEP").val(),
                    "Email": $(form).find("#Email").val(),
                    "Sobrenome": $(form).find("#Sobrenome").val(),
                    "Nacionalidade": $(form).find("#Nacionalidade").val(),
                    "Estado": $(form).find("#Estado").val(),
                    "Cidade": $(form).find("#Cidade").val(),
                    "Logradouro": $(form).find("#Logradouro").val(),
                    "Telefone": $(form).find("#Telefone").val(),
                    "Beneficiarios": beneficiarios
                }),
                error:
                    function (r) {
                        if (r.status == 400)
                            ModalDialog("Ocorreu um erro", r.responseJSON);
                        else if (r.status == 500)
                            ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                    },
                success:
                    function (r) {
                        ModalDialog("Sucesso!", r)
                        $("#formCadastro")[0].reset();
                        CarregarBeneficiarios([]);
                    }
            });
        }
    });

})

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}