function OnChange(executionContext) {

    debugger;

    // Recupera contexto do formulário
    var formularioContexto = executionContext.getFormContext();

    // Guarda valor do cnpj digitado na tela em uma variável
    var cnpj = formularioContexto.getAttribute("grp3_cpfcnpj").getValue();

    //Compara se o campo está nulo após alguma edição, em caso positivo solicita inserir dado
    if (cnpj === null) {
        var cnpjFieldControl = formularioContexto.getControl('grp3_cpfcnpj');
        cnpjFieldControl.setNotification("Inserir CNPJ.");
        return;
    }


    var valida = validarCNPJ(cnpj);

    if (valida === false) {

        var cnpjFieldControl = formularioContexto.getControl('grp3_cpfcnpj');
        formularioContexto.getControl('grp3_cpfcnpj').clearNotification();
        cnpjFieldControl.setNotification("CNPJ inválido!.", "ERROR", "2");
        return;


    }
    else {
        formularioContexto.getControl('grp3_cpfcnpj').clearNotification();
    }

    // Filtro realizando busca na base de dados para verificar se já existe o CNPJ na base de dados 
    var fetchXml = '?fetchXml=<fetch version="1.0" output-format="xml-platform" mapping="logical" distinct="false">' +
        '<entity name="account">' +
        '<attribute name="name" />' +
        '<attribute name="telephone1" />' +
        '<attribute name="address1_city" />' +
        '<attribute name="primarycontactid" />' +
        '<attribute name="statecode" />' +
        '<filter type="and">' +
        '<condition attribute="grp3_cpfcnpj" operator="eq" value="' + cnpj + '" />' +
        '</filter>' +
        '<link-entity name="contact" from="contactid" to="primarycontactid" visible="false" link-type="outer" alias="accountprimarycontactidcontactcontactid">' +
        '<attribute name="emailaddress1" />' +
        '</link-entity>' +
        '</entity>' +
        '</fetch>';

    //Chamada a API enviando como arguentos a entidade e o filtro para consulta
    Xrm.WebApi.retrieveMultipleRecords("account", fetchXml).then(
        function success(result) {

            // Se o result trazer algum dado na entidade significa que existe o dado. É informado que o dado já está cadastrado
            if (result.entities.length > 0) {
                var cnpjFieldControl = formularioContexto.getControl('grp3_cpfcnpj');

                cnpjFieldControl.setNotification("CNPJ já cadastrado!");
            }

            // Apaga a notificação caso o campo seja alterado para um dado válido
            else {
                debugger;
                formularioContexto.getControl('grp3_cpfcnpj').clearNotification();
            }
        },
        function (error) {
            console.log(error.message);

        }
    );
}

function validarCNPJ(cnpj) {

    cnpj = cnpj.replace(/[^\d]+/g, '');

    if (cnpj == '') return false;

    if (cnpj.length != 14)
        return false;

    // Elimina CNPJs invalidos conhecidos
    if (cnpj == "00000000000000" ||
        cnpj == "11111111111111" ||
        cnpj == "22222222222222" ||
        cnpj == "33333333333333" ||
        cnpj == "44444444444444" ||
        cnpj == "55555555555555" ||
        cnpj == "66666666666666" ||
        cnpj == "77777777777777" ||
        cnpj == "88888888888888" ||
        cnpj == "99999999999999")
        return false;

    // Valida DVs
    tamanho = cnpj.length - 2
    numeros = cnpj.substring(0, tamanho);
    digitos = cnpj.substring(tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(0))
        return false;

    tamanho = tamanho + 1;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(1))
        return false;

    return true;

}