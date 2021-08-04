using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;

namespace Dynamics_CRM
{
    class ImportacaoPedido
    {
        #region Pedido
        public void ImportarPedido(CrmServiceClient CrmImport)
        {
            CreateEntidade createEntidade = new CreateEntidade();
            string query = @"<fetch version='1.0' output-format='xml-plataform' mapping='logical' distinct='true' >
                            <entity name='drf_pedidos'>                             
                             <attribute name='drf_idpedido' />
                             <attribute name='drf_idlead' />
                             <attribute name='drf_nomedocliente' />
                             <attribute name='drf_valordopedido' />                             
                             </entity>
                            </fetch>";

            EntityCollection colecaoFrom = CrmImport.RetrieveMultiple(new FetchExpression(query));

            var conectionTo = new ConexaoCrm().Obter();
            string nameEntity = "grp3_pedidos";

            foreach (var item in colecaoFrom.Entities)
            {
                try
                {
                    Guid registro = new Guid();

                    string query2 = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='grp3_pedidos'>                                        
                                        <attribute name='grp3_idpedido' />
                                        <attribute name='grp3_idlead' />
                                        <attribute name='grp3_nomedocliente' />
                                        <attribute name='grp3_valordopedido' />      
                                    <order attribute='grp3_idpedido' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='grp3_idpedido' operator='eq' value= '{0}'/>                                                                      
                                    </filter>
                                </entity>
                            </fetch>";

                    query2 = string.Format(query2, item["drf_idpedido"].ToString());

                    EntityCollection col = conectionTo.RetrieveMultiple(new FetchExpression(query2));

                    if (col.Entities.Count == 0)
                    {
                        registro = createEntidade.CreateEntidades(item, nameEntity, conectionTo, registro);
                    }
                }
                catch (Exception ex)
                {                    
                    Guid registro = new Guid();

                    createEntidade.CreateErrorException(ex, nameEntity, conectionTo, registro);
                    
                    /*var entidadeErro = new Entity("grp3_erroimportacao");

                    entidadeErro.Attributes.Add("grp3_nomeentidade", "Pedido");
                    entidadeErro.Attributes.Add("grp3_errogerado", ex.ToString() + " Gerado em: " + Convert.ToDateTime(DateTime.Now).ToString());

                    conectionTo.Create(entidadeErro);
                    Console.WriteLine("Erro gerado e gravado na tabela de erros.");*/
                }
            }
        }
        #endregion

        #region ItensPedido
        public void ImportarItensPedido(CrmServiceClient CrmImport)
        {
            CreateEntidade createEntidade = new CreateEntidade();

            string query = @"<fetch version='1.0' output-format='xml-plataform' mapping='logical' distinct='true' >
                            <entity name='drf_itenspedidos'>
                                <attribute name='drf_idpedido' />
                                <attribute name='drf_iditem' />                                
                                <attribute name='drf_nomedoitem' />
                                <attribute name='drf_quantidade' />      
                                <attribute name='drf_unidadedevenda' />
                                <attribute name='drf_valorunitario' />
                                <attribute name='drf_valortotal' />                              
                             </entity>
                            </fetch>";

            EntityCollection colecaoFrom = CrmImport.RetrieveMultiple(new FetchExpression(query));

            var conectionTo = new ConexaoCrm().Obter();

            string nameEntity = "grp3_itenspedidos";

            foreach (var item in colecaoFrom.Entities)
            {
                try
                {
                    var entidade = new Entity("grp3_itenspedidos");

                    Guid registro = new Guid();

                    string query2 = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='grp3_itenspedidos'>
                                        <attribute name='grp3_idpedido' />
                                        <attribute name='grp3_iditem' />                                
                                        <attribute name='grp3_nomedoitem' />
                                        <attribute name='grp3_quantidade' />      
                                        <attribute name='grp3_unidadedevenda' />
                                        <attribute name='grp3_valorunitario' />
                                        <attribute name='grp3_valortotal' />
                                        <attribute name='grp3_seqitemterceiro' />
                                    <order attribute='grp3_idpedido' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='grp3_idpedido' operator='eq' value= '{0}'/>                                                                      
                                        <condition attribute='grp3_seqitemterceiro' operator='eq' value= '{1}'/>
                                    </filter>
                                </entity>
                            </fetch>";

                    query2 = string.Format(query2, item["drf_idpedido"].ToString(), item["drf_seqitemterceiro"].ToString());

                    EntityCollection col = conectionTo.RetrieveMultiple(new FetchExpression(query2));

                    if (col.Entities.Count == 0)
                    {
                        //valida se item existe na tabela produto e cria o item caso não tenha
                        this.CheckImportProdutos(conectionTo, item["drf_iditem"].ToString(), item["drf_nomedoitem"].ToString());

                        registro = createEntidade.CreateEntidades(item, "grp3_itenspedidos", conectionTo, registro);
                    }
                }
                catch (Exception ex)
                {
                    Guid registro = new Guid();

                    createEntidade.CreateErrorException(ex, nameEntity, conectionTo, registro);

                    /*var entidadeErro = new Entity("grp3_erroimportacao");

                    entidadeErro.Attributes.Add("grp3_nomeentidade", "ItensPedido");
                    entidadeErro.Attributes.Add("grp3_errogerado", ex.ToString() + " Gerado em: " + Convert.ToDateTime(DateTime.Now).ToString());

                    conectionTo.Create(entidadeErro);
                    Console.WriteLine("Erro gerado e gravado na tabela de erros.");*/
                }
            }
        }
        #endregion

        #region Produtos
        public void CheckImportProdutos(CrmServiceClient CrmImportTo, string IdItem, string nomeItem)
        {
            CreateEntidade createEntidade = new CreateEntidade();

            string query = @"<fetch version='1.0' output-format='xml-plataform' mapping='logical' distinct='true' >
                            <entity name='grp3_produtos'>
                                <attribute name='grp3_iditem' />
                                <order attribute='grp3_iditem' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='grp3_iditem' operator='eq' value= '{0}'/>                                                                      
                                    </filter>
                             </entity>
                            </fetch>";

            query = string.Format(query, IdItem);

            EntityCollection colecao = CrmImportTo.RetrieveMultiple(new FetchExpression(query));

            if (colecao.Entities.Count > 0)
            {
                return;
            }

            string nameEntity = "grp3_produtos";

            try
            {

                var entidadeProduto = new Entity(nameEntity);

                entidadeProduto.Attributes.Add("grp3_nomeitem", nomeItem);
                entidadeProduto.Attributes.Add("grp3_iditem", Convert.ToInt32(IdItem));

                CrmImportTo.Create(entidadeProduto);
                Console.WriteLine("Item criado na tabela Produtos: " + nomeItem);
            }
            catch (Exception ex)
            {
                Guid registro = new Guid();

                createEntidade.CreateErrorException(ex, nameEntity, CrmImportTo, registro);

                /*var entidadeErro = new Entity("grp3_erroimportacao");
                entidadeErro.Attributes.Add("grp3_nomeentidade", "ItensPedido");
                entidadeErro.Attributes.Add("grp3_errogerado", ex.ToString() + " Gerado em: " + Convert.ToDateTime(DateTime.Now).ToString());

                CrmImportTo.Create(entidadeErro);
                Console.WriteLine("Erro gerado e gravado na tabela de erros.");*/
            }
        }
        #endregion
    }
}

