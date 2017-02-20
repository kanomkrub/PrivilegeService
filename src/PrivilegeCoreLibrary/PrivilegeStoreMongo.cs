using MongoDB.Driver;
using PrivilegeCoreLibrary.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace PrivilegeCoreLibrary
{
    public class PrivilegeStoreMongo : IPrivilegeStore
    {
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;
        public PrivilegeStoreMongo(NameValueCollection parameter)
        {
            var database = parameter["database"];
            var username = parameter["username"];
            var password = parameter["password"];
            //try
            //{
            //    password = BasicCrypto.DecryptLocal(password);
            //}
            //catch (Exception) { }
            var host = parameter["host"];
            var port = int.Parse(parameter["port"]);
            //_client = new MongoClient(
            //new MongoClientSettings
            //{Credentials = new[]
            //     {MongoCredential.CreateCredential(database,username,password)},
            //    Server = new MongoServerAddress(host, port)});
            _client = new MongoClient($"mongodb://{host}:{port}/{database}");
            _database = _client.GetDatabase(database);
        }


        public void CreateCompany(Company anno)
        {
            anno.id = Guid.NewGuid().ToString();
            var collection = _database.GetCollection<Company>(PrivilegeContentCollections.company);
            collection.InsertOneAsync(anno).Wait();
        }

        public bool UpdateCompany(Company company)
        {
            var collection = _database.GetCollection<Company>(PrivilegeContentCollections.company);
            return collection.ReplaceOneAsync(Builders<Company>.Filter.Eq(t => t.name, company.name), company).Result.MatchedCount > 0;
        }

        public bool DeleteCompany(string id)
        {
            var collection = _database.GetCollection<Company>(PrivilegeContentCollections.company);
            var filter = Builders<Company>.Filter.Eq(t => t.id, id);
            return collection.DeleteOneAsync(filter).Result.DeletedCount > 0;
        }
        public Company GetCompany(string name)
        {
            var collection = _database.GetCollection<Company>(PrivilegeContentCollections.company);
            var filter = Builders<Company>.Filter.Eq(t => t.name, name);
            return collection.Find(filter).SingleAsync().Result;
        }

        public void CreateCustomer(Customer customer)
        {
            customer.id = Guid.NewGuid().ToString();
            var collection = _database.GetCollection<Customer>(PrivilegeContentCollections.customer);
            collection.InsertOneAsync(customer).Wait();
        }

        public Customer GetCustomer(string email)
        {
            var collection = _database.GetCollection<Customer>(PrivilegeContentCollections.customer);
            var filter = Builders<Customer>.Filter.Eq(t => t.email, email);
            return collection.Find(filter).SingleOrDefaultAsync().Result;
        }

        public bool UpdateCustomer(Customer customer)
        {
            var collection = _database.GetCollection<Customer>(PrivilegeContentCollections.customer);
            return collection.ReplaceOneAsync(Builders<Customer>.Filter.Eq(t => t.email, customer.email),customer).Result.MatchedCount > 0;
        }

        public bool DeleteCustomer(string id)
        {
            var collection = _database.GetCollection<Customer>(PrivilegeContentCollections.customer);
            var filter = Builders<Customer>.Filter.Eq(t => t.id, id);
            return collection.DeleteOneAsync(filter).Result.DeletedCount > 0;
        }
        
        public void CreateCompanyStaff(Company company)
        {
            throw new NotImplementedException();
        }
        public Staff GetCompanyStaff(string email)
        {
            var collection = _database.GetCollection<Staff>(PrivilegeContentCollections.staff);
            var filter = Builders<Staff>.Filter.Eq(t => t.email, email);
            return collection.Find(filter).SingleOrDefaultAsync().Result;
        }

        /// <summary>
        /// get promotion by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Promotion GetPromotion(string id)
        {
            var collection = _database.GetCollection<Promotion>(PrivilegeContentCollections.promotion);
            var filter = Builders<Promotion>.Filter.Eq(t => t.id, id);
            return collection.Find(filter).SingleOrDefaultAsync().Result;
        }

        /// <summary>
        /// get promotion by company code
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public List<Promotion> GetPromotions(string company)
        {
            var result = new List<Promotion>();
            var collection = _database.GetCollection<Promotion>(PrivilegeContentCollections.promotion);
            var filter = Builders<Promotion>.Filter.Eq(t => t.company, company);
            collection.FindAsync<Promotion>(filter).Result.ForEachAsync(t => result.Add(t)).Wait();
            return result;
        }

        public bool UpdatePromotion(Promotion promotion)
        {
            var collection = _database.GetCollection<Promotion>(PrivilegeContentCollections.promotion);
            return collection.ReplaceOneAsync(Builders<Promotion>.Filter.Eq(t => t.id, promotion.id), promotion).Result.MatchedCount > 0;
        }
        public void CreatePromotion(Promotion promotion)
        {
            promotion.id = Guid.NewGuid().ToString();
            var collection = _database.GetCollection<Promotion>(PrivilegeContentCollections.promotion);
            collection.InsertOneAsync(promotion).Wait();
        }
        public bool DeletePromotion(string id)
        {
            var collection = _database.GetCollection<Promotion>(PrivilegeContentCollections.promotion);
            var filter = Builders<Promotion>.Filter.Eq(t => t.id, id);
            return collection.DeleteOneAsync(filter).Result.DeletedCount > 0;
        }

        public List<Branch> GetNearbyBranchs(float longtitude, float latitude, float maxdistance)
        {
            return null;
               //         db.branch.find(
               //{loc:{ $near:{
               //         $geometry:
               //                     {type: "Point" ,
               //            coordinates: [50, 51] },
               //         $maxDistance: 500000}}})
        }
        public List<object> GetNearbyBranchWithDistance(float longtitude, float latitude, float maxdistance)
        {
            return null;
            //var odd = new { geoNear = "branch",
            //     near = [54, 40.74],
            //     spherical = true
            //   };
        //_database.RunCommand(new MongoDB.Driver.JsonCommand { }"{ geoNear: "branch",near: [54, 40.74],spherical: true}}");
            //db.runCommand( {
            //    geoNear: "branch",
            //     near: [54, 40.74],
            //     spherical: true
            //   }  )
        }
    }
}
