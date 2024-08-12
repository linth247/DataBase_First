using System.Text.RegularExpressions;

namespace WebAPI.Parameters
{
    public class NewsParameter
    {
        public string title { get; set; }
        public string content { get; set; }
        public DateTime? startDateTime { get; set; }
        public int? minOrder { get; set; }
        public int? maxOrder { get; set; }

        private string _order;
        public string Order
        {
            get { return _order; }
            set
            {
                //_order = value; 
                Regex regex = new Regex(@"^\d*-\d$");
                //if (regex.Match(value).Success)
                if (regex.IsMatch(value))
                {
                    minOrder = Int32.Parse(value.Split('-')[0]);
                    maxOrder = Int32.Parse(value.Split('-')[1]);
                }
                _order = value; 
            }
        }
    }
}
