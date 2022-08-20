namespace SigmaDiTests.FakeData
{
    public interface IDataProvider
    {
        string GetProduct();
    }

    public class DataProvider : IDataProvider
    {
        public string GetProduct()
        {
            return "Samsung";
        }
    }

    public class TwoConstructor : IDataProvider
    {
        public TwoConstructor(string val)
        {

        }

        public TwoConstructor(int val)
        {

        }

        public string GetProduct()
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IBusinessLayer
    {
        string GetProductName();
    }

    public class BusinessLayer : IBusinessLayer
    {
        private readonly IDataProvider _dataProvider;

        public BusinessLayer(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public string GetProductName()
        {
            return "BL " + _dataProvider.GetProduct();
        }
    }
}
