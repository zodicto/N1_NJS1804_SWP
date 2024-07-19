namespace ODTLearning.BLL.Helpers
{
    public class MyLibrary
    {
        public string DeleteLastIndexString(string x)
        {
            if (x.Length > 1)
            {
                x = x.Substring(0, x.Length - 1);
            }

            return x;
        }
    }
}
