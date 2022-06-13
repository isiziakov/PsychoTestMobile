using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace PsychoTestWeb.Models
{
    public class Service
    {
        IMongoCollection<User> Users; // коллекция в базе данных
        IMongoCollection<Patient> Patients;
        IMongoCollection<Test> Tests;
        IMongoCollection<BsonDocument> TestsBson;
        public Service()
        {
            // строка подключения
            string connectionString = "mongodb://localhost:27017/MobilePsychoTest";
            var connection = new MongoUrlBuilder(connectionString);
            // получаем клиента для взаимодействия с базой данных
            MongoClient client = new MongoClient(connectionString);
            // получаем доступ к самой базе данных
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            // обращаемся к коллекциям
            Users = database.GetCollection<User>("Users");
            Patients = database.GetCollection<Patient>("Patients");
            Tests = database.GetCollection<Test>("Tests");
            TestsBson = database.GetCollection<BsonDocument>("Tests");
        }
        // получаем всех пользователей
        public IEnumerable<User> GetUsers()
        {
            // строитель фильтров
            var builder = new FilterDefinitionBuilder<User>();
            var filter = builder.Empty; // фильтр для выборки всех документов

            return Users.Find(filter).ToList();
        }
        // получаем всех пациетов
        public IEnumerable<Patient> GetPatients()
        {
            var builder = new FilterDefinitionBuilder<Patient>();
            var filter = builder.Empty;
            return Patients.Find(filter).ToList();
        }
        // фильтрация пациентов по имени
        public IEnumerable<Patient> GetPatientsByName(string value)
        {
            var builder = new FilterDefinitionBuilder<Patient>();
            var filter = builder.Empty;
            var allPatients = Patients.Find(filter).ToList();
            if (value != "")
                return allPatients.FindAll(x => x.name.ToLower().Contains(value.ToLower()) == true);
            else return allPatients;
        }
        //получаем пациента по id
        public Patient GetPatientById(string id)
        {
            return Patients.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefault();
        }
        // добавление пациента
        public void CreatePatient(Patient p)
        {
            Patients.InsertOne(p);
        }
        // обновление пациента
        public void UpdatePatient(string id, Patient p)
        {
            BsonDocument doc = new BsonDocument("_id", new ObjectId(id));
            Patients.ReplaceOne(doc, p);
        }
        // удаление пациента
        public void RemovePatient(string id)
        {
            Patients.DeleteOne(new BsonDocument("_id", new ObjectId(id)));
        }
        // получаем все тесты
        public IEnumerable<Test> GetTests()
        {
            var builder = new FilterDefinitionBuilder<Test>();
            var filter = builder.Empty;
            return Tests.Find(filter).ToList();
        }
        // фильтрация тестов по имени
        public IEnumerable<Test> GetTestsByName(string value)
        {
            var builder = new FilterDefinitionBuilder<Test>();
            var filter = builder.Empty;
            var allPatients = Tests.Find(filter).ToList();
            return allPatients.FindAll(x => x.name.ToLower().Contains(value.ToLower()) == true);
        }

        public void ImportFile(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(file);
            var json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, true);
            //string json = JsonConvert.SerializeXmlNode(file);
            BsonDocument bsonDoc = BsonDocument.Parse(json);
            TestsBson.InsertOne(bsonDoc);
        }
    }

}