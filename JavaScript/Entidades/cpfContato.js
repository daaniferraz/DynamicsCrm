 //Arquivo: cpfContato.js
function OnChange(executionContext) {

    debugger;

    var formularioContexto = executionContext.getFormContext();

    var cpf = formularioContexto.getAttribute("grp3_cpf").getValue();
    if (cpf === null) {
        var cnpjFieldControl = formularioContexto.getControl('grp3_cpf');
        cnpjFieldControl.setNotification("Inserir CPF.");
        return;
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