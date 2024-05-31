using Microsoft.EntityFrameworkCore;
using ODTLearning.Entities;
using ODTLearning.Models;


namespace ODTLearning.Repositories
{
    public class TutorListRepository : ITutorListRepository
    {
        private readonly DbminiCapstoneContext _context;
        private readonly int _pageSize = 10;

        public TutorListRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }
        public List<Account> SearchTutorList(string name, string field)
        {
            //list all
            var list = _context.Accounts.Where(x => x.Role == "Tutor");

            //list search by name
            if (!string.IsNullOrEmpty(name))
            {
                list = list.Where(x => x.FisrtName == name || x.LastName == name);
            }

            //list search by field
            if (!string.IsNullOrEmpty(field))
            {                
                var fieldId = _context.Fields.Where(x => x.FieldName == field).Select(x => x.IdField).ToString();
                
                var kq = _context.TutorFields.Where(x => x.IdField == fieldId).Join(_context.Tutors, tf => tf.IdTutor, t => t.IdTutor, (tf, t) => t.IdAccount).ToList();

                IQueryable<Account> a = null;

                foreach (var id in kq)
                {
                    var k = list.Where(x => x.IdAccount == id);
                    a = a.Union(k);
                }

                list = a;
            }

            return list.ToList();
            
        }        
    }
}
