// VALIDA CPF

function OnChange(executionContext) {

    debugger;

    var formularioContexto = executionContext.getFormContext();

    var cpf = formularioContexto.getAttribute("grp3_cpf").getValue();
    if (cpf === null) {
        var cnpjFieldControl = formularioContexto.getControl('grp3_cpf');
        cnpjFieldControl.setNotification("Inserir CPF.");
        return;
    }


    var valida = TestaCPF(cpf);

    if (valida === false) {

        var cnpjFieldControl = formularioContexto.getControl('grp3_cpf');
        formularioContexto.getControl('grp3_cpf').clearNotification();
        cnpjFieldControl.setNotification("CPF inválido!.", "ERROR", "2");
        return;


    }
    else {
        formularioContexto.getControl('grp3_cpf').clearNotification();
    }


    var fetchXml = '?fetchXml=<fetch version="1.0" output-format="xml-platform" mapping="logical" distinct="false">' +
        '<entity name="contact">' +
        '<attribute name="fullname" />' +
        '<attribute name="telephone1" />' +
        '<attribute name="contactid" />' +
        '<order attribute="fullname" descending="false" />' +
        '<filter type="and">' +
        '<condition attribute="grp3_cpf" operator="eq" value="' + cpf + '" />' +
        '</filter>' +
        '</entity>' +
        '</fetch>';

    Xrm.WebApi.retrieveMultipleRecords("contact", fetchXml).then(
        function success(result) {
            if (result.entities.length > 0) {
                var cnpjFieldControl = formularioContexto.getControl('grp3_cpf');

                cnpjFieldControl.setNotification("CPF já cadastrado!");
            }
            else {
                debugger;
                formularioContexto.getControl('grp3_cpf').clearNotification();
            }
        },
        function (error) {
            console.log(error.message);

        }
    );
}

function TestaCPF(strCPF) {
    var Soma;
    var Resto;
    Soma = 0;
    if (strCPF == "00000000000") return false;

    for (i = 1; i <= 9; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i);
    Resto = (Soma * 10) % 11;

    if ((Resto == 10) || (Resto == 11)) Resto = 0;
    if (Resto != parseInt(strCPF.substring(9, 10))) return false;

    Soma = 0;
    for (i = 1; i <= 10; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i);
    Resto = (Soma * 10) % 11;

    if ((Resto == 10) || (Resto == 11)) Resto = 0;
    if (Resto != parseInt(strCPF.substring(10, 11))) return false;
    return true;
}


