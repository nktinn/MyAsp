using MyAsp.Storage;

namespace MyAsp.Logic.Files
{
    public class FileManager: IFileManager
    {
        private readonly Context _context;
        public FileManager(Context context)
        {
            _context = context;
        }

        public async Task UpdatePhoto(int id, string file)
        {
            var IsPM = _context.PM.FirstOrDefault(p => p.UserID == id);
            if (IsPM != null)
            {
                await RejectPhoto(IsPM.Id);
            }
            PhotoManagement pm = new PhotoManagement();
            pm.UserID = id;
            pm.File = file;
            pm.Date = DateTime.Now;
            _context.PM.Add(pm);
            await _context.SaveChangesAsync();
        }

        public async Task AcceptPhoto(int id)
        {
            var photo = _context.PM.FirstOrDefault(ph => ph.Id == id);
            if (photo != null)
            {
                var account = _context.Users.FirstOrDefault(u => u.Id == photo.UserID);
                if (account.Photo != "alt.png")
                {
                    string prevPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Avatars/", account.Photo);

                    try
                    {
                        FileInfo fileInfo = new FileInfo(prevPath);
                        if (fileInfo.Exists)
                            fileInfo.Delete();
                    }
                    catch (Exception ex)
                    { }
                }
                account.Photo = photo.File;
                _context.PM.Remove(photo);
                await _context.SaveChangesAsync();
            }
        }
        public async Task RejectPhoto(int id)
        {
            var photo = _context.PM.FirstOrDefault(ph => ph.Id == id);
            if (photo != null)
            {
                if (photo.File != "alt.png")
                {
                    string prevPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Avatars/", photo.File);

                    try
                    {
                        FileInfo fileInfo = new FileInfo(prevPath);
                        if (fileInfo.Exists)
                            fileInfo.Delete();
                    }
                    catch (Exception ex)
                    { }
                }

                _context.PM.Remove(photo);
                await _context.SaveChangesAsync();
            }
        }
        public async Task RejectPhotoByUserId(int id)
        {
            var photo = _context.PM.FirstOrDefault(ph => ph.UserID == id);

            if (photo != null)
            {
                if (photo.File != "alt.png")
                {
                    string prevPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Avatars/", photo.File);

                    try
                    {
                        FileInfo fileInfo = new FileInfo(prevPath);
                        if (fileInfo.Exists)
                            fileInfo.Delete();
                    }
                    catch (Exception ex)
                    { }
                }

                _context.PM.Remove(photo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
