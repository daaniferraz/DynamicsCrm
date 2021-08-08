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
                             <attribute name='drf_pedido' />
                             <attribute name='drf_idlead' />
                             <attribute name='drf_nomedocliente' />
                             <attribute name='drf_valordopedido' />
                             <attribute name='drf_formadepagamento' />
                             <attribute name='drf_parcelas' />
                             </entity>
                            </fetch>";

            EntityCollection colecaoFrom = CrmImport.RetrieveMultiple(new FetchExpression(query));

            var conectionTo = new ConexaoCrm().Obter();            
            string nameEntity = "grp3_pedidos";

            foreach (var item in colecaoFrom.Entities)
            {

                var nameError = item["drf_pedido"].ToString();
                try
                {
                    Guid registro = new Guid();

                    string query2 = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='grp3_pedidos'>                                        
                                        <attribute name='grp3_pedido' />
                                        <attribute name='grp3_idlead' />
                                        <attribute name='grp3_nomedocliente' />
                                        <attribute name='grp3_valordopedido' />                                              
                                        <attribute name='grp3_formadepagamento' />                                              
                                        <attribute name='grp3_parcelas' />                                              
                                    <order attribute='grp3_pedido' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='grp3_pedido' operator='eq' value= '{0}'/>                                                                                                              
                                    </filter>
                                </entity>
                            </fetch>";

                    query2 = string.Format(query2, item["drf_pedido"].ToString());

                    EntityCollection col = conectionTo.RetrieveMultiple(new FetchExpression(query2));

                    if (col.Entities.Count == 0)
                    {
                        registro = createEntidade.CreateEntidades(item, nameEntity, conectionTo, registro);
                    }
                }
                catch (Exception ex)
                {                    
                    Guid registro = new Guid();

                    Console.WriteLine("Não foi possível importar o pedido: " + nameError);
                    createEntidade.CreateErrorException(ex, nameEntity, conectionTo, registro);
                    
                }
            }
        }
        #endregion

        #region ItensPedido
        public void ImportarItensPedido(CrmServiceClient CrmImport)
        {
            CreateEntidade createEntidade = new CreateEntidade();

            string queryFromItens = @"<fetch version='1.0' output-format='xml-plataform' mapping='logical' distinct='true' >
                            <entity name='drf_itenspedidos'>
                                <attribute name='drf_pedido' />
                                <attribute name='drf_iditem' />                                
                                <attribute name='drf_nomedoitem' />
                                <attribute name='drf_quantidade' />      
                                <attribute name='drf_unidadedevenda' />
                                <attribute name='drf_valorunitario' />
                                <attribute name='drf_valortotal' /> 
                                <attribute name='drf_seqitemterceiro' />
                             </entity>
                            </fetch>";

            EntityCollection colecaoFrom = CrmImport.RetrieveMultiple(new FetchExpression(queryFromItens));

            var conectionTo = new ConexaoCrm().Obter();

            string nameEntity = "grp3_itenspedidos";

            foreach (var item in colecaoFrom.Entities)
            {

                var nameError = item["drf_pedido"].ToString();
                var itemError = item["drf_nomedoitem"].ToString();
                try
                {
                    var entidade = new Entity("grp3_itenspedidos");

                    Guid registro = new Guid();                    

                    EntityReference newPedido = item.GetAttributeValue<EntityReference>("drf_pedido");
                    string pedidocompare = newPedido.Name;

                    string queryPedido = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='grp3_pedidos'>                                        
                                        <attribute name='grp3_pedido' />
                                        <attribute name='grp3_idlead' />
                                        <attribute name='grp3_nomedocliente' />
                                        <attribute name='grp3_valordopedido' />                                              
                                    <order attribute='grp3_pedido' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='grp3_pedido' operator='eq' value= '{0}'/>                                                                                                              
                                    </filter>
                                </entity>
                            </fetch>";

                    queryPedido = string.Format(queryPedido, pedidocompare.ToString());
                    EntityCollection colectionPedido = conectionTo.RetrieveMultiple(new FetchExpression(queryPedido));
                    
                    if (colectionPedido.Entities.Count == 1)
                    {
                        Guid guidItemPedido;
                        EntityCollection colIntens;

                        string queryItens = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='grp3_itenspedidos'>
                                        <attribute name='grp3_pedido' />
                                        <attribute name='grp3_iditem' />                                
                                        <attribute name='grp3_nomedoitem' />
                                        <attribute name='grp3_quantidade' />      
                                        <attribute name='grp3_unidadedevenda' />
                                        <attribute name='grp3_valorunitario' />
                                        <attribute name='grp3_valortotal' />
                                        <attribute name='grp3_seqitemterceiro' />
                                    <order attribute='grp3_pedido' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='grp3_pedido' operator='eq' value= '{0}'/>                                                                      
                                        <condition attribute='grp3_seqitemterceiro' operator='eq' value= '{1}'/>
                                    </filter>
                                </entity>
                            </fetch>";

                        Entity itemPedido = colectionPedido.Entities[0];
                        guidItemPedido    = itemPedido.Id;

                        queryItens = string.Format(queryItens, guidItemPedido, item["drf_seqitemterceiro"].ToString());

                        colIntens = conectionTo.RetrieveMultiple(new FetchExpression(queryItens));

                        if (colIntens.Entities.Count == 0)
                        {
                            //valida se item existe na tabela produto e cria o item caso não tenha
                            this.CheckImportProdutos(conectionTo, item["drf_iditem"].ToString(), item["drf_nomedoitem"].ToString());

                            createEntidade.getfieldName = "grp3_pedido";
                            createEntidade.getfieldValue = guidItemPedido;

                            registro = createEntidade.CreateEntidades(item, "grp3_itenspedidos", conectionTo, registro);
                        }
                    } 
                }
                catch (Exception ex)
                {
                    Guid registro = new Guid();

                    Console.WriteLine("Não foi possível importar o item "+ itemError+" do pedido: " + nameError);
                    createEntidade.CreateErrorException(ex, nameEntity, conectionTo, registro);
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
            }
            catch (Exception ex)
            {
                Guid registro = new Guid();
                Console.WriteLine("Não foi possível importar o produto: " + nomeItem);
                createEntidade.CreateErrorException(ex, nameEntity, CrmImportTo, registro);
            }
        }
        #endregion
    }
}

