using System;
using System.Collections.Generic;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using RestApi.Models;

namespace RestApi.Mappings
{
    public sealed class Repository
    {
        private ISessionFactory _sessionFactory;
        private ISession _session;

        private Repository()
        {
            InitializeSession();
        }
        
        public static Repository Instance { get; } = new Repository();

        private void InitializeSession()
        {
            try
            {
                _sessionFactory = Fluently.Configure()
                    .Database(PostgreSQLConfiguration.PostgreSQL82
                        .ConnectionString(
                            "Server=localhost;Database=nhibernate;User ID=dbuser;Password=dbpass;Enlist=true;"))
                    .Mappings(m => m
                        .FluentMappings.AddFromAssemblyOf<Repository>())
                    .BuildSessionFactory();
                _session = _sessionFactory.OpenSession();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public IList<Recipe> GetAllRecipes()
        {
            return _session.QueryOver<Recipe>().List();
        }

        public Recipe GetRecipe(Guid id)
        {
            return _session.Get<Recipe>(id);
        }

        public Recipe AddRecipe(Recipe recipe)
        {
            using (var transaction = _session.BeginTransaction())
            {
                Recipe entity = null;
                try
                {
                    recipe.RecipeId = Guid.NewGuid();
                    recipe.ModifyDate = DateTime.Now;
                    entity = _session.Save(recipe) as Recipe;
                    transaction.Commit();
                }
                catch (StaleObjectStateException)
                {
                    try
                    {
                        entity = _session.Merge(recipe);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                return entity;
            }
        }

        public void UpdateRecipe(Recipe recipe)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _session.Update(recipe);
                    transaction.Commit();
                }
                catch (StaleObjectStateException)
                {
                    try
                    {
                        _session.Merge(recipe);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

            }
        }

        public void DeleteRecipe(Guid id)
        {
            using (var transaction = _session.BeginTransaction())
            {
                var recipe = _session.Get<Recipe>(id);
                if (recipe == null) return;
                try
                {
                    _session.Delete(recipe);
                    transaction.Commit();
                }
                catch (StaleObjectStateException)
                {
                    try
                    {
                        _session.Merge(recipe);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}