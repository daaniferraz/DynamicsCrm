// Arquivo: cnpjLead.js

function OnChange(executionContext) {

    debugger;

    var formularioContexto = executionContext.getFormContext();

    var cnpj = formularioContexto.getAttribute("grp3_cnpj").getValue();
    if (cnpj === null) {
        var cnpjFieldControl = formularioContexto.getControl('grp3_cnpj');
        cnpjFieldControl.setNotification("Inserir CNPJ.");
        return;
    }

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

    Xrm.WebApi.retrieveMultipleRecords("grp3_clientepotenciallead", fetchXml).then(
        function success(result) {
            if (result.entities.length > 0) {
                var cnpjFieldControl = formularioContexto.getControl('grp3_cnpj');

                cnpjFieldControl.setNotification("CNPJ já cadastrado!");
            }
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