using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;

namespace PsychoTestWeb.Models
{
    public class Service
    {
        IMongoCollection<User> Users; // коллекция в базе данных
        IMongoCollection<Patient> Patients;
        IMongoCollection<Test> Tests;
        IMongoCollection<BsonDocument> TestsBson;
        IMongoCollection<BsonDocument> NormsBson;

        IMongoDatabase database;
        public Service()
        {
            string connectionString = "mongodb://localhost:27017/MobilePsychoTest";
            var connection = new MongoUrlBuilder(connectionString);
            // получаем клиента для взаимодействия с базой данных
            MongoClient client = new MongoClient(connectionString);
            // получаем доступ к самой базе данных
            database = client.GetDatabase(connection.DatabaseName);
            // обращаемся к коллекциям
            Users = database.GetCollection<User>("Users");
            Patients = database.GetCollection<Patient>("Patients");
            Tests = database.GetCollection<Test>("Tests");
            TestsBson = database.GetCollection<BsonDocument>("Tests");
            NormsBson = database.GetCollection<BsonDocument>("Norms");
        }

        //пользователи
        #region
        //список всех пользователей
        public async Task<IEnumerable<User>> GetUsers()
        {
            // строитель фильтров
            var builder = new FilterDefinitionBuilder<User>();
            var filter = builder.Empty; // фильтр для выборки всех документов
            return await Users.Find(filter).ToListAsync();
        }
        //пользователь по логину и паролю
        public User GetIdentityUsers(string username, string password)
        {
            // строитель фильтров
            var builder = new FilterDefinitionBuilder<User>();
            var filter = builder.Empty;
            var people = Users.Find(filter).ToList();
            return people.FirstOrDefault(x => x.login == username && x.password == password);
        }
        //получаем количество страниц с пользователями, если на странице 10 пользователей
        public async Task<double> GetUsersPagesCount()
        {
            var builder = new FilterDefinitionBuilder<User>();
            var filter = builder.Empty;
            long count = await Users.CountDocumentsAsync(filter);
            return Math.Ceiling((double)count / 10.0);
        }
        //получаем часть пользователей для пагинации
        public async Task<IEnumerable<User>> GetUsersWithCount(int pageNumber)
        {
            var builder = new FilterDefinitionBuilder<User>();
            var filter = builder.Empty;
            List<User> allUsers = await Users.Find(filter).ToListAsync();
            int start = (pageNumber - 1) * 10;
            int count = 10;
            if (start + count >= allUsers.Count)
                count = allUsers.Count - start;
            User[] page = new User[count];
            allUsers.CopyTo(start, page, 0, count);
            return page;
        }
        //фильтрация пользователей по имени
        public async Task<IEnumerable<User>> GetUsersByName(string value)
        {
            var builder = new FilterDefinitionBuilder<User>();
            var filter = builder.Empty;
            var allUsers = await Users.Find(filter).ToListAsync();
            return allUsers.FindAll(x => x.name.ToLower().Contains(value.ToLower()) == true);
        }
        //получаем количество страниц с пользователями c фильтрацией по имени, если на странице 10 пользователей
        public async Task<double> GetUsersByNamePagesCount(string value)
        {
            var users = await GetUsersByName(value);
            users = users.ToList();
            long count = users.Count();
            return Math.Ceiling((double)count / 10.0);
        }
        //получаем часть пользователей c фильтрацией по имени для пагинации
        public async Task<IEnumerable<User>> GetUsersByNameWithCount(int pageNumber, string name)
        {
            List<User> users = new List<User>();
            var u = await GetUsersByName(name);
            users = u.ToList();
            int start = (pageNumber - 1) * 10;
            int count = 10;
            if (start + count >= users.Count)
                count = users.Count - start;
            User[] page = new User[count];
            users.CopyTo(start, page, 0, count);
            return page;
        }

        //получаем пользователя по id
        public async Task<User> GetUserById(string id)
        {
            return await Users.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }
        // добавление пользователя
        public async Task CreateUser(User u)
        {
            await Users.InsertOneAsync(u);
        }
        // обновление пользователя
        public async Task UpdateUser(string id, User u)
        {
            BsonDocument doc = new BsonDocument("_id", new ObjectId(id));
            await Users.ReplaceOneAsync(doc, u);
        }
        // удаление пользователя
        public async Task RemoveUser(string id)
        {
            await Users.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }
        #endregion

        //пациенты
        #region
        //список всех пациентов
        public async Task<IEnumerable<Patient>> GetPatients()
        {
            var builder = new FilterDefinitionBuilder<Patient>();
            var filter = builder.Empty;
            return await Patients.Find(filter).ToListAsync();
        }
        //получаем пациента по id
        public async Task<Patient> GetPatientById(string id)
        {
            return await Patients.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }
        //получаем пациента по токену
        public async Task<Patient> GetPatientByToken(string token)
        {
            return await Patients.Find(new BsonDocument("token", token)).FirstOrDefaultAsync();
        }
        //получаем количество страниц с пациентами, если на странице 10 пациентов
        public async Task<double> GetPatientsPagesCount()
        {
            var builder = new FilterDefinitionBuilder<Patient>();
            var filter = builder.Empty;
            long count = await Patients.CountDocumentsAsync(filter);
            return Math.Ceiling((double)count / 10.0);
        }
        //получаем часть пациентов для пагинации
        public async Task<IEnumerable<Patient>> GetPatientsWithCount(int pageNumber)
        {
            var builder = new FilterDefinitionBuilder<Patient>();
            var filter = builder.Empty;
            List<Patient> allPatients = await Patients.Find(filter).ToListAsync();
            int start = (pageNumber - 1) * 10;
            int count = 10;
            if (start + count >= allPatients.Count)
                count = allPatients.Count - start;
            Patient[] page = new Patient[count];
            allPatients.CopyTo(start, page, 0, count);
            return page;
        }
        //фильтрация по имени
        public async Task<IEnumerable<Patient>> GetPatientsByName(string value)
        {
            var builder = new FilterDefinitionBuilder<Patient>();
            var filter = builder.Empty;
            var allPatients = await Patients.Find(filter).ToListAsync();
            return allPatients.FindAll(x => x.name.ToLower().Contains(value.ToLower()) == true);
        }
        //получаем количество страниц с пациентами c фильтрацией по имени, если на странице 10 пациентов
        public async Task<double> GetPatientsByNamePagesCount(string value)
        {
            var patients = await GetPatientsByName(value);
            patients = patients.ToList();
            long count = patients.Count();
            return Math.Ceiling((double)count / 10.0);
        }
        //получаем часть пациентов c фильтрацией по имени для пагинации
        public async Task<IEnumerable<Patient>> GetPatientsByNameWithCount(int pageNumber, string name)
        {
            List<Patient> patients = new List<Patient>();
            var p = await GetPatientsByName(name);
            patients = p.ToList();
            int start = (pageNumber - 1) * 10;
            int count = 10;
            if (start + count >= patients.Count)
                count = patients.Count - start;
            Patient[] page = new Patient[count];
            patients.CopyTo(start, page, 0, count);
            return page;
        }
        // добавление пациента
        public async Task<string> CreatePatient(Patient p)
        {
            p.token = GenerateToken();
            await Patients.InsertOneAsync(p);
            return p.token;
        }
        // обновление пациента
        public async Task UpdatePatient(string id, Patient p)
        {
            BsonDocument doc = new BsonDocument("_id", new ObjectId(id));
            await Patients.ReplaceOneAsync(doc, p);
        }
        // удаление пациента
        public async Task RemovePatient(string id)
        {
            await Patients.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }

        #endregion

        //Связь и обработка
        #region

        // Generate a fixed length token
        public string GenerateToken(int numberOfBytes = 32)
        {
            return WebEncoders.Base64UrlEncode(GenerateRandomBytes(numberOfBytes));
        }
        // Generate a cryptographically secure array of bytes with a fixed length
        private static byte[] GenerateRandomBytes(int numberOfBytes)
        {
            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                byte[] byteArray = new byte[numberOfBytes];
                provider.GetBytes(byteArray);
                return byteArray;
            }
        }
        //получаем пациента по токену аутентификации
        public async Task<Patient> AuthenticationPatient(string token)
        {
            return await Patients.Find(new BsonDocument("token", token)).FirstOrDefaultAsync();
        }


        //обработка полученных результатов теста
        public async Task ProcessingResults(TestsResult result, Patient patient)
        {
            //сразу удаляем тест из доступных
            patient.tests.Remove(result.id);
            await UpdatePatient(patient.id, patient);

            var doc = await TestsBson.Find(new BsonDocument("_id", new ObjectId(result.id))).FirstOrDefaultAsync();
            if (doc != null)
            {
                var dotNetObj = BsonTypeMapper.MapToDotNetValue(doc);
                var json = JsonConvert.SerializeObject(dotNetObj);

                //пройденный тест
                var test = JObject.Parse(json);
                //норма для данного теста
                var norm = await NormsBson.Find(new BsonDocument("main.groups.item.id", test["IR"]["ID"].ToString())).FirstOrDefaultAsync();

                //обработка результатов
                ProcessingResults processingResults = new ProcessingResults(test, result, norm);
                DateTime now = DateTime.Now;
                processingResults.patientResult.date = now.ToString("g");
                processingResults.patientResult.comment = "";
                processingResults.patientResult.test = result.id;

                //добавление в бд
                patient.results.Add(processingResults.patientResult);
                await UpdatePatient(patient.id, patient);
            }
        }


        
        #endregion

        //Тесты
        #region
        //получаем краткий список всех тестов в формате id-название-заголовок-инструкция
        public async Task<IEnumerable<Test>> GetTestsView()
        {
            var documents = await TestsBson.Find(new BsonDocument()).ToListAsync();
            List<Test> tests = new List<Test>();
            foreach (BsonDocument doc in documents)
            {
                tests.Add(new Test
                {
                    name = doc["IR"]["Name"]["#text"].AsString,
                    id = doc["_id"].AsObjectId.ToString(),
                    title = doc["IR"]["Title"]["#text"].AsString,
                    instruction = doc["Instruction"]["#text"].AsString
                });
            }
            return tests;
        }

        //// получаем все тесты
        //public async Task<List<object>> GetTests()
        //{
        //    var documents = await TestsBson.Find(new BsonDocument()).ToListAsync();
        //    var dotNetObjList = documents.ConvertAll(BsonTypeMapper.MapToDotNetValue);
        //    return dotNetObjList;
        //}


        //получаем тест по id
        public async Task<string> GetTestById(string id)
        {
            var bsonDoc = await TestsBson.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
            var dotNetObj = BsonTypeMapper.MapToDotNetValue(bsonDoc);
            return JsonConvert.SerializeObject(dotNetObj);
        }

        //получаем все назначенные пациенту тесты 
        public async Task<IEnumerable<Test>> GetTestsByPatientToken(Patient patient)
        {
            List<Test> tests = new List<Test>();
            var documents = await TestsBson.Find(new BsonDocument()).ToListAsync();
            if (patient.tests != null)
            {
                foreach (string idTest in patient.tests)
                {
                    foreach (BsonDocument doc in documents)
                    {
                        if (idTest == doc["_id"].AsObjectId.ToString())
                        {
                            tests.Add(new Test
                            {
                                name = doc["IR"]["Name"]["#text"].AsString,
                                id = doc["_id"].AsObjectId.ToString(),
                                title = doc["IR"]["Title"]["#text"].AsString,
                                instruction = doc["Instruction"]["#text"].AsString
                            });
                        }
                    }
                }
            }
            return tests;
        }

        //Импорт теста
        public async Task<string> ImportTestFile(string file)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(file);
            var json = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.None, true);
            var jObj = JObject.Parse(json);

            var test =  await TestsBson.Find(new BsonDocument("IR.ID", jObj["IR"]["ID"].ToString())).FirstOrDefaultAsync();
            if (test == null)
            {
                int i = 0;
                foreach (var question in jObj["Questions"]["item"])
                {
                    question["Question_id"] = i;
                    i++;

                    //если вопрос только с текстом — Question_Type = 0, если с изображением Question_Type = 1
                    if (question["ImageFileName"] != null)
                        question["Question_Type"] = 1;
                    else
                        question["Question_Type"] = 0;

                    //если вопрос c выбором одного отета — Question_Choice = 1, если с вводом своего — Question_Choice = 0
                    if (question["TypeQPanel"] != null)
                    {
                        if (question["TypeQPanel"].ToString() == "2" || question["AnsString_Num"] != null ||
                                    question["AnsString_ExanineAnsMethod"] != null || question["Ans_CheckUpperCase"] != null)
                        {
                            question["Question_Choice"] = 0;
                            if (question["Answers"]["item"] is JArray)
                                foreach (var answer in question["Answers"]["item"])
                                    answer["Answer_Type"] = 1;
                            else
                            {
                                JArray arr = new JArray();
                                question["Answers"]["item"]["Answer_Type"] = 1;
                                arr.Add(question["Answers"]["item"]);
                                question["Answers"]["item"] = arr;
                            }

                        }
                        else
                        {
                            question["Question_Choice"] = 1;
                            if (question["Answers"]["item"] is JArray)
                                foreach (var answer in question["Answers"]["item"])
                                    answer["Answer_Type"] = 0;
                            else
                            {
                                JArray arr = new JArray();
                                question["Answers"]["item"]["Answer_Type"] = 0;
                                arr.Add(question["Answers"]["item"]);
                                question["Answers"]["item"] = arr;
                            }
                        }
                    }
                    else
                    {
                        question["Question_Choice"] = 1;
                        if (question["Answers"]["item"] is JArray)
                            foreach (var answer in question["Answers"]["item"])
                                answer["Answer_Type"] = 0;
                        else
                        {
                            JArray arr = new JArray();
                            question["Answers"]["item"]["Answer_Type"] = 0;
                            arr.Add(question["Answers"]["item"]);
                            question["Answers"]["item"] = arr;
                        }
                    }
                    int j = 0;
                    if (question["Answers"]["item"] is JArray)
                        foreach (var answer in question["Answers"]["item"])
                        {
                            answer["Answer_id"] = j;
                            j++;
                            //если ответ это текст — Answer_Type = 0, если это поле для ввода — Answer_Type = 1, если изображение — Answer_Type = 2;
                            if (answer["ImageFileName"] != null)
                                answer["Answer_Type"] = 2;
                        }
                    else
                    {
                        JArray arr = new JArray();
                        question["Answers"]["item"]["Answer_id"] = 0;
                        if (question["Answers"]["item"]["ImageFileName"] != null)
                            question["Answers"]["item"]["Answer_Type"] = 2;
                        arr.Add(question["Answers"]["item"]);
                        question["Answers"]["item"] = arr;
                    }
                }
                var document = BsonDocument.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(jObj));
                await TestsBson.InsertOneAsync(document);
                return jObj["IR"]["ID"].ToString();
            }
            else return null;
        }

        //Импорт норм
        public async Task ImportNormFile(string file, string testId)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(file);
            var json = JsonConvert.SerializeXmlNode(xmlDoc);
            var jObj = JObject.Parse(json);
            jObj["main"]["groups"]["item"]["id"] = testId;
            var document = BsonDocument.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(jObj));
            await NormsBson.InsertOneAsync(document);
        }

        //получаем список id всех норм
        public async Task<IEnumerable<string>> GetNorms()
        {
            var documents = await NormsBson.Find(new BsonDocument()).ToListAsync();
            List<string> norms = new List<string>();
            foreach (BsonDocument doc in documents)
            {
                norms.Add(doc["_id"].AsObjectId.ToString());
            }
            return norms;
        }

        // удаление норм
        public async Task RemoveNorm(string id)
        {
            await NormsBson.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }
        // удаление теста вместе с нормой
        public async Task RemoveTest(string id)
        {
            var test = await TestsBson.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
            await NormsBson.DeleteOneAsync(new BsonDocument("main.groups.item.id", test["IR"]["ID"].AsString));
            await Tests.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }
        #endregion
    }

}