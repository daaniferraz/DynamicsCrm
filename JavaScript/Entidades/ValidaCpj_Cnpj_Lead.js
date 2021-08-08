// VALIDA CPF E CNPJ DO LEAD 

function OnChange(executionContext) {

    debugger;

    var formularioContexto = executionContext.getFormContext();

    var cpfFieldControl = formularioContexto.getControl('grp3_cpf');
    var cnpjFieldControl = formularioContexto.getControl('grp3_cnpj');

    formularioContexto.getControl('grp3_cpf').clearNotification();
    formularioContexto.getControl('grp3_cnpj').clearNotification();

    if ((formularioContexto.getControl("grp3_cpf").getVisible())) {

        var cpf = formularioContexto.getAttribute("grp3_cpf").getValue();
        if (cpf === null) {
            var cpfFieldControl = formularioContexto.getControl('grp3_cpf');
            cpfFieldControl.setNotification("Inserir CPF.");
            return;
        }

        var valida = TestaCPF(cpf);

        if (valida === false) {

            var cpfFieldControl = formularioContexto.getControl('grp3_cpf');
            formularioContexto.getControl('grp3_cpf').clearNotification();
            cpfFieldControl.setNotification("CPF inválido!.", "ERROR", "2");
            return;


        }
        else {
            formularioContexto.getControl('grp3_cpf').clearNotification();
        }

        var fetchXml = '?fetchXml=<fetch version="1.0" output-format="xml-platform" mapping="logical" distinct="false">' +
            '<entity name="grp3_clientepotenciallead">' +
            '<attribute name="grp3_clientepotencialleadid" />' +
            '<attribute name="grp3_idlead" />' +
            '<attribute name="createdon" />' +
            '<order attribute="grp3_idlead" descending="false" />' +
            '<filter type="and">' +
            '<condition attribute="grp3_cpf" operator="eq" value="' + cpf + '" />' +
            '</filter>' +
            '</entity>' +
            '</fetch>';

        Xrm.WebApi.retrieveMultipleRecords("grp3_clientepotenciallead", fetchXml).then(
            function success(result) {
                if (result.entities.length > 0) {
                    var cpfFieldControl = formularioContexto.getControl('grp3_cpf');

                    cpfFieldControl.setNotification("CPF já cadastrado!");
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
    else {
        var cnpj = formularioContexto.getAttribute("grp3_cnpj").getValue();

        if (cnpj === null) {
            var cnpjFieldControl = formularioContexto.getControl('grp3_cnpj');
            cnpjFieldControl.setNotification("Inserir CNPJ.");
            return;
        }

        var valida = validarCNPJ(cnpj);

        if (valida === false) {

            var cnpjFieldControl = formularioContexto.getControl('grp3_cnpj');
            formularioContexto.getControl('grp3_cnpj').clearNotification();
            cnpjFieldControl.setNotification("CNPJ inválido!.", "ERROR", "2");
            return;


        }
        else {
            formularioContexto.getControl('grp3_cnpj').clearNotification();
        }


        // Filtro realizando busca na base de dados para verificar se já existe o CNPJ na base de dados 
        var fetchXml = '?fetchXml=<fetch version="1.0" output-format="xml-platform" mapping="logical" distinct="false">' +
            '<entity name="grp3_clientepotenciallead">' +
            '<attribute name="grp3_clientepotencialleadid" />' +
            '<attribute name="grp3_idlead" />' +
            '<attribute name="createdon" />' +
            '<order attribute="grp3_idlead" descending="false" />' +
            '<filter type="and">' +
            '<condition attribute="grp3_cnpj" operator="eq" value="' + cnpj + '" />' +
            '</filter>' +
            '</entity>' +
            '</fetch>';

        //Chamada a API enviando como arguentos a entidade e o filtro para consulta
        Xrm.WebApi.retrieveMultipleRecords("grp3_clientepotenciallead", fetchXml).then(
            function success(result) {

                // Se o result trazer algum dado na entidade significa que existe o dado. É informado que o dado já está cadastrado
                if (result.entities.length > 0) {
                    var cnpjFieldControl = formularioContexto.getControl('grp3_cnpj');

                    cnpjFieldControl.setNotification("CNPJ já cadastrado!");
                }

                // Apaga a notificação caso o campo seja alterado para um dado válido
                else {
                    debugger;
                    formularioContexto.getControl('grp3_cnpj').clearNotification();
                }
            },
            function (error) {
                console.log(error.message);

            }
        );
    }


}


function TestaCPF(strCPF) {
    var Soma;
    var Resto;
    Soma = 0;
    if (strCPF == "00000000000" ||
        strCPF == "11111111111" ||
        strCPF == "22222222222" ||
        strCPF == "33333333333" ||
        strCPF == "44444444444" ||
        strCPF == "55555555555" ||
        strCPF == "66666666666" ||
        strCPF == "77777777777" ||
        strCPF == "88888888888" ||
        strCPF == "99999999999") return false;

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
