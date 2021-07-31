// CORREÇÃO CNPJ EM CONTA

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
