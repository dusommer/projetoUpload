using Upload.Infra.Persistence;

namespace Upload.Infra.Transaction
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UploadContext _context;

        public UnitOfWork(UploadContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
