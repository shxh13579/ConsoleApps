using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.IServices;
using TestApp.Model;

namespace TestApp.DBContext
{
    public class TestServices : ITestServices
    {

        private readonly IMongoCollection<TestModel> _testmodelCollection;

        private readonly IMongoCollection<TestModel> _groupCollection;

        /// <summary>
        /// service for test
        /// </summary>
        public TestServices(IOptions<TestSettingModel> opt)
        {
            var client = new MongoClient(opt.Value.ConnectionString);
            var database = client.GetDatabase(opt.Value.DatabaseName);
            //database.CreateCollection("GroupCollection");
            _groupCollection = database.GetCollection<TestModel>("GroupCollection");
            _testmodelCollection = database.GetCollection<TestModel>("TestCollection");
        }

        #region boring CRUD
        public async Task<List<TestModel>> GetList(string queryValue)
        {
            try
            {
                var data = _testmodelCollection.Find(x => x.Value.Contains(queryValue));
                var result = await data.ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<object> InsertTestRecord(List<TestModel> info)
        {
            try
            {
                foreach (var ele in info)
                {
                    if (ele._id == default)
                    {
                        ele._id = ObjectId.GenerateNewId();
                    }
                }
                await _testmodelCollection.InsertManyAsync(info);
                return "success";
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// aggregate test
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetAggregationResult()
        {
            try
            {
                var project1 = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"_id", "$_id"},
                                {"Type","$Type"},
                                {"Value","$Value" },
                                {"BackupData","$BackupData" },
                                {"Count", 1},
                            }
                    }
                };

                var project2 = new BsonDocument
                {
                    {
                        "$project",
                        new BsonDocument
                            {
                                {"_id", "$_id"},
                                {"Type","$Type"},
                                {"Value","$Value" },
                                {"Count", 1},
                            }
                    }
                };

                var filter = new BsonDocument {
                    {
                        "$match",
                        new BsonDocument
                        {
                            {
                                "Type",new BsonDocument
                                {
                                    {
                                        "$gt",0
                                    }
                                }
                            },
                            {
                                "Value","Ash Nazg"
                            }
                        }
                    }
                };

                var limit = new BsonDocument {
                    {
                    "$limit",10
                    }
                };

                var group = new BsonDocument
                {
                    {
                        "$group",new BsonDocument
                        {
                            {"_id","$Type" },
                            {"count",new BsonDocument{{"$sum",1}} }
                        }
                    }
                };
                PipelineDefinition<TestModel, BsonDocument> pipelines = new[] { group };
                var result = _testmodelCollection.Aggregate<BsonDocument>(pipelines);

                var res1 = _testmodelCollection.AsQueryable<TestModel>().Where(x => x.BackupData != null);
                var res2 = _groupCollection.AsQueryable<TestModel>();
                var res = from r1 in res1
                          join r2 in res2 on r1.BackupData equals r2.BackupData into rs1
                          from rsc1 in rs1.DefaultIfEmpty()
                          select new
                          {
                              T1 = r1.Type,
                              T2 = rsc1.BackupData,
                              T3 = r1._id
                          };

                ////PipelineDefinition<TestModel,BsonDocument> testPipeline = new 
                //PipelineDefinition<TestModel, BsonDocument> pipeline = new PipelineStagePipelineDefinition<TestModel, BsonDocument>(stage1);

                //_testmodelCollection.Aggregate(pipeline);
                //res
                return res.ToList();
            }
            catch (NotSupportedException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// test for group
        /// </summary>
        /// <returns></returns>
        public async Task<object> GroupData()
        {
            try
            {
                var doc = new BsonDocument
                {
                    {
                        "$group",new BsonDocument{
                        { "_id","$Value"}
                    }
                    }
                };
                var pipeline = new[] { doc };
                var ret = await _groupCollection.AggregateAsync<BsonDocument>(pipeline, new AggregateOptions { AllowDiskUse = true });
                return await ret.ToListAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// try to add some info into  documents.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public async Task<object> UpdatePropertiesForDB(int start, int number)
        {
            try
            {
                var data = _testmodelCollection.AsQueryable().Skip(start).Take(number);
                foreach (var testModel in data)
                {
                    testModel.BackupData = "aabbccddee";
                }
                var result = await _testmodelCollection.UpdateManyAsync(x => x.Value.Contains("5eec8aa"), Builders<TestModel>.Update.Set(y => y.BackupData, "testDataInfo"));
                return result;
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}
