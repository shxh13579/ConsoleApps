using System.Collections.Generic;
using System.Threading.Tasks;
using TestApp.Model;

namespace TestApp.IServices
{
    public interface ITestServices
    {

        Task<List<TestModel>> GetList(string queryValue);

        Task<object> InsertTestRecord(List<TestModel> info);


        /// <summary>
        /// aggregate test
        /// </summary>
        /// <returns></returns>
        Task<object> GetAggregationResult();

        /// <summary>
        /// testing function of update.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        Task<object> UpdatePropertiesForDB(int start, int number);


        /// <summary>
        /// test for group
        /// </summary>
        /// <returns></returns>
        Task<object> GroupData();
    }
}
