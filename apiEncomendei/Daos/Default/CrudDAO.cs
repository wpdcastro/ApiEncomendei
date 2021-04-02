using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppDomain.Entitys;
using AppDomain.Entitys.Default;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace apiEncomendei.Daos.Default
{
    public abstract class CrudDAO<TEntity> where TEntity : Entity, new()
    {
        /// <summary>
        /// Contexto de banco de dados
        /// </summary>
        public Context Db { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<TEntity> DbSet { get; set; }

        public CrudDAO(Context context = null, string cnxStr = null)
        {

            if (context != null)
            {
                Db = context;
            }
            else if (string.IsNullOrEmpty(cnxStr))
            {
                throw new Exception("Database not found.");
            }
            else
            {
                //Db = new Context();
                Db = new Context(cnxStr);
            }
            DbSet = Db.Set<TEntity>();
            Db.Database.Log = query => Debug.Write(query);


        }

        public virtual List<TEntity> ListAll()
        {
            return DbSet.Where(z => !z.Removido).ToList();
        }

        /// <summary>
        /// ID do suporte na Tabela Acessos. => 4
        /// </summary>
        /// <returns></returns>
        public virtual List<TEntity> ListAllExceptionSuport()
        {
            return DbSet.Where(z => !z.Removido && z.Id != 4).ToList();
        }

        public virtual void SaveChanges()
        {
            try
            {
                bool saveFailed;
                int count = 0;
                do
                {
                    saveFailed = false;
                    count++;
                    try
                    {
                        Db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        saveFailed = true;

                        // Get the current entity values and the values in the database
                        // as instances of the entity type
                        var entry = ex.Entries.Single();
                        var databaseValues = entry.GetDatabaseValues();
                        var databaseValuesAsTEntity = (TEntity)databaseValues.ToObject();

                        // Choose an initial set of resolved values. In this case we
                        // make the default be the values currently in the database.
                        var resolvedValuesAsTEntity = (TEntity)databaseValues.ToObject();




                        // Update the original values with the database values and
                        // the current values with whatever the user choose.
                        entry.OriginalValues.SetValues(databaseValues);
                        entry.CurrentValues.SetValues(resolvedValuesAsTEntity);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }

                } while (saveFailed && count < 3);
                if (count > 3 && saveFailed)
                {
                    throw new Exception("Erro ao Comparar Versões");
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entidade do tipo \"{0}\" no estado \"{1}\" tem os seguintes erros de validação:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Erro: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        /// <summary>
        ///     Disconecta do contexto do banco de dados em questão.
        /// </summary>
        public virtual void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Remove objeto do contexto de monitoramento.
        /// </summary>
        /// <param name="obj">Objeto a ser retirado do contexto</param>
        public virtual void DetachedObject(TEntity obj)
        {
            var attachedEntity = Db.ChangeTracker.Entries<TEntity>().FirstOrDefault(e => e.Entity.Id == obj.Id);
            if (attachedEntity != null)
            {
                Db.Entry<TEntity>(attachedEntity.Entity).State = EntityState.Detached;
                GC.Collect();
            }
        }

        /// <summary>
        ///     Realiza o comando Count no Contexto do banco de dados.
        /// </summary>
        /// <param name="filtro">
        ///     Expressão lambda que filtra a lista antes de realizar o Count.
        /// </param>
        /// <returns>
        ///     Número de itens presentes na tabela.
        /// </returns>
        public int Count(Expression<Func<TEntity, bool>> filtro)
        {
            if (filtro == null)
            {
                return DbSet.Count();
            }
            else
            {
                return DbSet.Where(z => !z.Removido).Where(filtro).Count();
            }
        }

        /// <summary>
        ///     Busca o objeto no banco de dados, pelo Id desse objeto.
        /// </summary>
        /// <param name="id">
        ///     Id do objeto em questão.
        /// </param>
        /// <returns>
        ///     Obejto em questão.
        /// </returns>
        public virtual TEntity FindById(int id)
        {
            return DbSet.Where(z => !z.Removido).FirstOrDefault(e => e.Id == id);
        }

        /// <summary>
        /// Obtem um registro do banco sem incerilo no contexto de monitoramento
        /// </summary>
        /// <param name="id">ID do objeto a ser recuperado</param>
        /// <returns>
        /// Objeto encontrado no banco
        /// </returns>
        public virtual TEntity FindByIdWithNoTracking(int id)
        {
            return DbSet.AsNoTracking().Where(z => !z.Removido).FirstOrDefault(lp => lp.Id == id);
        }

        /// <summary>
        ///     Salva no banco de dados mais especificamente na tabela referente a entidade enviada por paramentro
        /// </summary>
        /// <param name="obj">
        ///     objeto de uma entidade.
        /// </param>
        /// <returns>
        ///     Retorna o mesmo objeto armazenado.
        /// </returns>
        public virtual TEntity Save(TEntity obj)
        {

            var objAdd = DbSet.Add(obj);
            SaveChanges();
            return objAdd;
        }

        /// <summary>
        ///     Salva no banco de dados mais especificamente na tabela referente a entidade enviada por paramentro, um lote de dados.
        /// </summary>
        /// <param name="obj">
        ///     Lote de objetos de uma entidade.
        /// </param>
        /// <returns>
        ///     Retorna o mesmo lote armazenado.
        /// </returns>
        public virtual void SaveBach(IEnumerable<TEntity> objs)
        {
            foreach (var obj in objs)
            {
                if (obj.Id > 0)
                {
                    Update(obj);
                }
                else
                {
                    Save(obj);
                }

            }
            SaveChanges();
        }

        /// <summary>
        ///     Atualiza o banco de dados mais especificamente na tabela referente a entidade envia por parametro.
        /// </summary>
        /// <param name="obj">
        ///     Qualquer objeto que Extende a Classe Entity
        /// </param>
        /// <returns>
        ///     Retorna uma instancia do mesmo tipo que foi enviado
        /// </returns>
        public virtual TEntity Update(TEntity obj)
        {
            var entry = Db.Entry(obj);
            var attachedEntity = Db.ChangeTracker.Entries<TEntity>().FirstOrDefault(e => e.Entity.Id == obj.Id);
            if (attachedEntity != null)
            {
                Db.Entry<TEntity>(attachedEntity.Entity).State = EntityState.Modified;
            }
            else
            {
                DbSet.Attach(obj);
                entry.State = EntityState.Modified;
            }
            SaveChanges();
            return obj;
        }

        /// <summary>
        ///     Remove registro no banco de dados que contem o id informado, na tabela refente a entidade
        /// </summary>
        /// <param name="id">
        ///     Id do objeto que deseja remover
        /// </param>
        public virtual void RemoveById(int id)
        {
            var obj = DbSet.Find(id);
            obj.Removido = true;
            Update(obj);
            SaveChanges();
        }


        /// <summary>
        /// Remove por Varios elementos pelos seus Ids
        /// </summary>
        /// <param name="ids"></param>
        public virtual void RemoveRange(int[] ids)
        {
            foreach (var id in ids)
            {
                RemoveById(id);
            }
            SaveChanges();
        }

        public virtual void RemoveMany(int[] ids)
        {
            foreach (var id in ids)
            {
                DbSet.Remove(DbSet.Find(id));
            }
            SaveChanges();

        }
    }
}
