namespace MyAsp.Logic.Files
{
    public interface IFileManager
    {
        public Task UpdatePhoto(int id, string file);
        public Task AcceptPhoto(int id);
        public Task RejectPhoto(int id);
    }
}
