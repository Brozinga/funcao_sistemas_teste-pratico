$.extend($.validator.messages, {
    required: "Campo obrigatório.",
    minlength: $.validator.format("Digite no mínimo {0} letras.")
});

$.validator.addMethod("cpf", function (value, element) {
    if (this.optional(element))
        return true;

    if (value == "123.456.789-09")
        return false;

    return $.validator.methods.cpfBR.call(this, value, element);
}, "CPF inválido.");

$.validator.setDefaults({
    errorElement: "span",
    errorClass: "help-block",
    highlight: function (element) {
        $(element).closest(".form-group").addClass("has-error");
    },
    unhighlight: function (element) {
        $(element).closest(".form-group").removeClass("has-error");
    },
    errorPlacement: function (error, element) {
        error.insertAfter(element);
    }
});

$(document).on("submit", "#formCadastro", function (e) {
    e.preventDefault();
});

$(document).ready(function () {
    $("#CPF").mask("000.000.000-00");
});
