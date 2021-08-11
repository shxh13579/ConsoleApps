using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp.IServices;
using TestApp.Model;

namespace TestApp
{
    /// <summary>
    /// 
    /// </summary>
    [Route("MoqTestRequest")]
    public class MoqTestController : ControllerBase
    {
        private readonly ITestServices _testServices;

        public MoqTestController(ITestServices testServices)
        {
            _testServices = testServices;
        }

        /// <summary>
        /// Get data from mongodb
        /// </summary>
        /// <param name="queryValue"></param>
        /// <returns></returns>
        [HttpGet("GetMongoData")]
        public async Task<List<TestModel>> GetMongoData(string queryValue)
        {
            return await _testServices.GetList(queryValue);
        }

        /// <summary>
        /// try to insert some data
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost("InsertDataList")]
        public async Task<object> InsertTestData([FromBody] List<TestModel> list)
        {
            return await _testServices.InsertTestRecord(list);
        }

        /// <summary>
        /// get aggregate result 
        /// </summary>
        /// <returns></returns>
        [HttpGet("AggregateData")]
        public async Task<object> AggregateData()
        {
            return await _testServices.GetAggregationResult();
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="start"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        [HttpPost("UpdateData")]
        public async Task<object> UpdateData(int start, int number)
        {
            return await _testServices.UpdatePropertiesForDB(start, number);
        }

        /// <summary>
        /// group test
        /// </summary>
        /// <returns></returns>
        [HttpGet("GroupData")]
        public async Task<object> GroupData()
        {
            return await _testServices.GroupData();
        }

        /// <summary>
        /// moqTest
        /// </summary>
        /// <returns></returns>
        [HttpGet("MoqTest")]
        public async Task<object> MoqTest(string str)
        {
            try
            {

                var moq = new Mock<ITestServices>();
                var res = "successsasdasdasdasdasdasdasd";
                var re = new List<TestModel>();
                moq.Setup(foo => foo.GroupData()).ReturnsAsync(() => new List<TestModel>());
                var d = await moq.Object.GroupData();
                var c = await moq.Object.GroupData();

                moq.Verify(x => x.GroupData(), Times.Exactly(2));
                Assert.AreEqual(re, d);
                return true;
            }
            catch (AssertionException e)
            {
                return e.Message;
            }
        }

    }
}
