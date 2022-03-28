using SquareAPI.Business;

namespace SquareAPI.Web {
    public class UserInput {
        public UserInput() {
            Points = new List<Point>();
        }

        public int UserId {get;set;}
        public List<Point> Points {get;set;}
    }
}