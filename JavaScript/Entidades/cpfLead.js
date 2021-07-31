// VALIDAR CPF DULICADO CLIENTE POTENCIAL LEAD


function OnChange(executionContext) {

    debugger;

    // Recupera contexto do formulário
    var formularioContexto = executionContext.getFormContext();

    // Guarda valor do CPF digitado na tela em uma variável
    var cpf = formularioContexto.getAttribute("grp3_cpf").getValue();

    //Compara se o campo está nulo após alguma edição, em caso positivo solicita inserir dado
    if (cpf === null) {
        var cnpjFieldControl = formularioContexto.getControl('grp3_cpf');
        cnpjFieldControl.setNotification("Inserir CPF.");
        return;
    }

    // Filtro realizando busca na base de dados para verificar se já existe o CPF na base de dados 
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

    //Chamada a API enviando como arguentos a entidade e o filtro para consulta
    Xrm.WebApi.retrieveMultipleRecords("grp3_clientepotenciallead", fetchXml).then(
        function success(result) {

            // Se o result trazer algum dado na entidade significa que existe o dado. É notificado que o dado já está cadastrado
            if (result.entities.length > 0) {
                var cnpjFieldControl = formularioContexto.getControl('grp3_cpf');

                cnpjFieldControl.setNotification("CPF já cadastrado!");
            }

            // Apaga a notificação caso o campo seja alterado para um dado válido
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
